module Lib.Types

type Info =
  { distroId: string
    distroName: string
    kernelName: string
    shell: string
    user: string
    hostName: string
    memInfo: string
    cpuModel: string
    gpu: string option
    localIp: string
    upTime: string
    battery: string option
    terminal: string }

type DistroLogo =
  { distro: string
    logo: string
    colors: string list }

type DisplayMode =
  | Logo
  | DistroName

type LogoPosition =
  | Left
  | Right

type Config =
  { displayMode: DisplayMode
    logoPosition: LogoPosition
    distroNameLabelColor: string  // Color para etiquetas en modo DistroName
    distroNameHeaderColor: string } // Color para header en modo DistroName
