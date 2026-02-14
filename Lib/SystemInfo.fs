module Lib.SystemInfo

open System.Runtime.InteropServices

open Types

let systemInfo () : Info =
  if RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
    SystemInfoLinux.systemInfo ()
  elif RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
    SystemInfoWindows.systemInfo ()
  else
    SystemInfoLinux.systemInfo ()
