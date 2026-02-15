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
  
  // Agregar margen al logo
  let padder = Padder(image :> IRenderable)
  padder.Padding <- Padding(4, 2, 0, 2) // izquierda, arriba, derecha, abajo
  padder :> IRenderable

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
    let config = Lib.Config.loadConfig ()
    Lib.Config.createDefaultConfigFile()
    
    let info = systemInfo ()
    let colorScheme = getColorScheme info.distroId

    // Función auxiliar para crear líneas con etiqueta y valor
    let createInfoLine (label: string) (value: string) =
        let labelPart = Text(label, Style(colorScheme.LabelColor))
        let valuePart = Text(value, Style(colorScheme.ValueColor))
        Columns([labelPart :> IRenderable; valuePart :> IRenderable], Padding = Padding(0, 0, 1, 0), Expand = false) :> IRenderable

    let (rows: IRenderable seq) =
        seq {
            Text($"{info.user}@{info.hostName}", Style(colorScheme.HeaderColor))
            let separator = String.replicate (info.user.Length + info.hostName.Length + 1) "─"
            Text(separator, Style(Color.White))
            createInfoLine "Distribution:" info.distroName
            createInfoLine "Kernel:" info.kernelName
            createInfoLine "Shell:" info.shell
            createInfoLine "User:" info.user
            createInfoLine "Hostname:" info.hostName
            createInfoLine "Uptime:" info.upTime
            createInfoLine "Memory:" info.memInfo
            createInfoLine "CPU:" info.cpuModel
            createInfoLine "LocalIP:" info.localIp
        }

    let textPanel = Rows rows :> IRenderable
    
    let headerPanel : IRenderable =
        match config.displayMode with
        | DistroName ->
            renderDistroName info.distroId colorScheme.HeaderColor

        | Logo ->
            // Padding solo para alinear verticalmente
            let alignedTextPanel = 
                let padder = Padder(textPanel)
                padder.Padding <- Padding(4, 2, 0, 0) 
                padder :> IRenderable
            
            match config.logoPosition with
            | Left  -> 
                let cols = Columns [ loadLogo info.distroId; alignedTextPanel ]
                cols.Padding <- Padding(0, 0, 0, 0)
                cols.Expand <- false // Collapse para ajustar al contenido
                cols :> IRenderable
            | Right -> 
                let cols = Columns [ alignedTextPanel; loadLogo info.distroId ]
                cols.Padding <- Padding(0, 0, 0, 0)
                cols.Expand <- false // Collapse para ajustar al contenido
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
