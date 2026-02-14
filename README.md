# Fitch

Command line system information display utility for Windows and Linux systems built with .NET (F#).

![Fitch CLI Tool](./images/fitch-display.png)

[![Generic badge](https://img.shields.io/badge/Made%20with-FSharp-rgb(1,143,204).svg)](https://shields.io/)
![Tests][tests]

**NOTE: This application works on Windows and Linux systems. It's been tested on the following distributions:**

### Linux
- Arch
    - Manjaro
- Debian
    - Ubuntu
- NixOS. For additional guidance, see this [article](https://www.luisquintanilla.me/wiki/nixos-dotnet-packages-source).

### Windows
- Windows 10 and later

## Dependencies

- [Spectre.Console](https://spectreconsole.net/)

## Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Install from [nuget](https://www.nuget.org/packages/fitch)

#### Instructions

Installation is as easy as:

```bash
dotnet tool install --global fitch
```

### Build from source

#### Instructions - Linux

1. Clone [fitch repo](http://www.luisquintanilla.me/github/fitch) on your machine

    ```bash
    git clone https://github.com/lqdev/fitch.git && cd fitch
    ```

1. Run the setup script

    ```bash
    ./Setup.sh
    ```

    ![Setup.sh](./images/setup.gif)

    Running this script will generate an executable called `fitch` in the *bin/Release/net8.0/linux-x64/publish* directory and copy it to the */usr/bin/* directory, so you can run the application from anywhere in your system.

#### Instructions - Windows

1. Clone [fitch repo](http://www.luisquintanilla.me/github/fitch) on your machine

    ```powershell
    git clone https://github.com/lqdev/fitch.git; cd fitch
    ```

1. Run the PowerShell setup script

    ```powershell
    .\Setup.ps1
    ```

    Running this script will build, package, and install fitch as a global dotnet tool, so you can run the application from anywhere in your terminal.

## Run application

1. For both cases just type `fitch` in your terminal

    ```bash
    fitch
    ```

1. (Optional) Add the `fitch` command to your shell config file to start when your shell starts

## Configuration

Fitch can be customized through a configuration file. The configuration file is automatically created on first run with default values.

**Configuration file location:**
- **Linux**: `~/.config/fitch/.fitch`
- **Windows**: `%APPDATA%\fitch\.fitch`

### Configuration Options

The `.fitch` file uses TOML format with the following options:

```toml
# Modo de visualización: "logo" o "distroname"
displaymode = "distroname"

# Posición del logo: "left" o "right"
logoposition = "right"

# Color del texto: nombre de color Spectre (HotPink, Yellow, Blue, Green, etc.)
textcolor = "HotPink"
```

#### displaymode
- `"logo"` - Displays the distribution's ASCII logo
- `"distroname"` - Displays the distribution name with Spectre Console styling (default)

#### logoposition
- `"left"` - Places the logo/name on the left side
- `"right"` - Places the logo/name on the right side (default)

#### textcolor
- Any valid Spectre Console color name (e.g., `HotPink`, `Yellow`, `Blue`, `Green`, `Cyan`, `Magenta`, etc.)
- This color applies to the distribution name, kernel, and shell labels

### Example Configurations

**Show logo with custom colors:**
```toml
displaymode = "logo"
logoposition = "left"
textcolor = "Cyan"
```

**Show distro name on the right in blue:**
```toml
displaymode = "distroname"
logoposition = "right"
textcolor = "Blue"
```

## Building for Multiple Platforms

Fitch now supports building for both Windows and Linux in a single codebase. The application uses runtime OS detection to automatically select the correct system information gathering implementation.

### Build All Platforms

**On Linux:**
```bash
./Build.sh
```

**On Windows:**
```powershell
.\Build.ps1
```

This will generate executables for:
- **Windows x64** and **Windows ARM64**
- **Linux x64**

### Build for Specific Platform

```bash
# Linux
dotnet publish -c Release -r linux-x64

# Windows x64
dotnet publish -c Release -r win-x64

# Windows ARM64
dotnet publish -c Release -r win-arm64
```

## Architecture

The application uses platform-specific modules that are selected at runtime:

- **`SystemInfoLinux.fs`** - Gathers system info from `/proc` and `/etc` files
- **`SystemInfoWindows.fs`** - Gathers system info using Windows Management Instrumentation (WMI)
- **`SystemInfo.fs`** - Router module that detects the OS and calls the appropriate implementation

This approach allows maintaining a single codebase while supporting different operating systems efficiently.

## To-Dos

- [x] Enable customization through config file
- [x] Cross-platform support (Windows and Linux)

## Acknowledgements

This project was inspired by [Nitch](https://github.com/unxsh/nitch), [Neofetch](https://github.com/dylanaraps/neofetch) and made with [WSL](https://learn.microsoft.com/en-us/windows/wsl/) 🐧


[tests]: https://github.com/lamg/fitch/workflows/tests/badge.svg