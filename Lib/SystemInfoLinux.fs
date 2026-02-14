module Lib.SystemInfoLinux

open System
open System.IO
open System.Net
open System.Net.Http
open System.Text.RegularExpressions

open Types

let getDistroId (fileName: string) =
  match File.Exists(fileName) with
  | true ->
    let lines = File.ReadLines(fileName)

    let configProperties =
      lines
      |> Seq.map (fun x ->
        let values = x.Split('=')
        (values[0], values[1]))
      |> Seq.filter (fun (k, v) -> k = "ID")
      |> List.ofSeq


    let distroName =
      match configProperties.IsEmpty with
      | true -> ""
      | false -> snd configProperties.Head

    distroName.Trim('"')
  | false -> ""

let getDistroName (fileName: string) =
  match File.Exists(fileName) with
  | true ->
    let lines = File.ReadLines(fileName)

    let configProperties =
      lines
      |> Seq.map (fun x ->
        let values = x.Split('=')
        (values[0], values[1]))
      |> Seq.filter (fun (k, v) -> k = "PRETTY_NAME")
      |> List.ofSeq


    let distroName =
      match configProperties.IsEmpty with
      | true -> ""
      | false -> snd configProperties.Head

    distroName.Trim('"')
  | false -> ""

let getHostName (fileName: string) =
  match File.Exists(fileName) with
  | true ->
    let lines = File.ReadLines(fileName) |> List.ofSeq

    match lines.IsEmpty with
    | true -> ""
    | false -> lines.Head
  | false -> ""

let getKernel (fileName: string) =
  match File.Exists(fileName) with
  | true ->
    let lines = File.ReadLines(fileName) |> List.ofSeq

    match lines.IsEmpty with
    | true -> ""
    | false -> lines.Head.Split(' ')[2]
  | false -> ""

let toByteSize (value: string) =
  let r = value |> Int32.Parse |> ByteSizeLib.ByteSize.FromKiloBytes
  r.GigaBytes.ToString("0.0") + " GB"

let getMemoryInfo (fileName: string) =
  if File.Exists(fileName) then
    let regexAvailableMem = Regex @"MemAvailable:\s+(\d+)\s+kB"
    let regexTotalMem = Regex @"MemTotal:\s+(\d+)\s+kB"
    let text = File.ReadAllText fileName
    let matchAvailableMem = regexAvailableMem.Match text
    let matchTotalMem = regexTotalMem.Match text

    if matchAvailableMem.Success && matchTotalMem.Success then
      let availMem = matchAvailableMem.Groups.[1].Value |> toByteSize
      let totalMem = matchTotalMem.Groups.[1].Value |> toByteSize
      $"{availMem}/{totalMem}"
    else
      ""
  else
    ""

let getShell (envVar: string) =
  Environment.GetEnvironmentVariable envVar
  |> Option.ofObj
  |> Option.map (fun v -> v.Split('/') |> Array.last)
  |> Option.defaultValue ""

let getLocalIpAddress () =
  let hostName = Dns.GetHostName()
  let addressList = Dns.GetHostEntry(hostName).AddressList |> List.ofArray

  match addressList.IsEmpty with
  | true -> ""
  | false ->
    addressList
    |> List.rev
    |> List.tryItem 2
    |> Option.map string
    |> Option.defaultValue ""

let getPublicIpAddressAsync (ipProvider: string) =
  async {
    use client = new HttpClient()
    return! client.GetStringAsync(ipProvider) |> Async.AwaitTask
  }

let getUptime (fileName: string) =
  match File.Exists(fileName) with
  | true ->
    let lines = File.ReadLines(fileName) |> List.ofSeq

    match lines.IsEmpty with
    | true -> ""
    | false ->
      let uptime = lines[0].Split('.')[0] |> Int32.Parse
      let hours = uptime / 3600
      let minutes = (uptime % 3600) / 60
      $"{hours}h{minutes}m"
  | false -> ""

let getUser (envVar: string) =
  Environment.GetEnvironmentVariable(envVar)

let getCPUModelName (fileName: string) =
  match File.Exists(fileName) with
  | true ->
    let lines = File.ReadLines(fileName) |> List.ofSeq

    match lines.IsEmpty with
    | true -> ""
    | false ->
      let configProperties =
        lines
        |> List.filter (fun line -> line.Contains(':'))
        |> List.map (fun line ->
          let kvPairs = line.Split(':')
          let k, v = kvPairs[0].Trim(), kvPairs[1].Trim()
          k, v)
        |> List.filter (fun (k, _) -> k = "model name")

      match configProperties.IsEmpty with
      | true -> ""
      | false -> configProperties |> List.head |> snd

  | false -> ""

let systemInfo () : Info =
  { distroId = getDistroId "/etc/os-release"

    distroName = getDistroName "/etc/os-release"

    kernelName = getKernel "/proc/version"

    shell = getShell "SHELL"

    user = getUser "USER"

    hostName = getHostName "/etc/hostname"

    memInfo = getMemoryInfo "/proc/meminfo"

    cpuModel = getCPUModelName "/proc/cpuinfo"

    localIp = getLocalIpAddress ()

    upTime = getUptime "/proc/uptime" }
