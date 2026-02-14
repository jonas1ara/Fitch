module DisplayInfo

open Lib
open Spectre.Console
open Spectre.Console.Rendering
open Lib.SystemInfo
open Lib.Types

let loadLogo (logo: string) =
  NeofetchLogos.logoDictionary.TryGetValue logo
  |> function
    | true, l -> l
    | false, _ -> NeofetchLogos.logoDictionary["unknown"]
  |> Colorize.colorize
  |> Text
  :> IRenderable

let getColorFromString (colorName: string) =
  try
    System.Enum.Parse(typeof<Color>, colorName, true) :?> Color
  with
  | _ -> Color.HotPink

let renderDistroName (distroId: string) (color: Color) =
  let figText = FigletText(distroId)
  figText.Color <- color
  figText :> IRenderable

let displayInfo () =
    let config = Config.loadConfig ()
    Config.createDefaultConfigFile()
    
    let info = systemInfo ()
    let textColor = getColorFromString config.textColor

    let (rows: IRenderable seq) =
        seq {
            Text($"Distribution: {info.distroName}", Style(Color.HotPink))
            Text($"Kernel: {info.kernelName}", Style(textColor))
            Text($"Shell: {info.shell}", Style(textColor))
            Text($"User: {info.user}", Style(Color.Yellow))
            Text($"Hostname: {info.hostName}", Style(Color.Yellow))
            Text($"Uptime: {info.upTime}", Style(Color.Blue))
            Text($"Memory: {info.memInfo}", Style(Color.Blue))
            Text($"CPU: {info.cpuModel}", Style(Color.Blue))
            Text($"LocalIP: {info.localIp}", Style(Color.Green))
        }

    let textPanel = Rows rows :> IRenderable

    let headerPanel : IRenderable =
        match config.displayMode with
        | DistroName ->
            renderDistroName info.distroId textColor

        | Logo ->
            match config.logoPosition with
            | Left  -> Columns [ loadLogo info.distroId; textPanel ] :> IRenderable
            | Right -> Columns [ textPanel; loadLogo info.distroId ] :> IRenderable

    let finalLayout =
        match config.displayMode with
        | DistroName ->
            Rows [
                headerPanel
                textPanel
            ] :> IRenderable

        | Logo ->
            headerPanel

    AnsiConsole.Write(finalLayout)
