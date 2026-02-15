module Lib.Config

open Lib.Types
open System
open System.IO

let configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "fitch")
let configPath = Path.Combine(configDir, ".fitch")

let defaultConfig =
  { displayMode = Logo
    logoPosition = Left
    distroNameLabelColor = "Grey"
    distroNameHeaderColor = "Grey" }

let parseDisplayMode (value: string) =
  match value.ToLower().Trim() with
  | "logo" -> Logo
  | "distroname" -> DistroName
  | _ -> DistroName

let parseLogoPosition (value: string) =
  match value.ToLower().Trim() with
  | "left" -> Left
  | "right" -> Right
  | _ -> Right

let parseTomlConfig (content: string) =
  let lines = content.Split([| '\n'; '\r' |], StringSplitOptions.RemoveEmptyEntries)
  let parseLine (line: string) =
    let trimmed = line.Trim()
    if trimmed.StartsWith("#") || String.IsNullOrWhiteSpace(trimmed) then
      None
    else
      let parts = trimmed.Split([| '=' |], 2, StringSplitOptions.RemoveEmptyEntries)
      if parts.Length = 2 then
        let key = parts.[0].Trim().ToLower()
        let value = parts.[1].Trim().Trim('"')
        Some(key, value)
      else
        None

  let settings = lines |> Array.choose parseLine |> Map.ofArray

  let displayMode =
    settings |> Map.tryFind "displaymode"
    |> Option.map parseDisplayMode
    |> Option.defaultValue defaultConfig.displayMode

  let logoPosition =
    settings |> Map.tryFind "logoposition"
    |> Option.map parseLogoPosition
    |> Option.defaultValue defaultConfig.logoPosition

  let distroNameLabelColor =
    settings |> Map.tryFind "distronamelabelcolor"
    |> Option.defaultValue defaultConfig.distroNameLabelColor

  let distroNameHeaderColor =
    settings |> Map.tryFind "distronameheadercolor"
    |> Option.defaultValue defaultConfig.distroNameHeaderColor

  { displayMode = displayMode
    logoPosition = logoPosition
    distroNameLabelColor = distroNameLabelColor
    distroNameHeaderColor = distroNameHeaderColor }

let loadConfig () =
  try
    if File.Exists configPath then
      let content = File.ReadAllText configPath
      parseTomlConfig content
    else
      defaultConfig
  with
  | _ -> defaultConfig

let createDefaultConfigFile () =
  try
    if not (Directory.Exists configDir) then
      Directory.CreateDirectory configDir |> ignore

    if not (File.Exists configPath) then
      let defaultContent = """# Fitch Configuration

# Display mode: "logo" or "distroname"
displaymode = "logo"

# Logo position: "left" or "right"
logoposition = "left"

# ============================================
# Colors for DistroName mode (only apply when displaymode = "distroname")
# ============================================

# Color for labels (Distribution:, Kernel:, etc.) and user@hostname
distronamelabelcolor = "Blue"

# Color for the header (distro name in FigletText)
distronameheadercolor = "Grey"

# Available colors: Black, Red, Green, Yellow, Blue, White, Grey,
# DarkRed, DarkGreen, DarkBlue, DarkMagenta, DarkCyan,
# HotPink, Orange, Purple, Teal, Aqua, Fuchsia, Lime, Maroon, Navy, Olive, Silver
"""
      File.WriteAllText(configPath, defaultContent)
  with
  | _ -> ()
