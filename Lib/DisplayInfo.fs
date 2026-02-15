module DisplayInfo

open System.Reflection
open Spectre.Console
open Spectre.Console.Rendering
open Lib.SystemInfo
open Lib.Types
open Lib.Colorize

let loadLogo (logo: string) =
  let assembly = Assembly.GetExecutingAssembly()
  
  let logoLower = logo.ToLower()
  let resourceName = $"Lib.Logos.{logoLower}.png"
  
  use stream = assembly.GetManifestResourceStream(resourceName)
  
  let image = 
    if stream <> null then
      CanvasImage(stream)
    else
      let fallbackStream = assembly.GetManifestResourceStream("Lib.Logos.linux.png")
      CanvasImage(fallbackStream)
  
  image.MaxWidth <- 16
  
  // Add margin to logo
  let padder = Padder(image :> IRenderable)
  padder.Padding <- Padding(4, 2, 0, 2) // left, top, right, bottom
  padder :> IRenderable

let getColorFromString (colorName: string) =
  match colorName.ToLower() with
  | "black" -> Color.Black
  | "red" -> Color.Red
  | "green" -> Color.Green
  | "yellow" -> Color.Yellow
  | "blue" -> Color.Blue
  | "magenta" -> Color.Magenta1
  | "cyan" -> Color.Cyan1
  | "white" -> Color.White
  | "grey" | "gray" -> Color.Grey
  | "darkred" -> Color.DarkRed
  | "darkgreen" -> Color.DarkGreen
  | "darkyellow" -> Color.DarkGoldenrod
  | "darkblue" -> Color.DarkBlue
  | "darkmagenta" -> Color.DarkMagenta
  | "darkcyan" -> Color.DarkCyan
  | "hotpink" -> Color.HotPink
  | "orange1" | "orange" -> Color.Orange1
  | "purple" -> Color.Purple
  | "teal" -> Color.Teal
  | "aqua" -> Color.Aqua
  | "fuchsia" -> Color.Fuchsia
  | "lime" -> Color.Lime
  | "maroon" -> Color.Maroon
  | "navy" -> Color.Navy
  | "olive" -> Color.Olive
  | "silver" -> Color.Silver
  | _ -> Color.HotPink

let renderDistroName (distroId: string) (color: Color) =
  let figText = FigletText(distroId)
  figText.Color <- color
  figText :> IRenderable

let displayInfo () =
    let config = Lib.Config.loadConfig ()
    Lib.Config.createDefaultConfigFile()
    
    let info = systemInfo ()
    
    // Determine which color scheme to use
    let colorScheme = 
        match config.displayMode with
        | DistroName ->
            // Use configurable colors for DistroName mode
            { LabelColor = getColorFromString config.distroNameLabelColor
              ValueColor = Color.White
              HeaderColor = getColorFromString config.distroNameHeaderColor }
        | Logo ->
            // Use distribution-specific colors for Logo mode
            getColorScheme info.distroId

    // Helper function to create lines with label and value
    let createInfoLine (label: string) (value: string) =
        let labelPart = Text(label, Style(colorScheme.LabelColor))
        let valuePart = Text(value, Style(colorScheme.ValueColor))
        Columns([labelPart :> IRenderable; valuePart :> IRenderable], Padding = Padding(0, 0, 1, 0), Expand = false) :> IRenderable

    let (rows: IRenderable seq) =
        seq {
            Text($"{info.user}@{info.hostName}", Style(colorScheme.LabelColor))
            let separator = String.replicate (info.user.Length + info.hostName.Length + 1) "─"
            Text(separator, Style(Color.White))
            createInfoLine "Distribution:" info.distroName
            createInfoLine "Kernel:" info.kernelName
            createInfoLine "Terminal:" info.terminal
            createInfoLine "Shell:" info.shell
            createInfoLine "User:" info.user
            createInfoLine "Hostname:" info.hostName
            createInfoLine "Uptime:" info.upTime
            createInfoLine "Memory:" info.memInfo
            createInfoLine "CPU:" info.cpuModel
            
            // Show GPU only if it exists
            match info.gpu with
            | Some gpu -> createInfoLine "GPU:" gpu
            | None -> ()
            
            // Show battery only if it exists
            match info.battery with
            | Some battery -> createInfoLine "Battery:" battery
            | None -> ()
            
            createInfoLine "LocalIP:" info.localIp
        }

    let textPanel = Rows rows :> IRenderable
    
    let headerPanel : IRenderable =
        match config.displayMode with
        | DistroName ->
            renderDistroName info.distroId colorScheme.HeaderColor

        | Logo ->
            // Padding only for vertical alignment
            let alignedTextPanel = 
                let padder = Padder(textPanel)
                padder.Padding <- Padding(4, 2, 0, 0) 
                padder :> IRenderable
            
            match config.logoPosition with
            | Left  -> 
                let cols = Columns [ loadLogo info.distroId; alignedTextPanel ]
                cols.Padding <- Padding(0, 0, 0, 0)
                cols.Expand <- false // Collapse to fit content
                cols :> IRenderable
            | Right -> 
                let cols = Columns [ alignedTextPanel; loadLogo info.distroId ]
                cols.Padding <- Padding(0, 0, 0, 0)
                cols.Expand <- false // Collapse to fit content
                cols :> IRenderable

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
