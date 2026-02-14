module Lib.SystemInfoWindows

open System
open System.Diagnostics
open System.Net

open Types

let getWindowsVersion () =
  try
    let osVersion = Environment.OSVersion
    let version = osVersion.VersionString
    version
  with
  | _ -> "Windows"

let getKernel () =
  try
    Environment.OSVersion.VersionString
  with
  | _ -> ""

let getDistroName () = "Windows"

let getDistroId () = "windows"

let getHostName () =
  try
    Dns.GetHostName()
  with
  | _ -> ""

let getMemoryInfo () =
  try
    let wmiQuery = "SELECT TotalVisibleMemorySize, AvailablePhysicalMemory FROM Win32_OperatingSystem"
    let searcher = new System.Management.ManagementObjectSearcher(wmiQuery)
    let collection = searcher.Get()
    
    let mutable totalMemory = 0UL
    let mutable availableMemory = 0UL
    
    for item in collection do
      totalMemory <- UInt64.Parse(item["TotalVisibleMemorySize"].ToString()) / 1024UL
      availableMemory <- UInt64.Parse(item["AvailablePhysicalMemory"].ToString()) / 1024UL
    
    let totalGB = (decimal totalMemory / 1024m).ToString("0.0")
    let availGB = (decimal availableMemory / 1024m).ToString("0.0")
    $"{availGB} GB / {totalGB} GB"
  with
  | _ -> ""

let getCPUModel () =
  try
    let wmiQuery = "SELECT Name FROM Win32_Processor"
    let searcher = new System.Management.ManagementObjectSearcher(wmiQuery)
    let collection = searcher.Get()
    
    let mutable cpuModel = ""
    for item in collection do
      cpuModel <- item["Name"].ToString()
    
    cpuModel
  with
  | _ -> ""

let getShell () =
  try
    let env = Environment.GetEnvironmentVariable("PROMPT")
    if isNull env then
      "CMD"
    else
      "PowerShell"
  with
  | _ -> "Unknown"

let getUser () =
  try
    Environment.UserName
  with
  | _ -> ""

let getUptime () =
  try
    let wmiQuery = "SELECT LastBootUpTime FROM Win32_OperatingSystem"
    let searcher = new System.Management.ManagementObjectSearcher(wmiQuery)
    let collection = searcher.Get()
    
    let mutable bootTime = DateTime.MinValue
    for item in collection do
      let bootTimeStr = item["LastBootUpTime"].ToString()
      if bootTimeStr.Length >= 14 then
        let year = int bootTimeStr.[0..3]
        let month = int bootTimeStr.[4..5]
        let day = int bootTimeStr.[6..7]
        let hour = int bootTimeStr.[8..9]
        let minute = int bootTimeStr.[10..11]
        let second = int bootTimeStr.[12..13]
        bootTime <- DateTime(year, month, day, hour, minute, second)
    
    if bootTime <> DateTime.MinValue then
      let uptime = DateTime.Now - bootTime
      let hours = int uptime.TotalHours
      let minutes = uptime.Minutes
      $"{hours}h{minutes}m"
    else
      ""
  with
  | _ -> ""

let getLocalIpAddress () =
  try
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
  with
  | _ -> ""

let systemInfo () : Info =
  { distroId = getDistroId ()
    distroName = getDistroName ()
    kernelName = getKernel ()
    shell = getShell ()
    user = getUser ()
    hostName = getHostName ()
    memInfo = getMemoryInfo ()
    cpuModel = getCPUModel ()
    localIp = getLocalIpAddress ()
    upTime = getUptime () }
