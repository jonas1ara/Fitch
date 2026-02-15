module Lib.Colorize

open Spectre.Console

type ColorScheme = {
    LabelColor: Color
    ValueColor: Color
    HeaderColor: Color
}

let getColorScheme (distroId: string) : ColorScheme =
    match distroId.ToLower() with
    | "fedora" -> 
        { LabelColor = Color(0uy, 163uy, 240uy)  // Fedora Blue RGB
          ValueColor = Color.White
          HeaderColor = Color(0uy, 163uy, 240uy) }
    
    | "ubuntu" -> 
        { LabelColor = Color(233uy, 84uy, 32uy)  // Ubuntu Orange RGB
          ValueColor = Color.White
          HeaderColor = Color(233uy, 84uy, 32uy) }
    
    | "debian" -> 
        { LabelColor = Color(215uy, 10uy, 83uy)  // Debian Red RGB
          ValueColor = Color.White
          HeaderColor = Color(215uy, 10uy, 83uy) }
    
    | "arch" -> 
        { LabelColor = Color(23uy, 147uy, 209uy)  // Arch Blue RGB
          ValueColor = Color.White
          HeaderColor = Color(23uy, 147uy, 209uy) }
    
    | "linux mint" | "mint" -> 
        { LabelColor = Color(135uy, 195uy, 69uy)  // Mint Green RGB
          ValueColor = Color.White
          HeaderColor = Color(135uy, 195uy, 69uy) }
    
    | "nixos" -> 
        { LabelColor = Color(126uy, 186uy, 228uy)  // NixOS Blue RGB
          ValueColor = Color.White
          HeaderColor = Color(126uy, 186uy, 228uy) }
    
    | "alpine" -> 
        { LabelColor = Color(13uy, 90uy, 167uy)  // Alpine Blue RGB
          ValueColor = Color.White
          HeaderColor = Color(13uy, 90uy, 167uy) }
    
    | "windows 10" | "windows 11" | "windows" -> 
        { LabelColor = Color(255uy, 209uy, 93uy)  // Windows Yellow RGB
          ValueColor = Color.White
          HeaderColor = Color(255uy, 209uy, 93uy) }
    
    | _ -> // Default
        { LabelColor = Color(255uy, 105uy, 180uy)  // HotPink RGB
          ValueColor = Color.White
          HeaderColor = Color(255uy, 105uy, 180uy) }
