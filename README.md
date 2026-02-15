# Fitch

A fast, cross-platform system information tool written in F#.

![fitch](https://github.com/user-attachments/assets/fe6841e4-60ba-4833-ba0d-9c5705017b73)


[![Generic badge](https://img.shields.io/badge/Made%20with-FSharp-rgb(1,143,204).svg)](https://shields.io/)
![Tests][tests]

## About This Fork

This is a maintained and actively developed fork of the original [Fitch](https://github.com/lqdev/fitch) project created by **Luis Quintanilla** ([@lquintanilla](https://github.com/lquintanilla)) and **Luis Angel Mendez Gort** ([@lamg](https://github.com/lamg)). 

The original project brought system information display to Linux using F#. This fork continues that vision with major enhancements and ongoing maintenance.

**Original authors**: Luis Quintanilla, Luis Angel Mendez Gort  
**Current maintainer**: Jonas Lara ([@jonas1ara](https://github.com/jonas1ara))

<details>
<summary><b>📜 Project Evolution History</b></summary>

<br>

### Original Project (2022-2024)
- Created by Luis Quintanilla
- Linux-only support with Spectre.Console for rendering
- Basic system information display
- Inspiration from Nitch and Neofetch
- Initial NuGet package release

### Refactor code and add ASCII art logos (2023)
- Refactored code for better maintainability
- Added ASCII art logos for supported distributions
- Improved configuration system with display modes and color options

### Current Version (2026)
- Port to Windows with WMI-based implementation
- PNG logo support replacing ASCII art
- GPU detection (Windows, Linux, WSL)
- Battery information display
- Terminal emulator detection
- Distribution-specific RGB color schemes
- Enhanced configuration system
- Implemented DistroName mode with customizable colors
- Implemented Setup.ps1 for Windows installation
- Improved Setup.sh with dependency checking
- Fixed case-sensitivity issues in distribution detection
- Fixed spacing in columns

### DistroName Mode - Honoring the Origin

The **DistroName** display mode was part of the original vision - displaying system information with the distribution name in beautiful FigletText. This mode has been enhanced with:
- Fully configurable colors via `.fitch` config file
- Distribution-specific color schemes in Logo mode
- Respect for the minimalist, elegant approach of the original design

</details>

---

## Features

**System Information:**
- Distribution/OS name and version
- Kernel version
- Terminal emulator
- Shell
- User and hostname
- Uptime
- Memory usage
- CPU model
- GPU (optional - if detected)
- Battery status (optional - on laptops)
- Local IP address

**Display Modes:**
- **Logo Mode**: Distribution PNG logos with system info
- **DistroName Mode**: Distribution name in FigletText with customizable colors

**Supported Platforms:**
- Linux (Debian, Ubuntu, Fedora, Arch, NixOS, Alpine, Mint)
- Windows 10/11
- WSL (Windows Subsystem for Linux)

---

## Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Option 1: Install from NuGet (Recommended)

```bash
dotnet tool install --global fitch
```

<details>
<summary><b>📦 Option 2: Build from Source</b></summary>

<br>

#### Linux

1. Clone the repository:
   ```bash
   git clone https://github.com/jonas1ara/fitch.git
   cd fitch
   ```

2. Run the setup script:
   ```bash
   chmod +x Setup.sh
   ./Setup.sh
   ```

   The script will:
   - Check for optional dependencies (`lspci` for GPU detection)
   - Build and install fitch as a global .NET tool
   - Make it available system-wide

#### Windows

1. Clone the repository:
   ```powershell
   git clone https://github.com/jonas1ara/fitch.git
   cd fitch
   ```

2. Run the PowerShell setup script:
   ```powershell
   .\Setup.ps1
   ```

</details>

---

## Usage

Simply run:
```bash
fitch
```

**Optional**: Add `fitch` to your shell config (`.bashrc`, `.zshrc`, etc.) to display on terminal startup:
```bash
# In ~/.bashrc or ~/.zshrc
fitch
```

---

## Configuration

Fitch creates a configuration file on first run with sensible defaults.

**Configuration file location:**
- **Linux**: `~/.config/fitch/.fitch`
- **Windows**: `%USERPROFILE%\.config\fitch\.fitch`

### Quick Configuration

```toml
# Display mode: "logo" or "distroname"
displaymode = "logo"

# Logo position: "left" or "right"
logoposition = "left"

# Colors for DistroName mode (only apply when displaymode = "distroname")
distronamelabelcolor = "Blue"
distronameheadercolor = "DarkBlue"
```

<details>
<summary><b>📖 Complete Configuration Guide</b></summary>

<br>

### Configuration Options

```toml
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
distronameheadercolor = "DarkBlue"

# Available colors: Black, Red, Green, Yellow, Blue, White, Grey,
# DarkRed, DarkGreen, DarkBlue, DarkMagenta, DarkCyan,
# HotPink, Orange, Purple, Teal, Aqua, Fuchsia, Lime, Maroon, Navy, Olive, Silver
```

### Display Modes

#### Logo Mode (Default)
- Shows distribution PNG logo
- Uses distribution-specific color schemes (Fedora blue, Ubuntu orange, etc.)
- Logo can be positioned left or right

#### DistroName Mode
- Shows distribution name in ASCII art (FigletText)
- Fully customizable label and header colors
- Honors the original minimalist design philosophy
- Perfect for terminals where PNG logos don't render well

### Example Configurations

**Fedora style with logo on the left:**
```toml
displaymode = "logo"
logoposition = "left"
```

**DistroName mode with custom colors:**
```toml
displaymode = "distroname"
logoposition = "right"
distronamelabelcolor = "Cyan"
distronameheadercolor = "Blue"
```

**Ubuntu orange theme:**
```toml
displaymode = "distroname"
logoposition = "left"
distronamelabelcolor = "Orange"
distronameheadercolor = "DarkRed"
```

</details>

---

## Optional Dependencies

<details>
<summary><b>🔧 GPU Detection (Linux)</b></summary>

<br>

For GPU detection on Linux, install `pciutils`:

**Debian/Ubuntu:**
```bash
sudo apt install pciutils
```

**Fedora/RHEL:**
```bash
sudo dnf install pciutils
```

**Arch:**
```bash
sudo pacman -S pciutils
```

**Alpine:**
```bash
sudo apk add pciutils
```

Without `pciutils`, fitch will still work but won't display GPU information.

</details>

---

## Building for Multiple Platforms

<details>
<summary><b>🛠️ Build Instructions</b></summary>

<br>

### Build All Platforms

**Linux:**
```bash
./Build.sh
```

**Windows:**
```powershell
.\Build.ps1
```

Generates executables for:
- Windows x64 and ARM64
- Linux x64

### Build for Specific Platform

```bash
# Linux
dotnet publish -c Release -r linux-x64

# Windows x64
dotnet publish -c Release -r win-x64

# Windows ARM64
dotnet publish -c Release -r win-arm64
```

</details>

---

## Architecture

<details>
<summary><b>🏗️ Technical Details</b></summary>

<br>

Fitch uses a modular architecture with platform-specific implementations:

- **`SystemInfoLinux.fs`** - Gathers info from `/proc`, `/etc`, and `lspci`
- **`SystemInfoWindows.fs`** - Uses Windows Management Instrumentation (WMI)
- **`SystemInfo.fs`** - Runtime OS detection and routing
- **`DisplayInfo.fs`** - Rendering with Spectre.Console
- **`Colorize.fs`** - Distribution-specific color schemes
- **`Config.fs`** - Configuration file parsing

This design allows:
- Single codebase for multiple platforms
- Easy addition of new distributions
- Clean separation of concerns
- Efficient runtime detection

### Dependencies

- [Spectre.Console](https://spectreconsole.net/) - Beautiful terminal UI
- [Spectre.Console.ImageSharp](https://spectreconsole.net/) - PNG image rendering
- [ByteSize](https://github.com/omar/ByteSize) - Memory size formatting

</details>

---

## Supported Distributions

<details>
<summary><b>🐧 View All Supported Systems</b></summary>

<br>

### Linux
- Arch Linux (Manjaro, EndeavourOS, etc.)
- Debian (Ubuntu, Linux Mint, Pop!_OS, etc.)
- Fedora (RHEL, CentOS, etc.)
- NixOS ([Additional guidance](https://www.luisquintanilla.me/wiki/nixos-dotnet-packages-source))
- Alpine Linux

### Windows
- Windows 10 and later
- Windows 11
- WSL (All supported Linux distributions)

</details>

---

## License

MIT License - See [LICENSE](LICENSE) file for details.

Original work Copyright (c) 2022-2024 Luis Quintanilla, Luis Angel Mendez Gort  
Modified work Copyright (c) 2026 Jonas Lara

---

## Acknowledgements

<details>
<summary><b>🙏 Credits & Inspiration</b></summary>

<br>

**Original Authors:**
- Luis Quintanilla ([@lquintanilla](https://github.com/lquintanilla))
- Luis Angel Mendez Gort ([@lamg](https://github.com/lamg))

**Inspiration:**
- [Nitch](https://github.com/unxsh/nitch) - Minimal system information tool
- [Neofetch](https://github.com/dylanaraps/neofetch) - The legendary system info script
- [Fastfetch](https://github.com/fastfetch-cli/fastfetch) - Fast neofetch-like tool

**Special Thanks:**
- F# Community for the amazing ecosystem
- Spectre.Console team for the beautiful terminal framework
- All contributors and users who keep this project alive

</details>

---

## Contributing

Contributions are welcome! Feel free to:
- Report bugs
- Suggest features
- Submit pull requests
- Improve documentation

Please ensure your code follows F# conventions and includes appropriate tests.

---

## Changelog

<details>
<summary><b>📜 Version History</b></summary>

<br>

### v2.0.0 (2026) - Major Update
- Port to Windows with WMI-based implementation
- PNG logo support replacing ASCII art
- GPU detection (Windows, Linux, WSL)
- Battery information display
- Terminal emulator detection
- Distribution-specific RGB color schemes
- Enhanced configuration system
- Implmemented DistroName mode with customizable colors
- Implemented Setup.ps1 for Windows installation
- Improved Setup.sh with dependency checking
- Fixed case-sensitivity issues in distribution detection
- Fixed spacing in columns

### v1.2 (2023) - Refactor code and add ASCI art logos
- Refactored code for better maintainability
- Added ASCII art logos for supported distributions
- Improved configuration system with display modes and color options

### v1.0 (2022-2023) - Original
- Initial Linux support
- Linux-only support with Spectre.Console for rendering
- Basic system information
- Inspired by Nitch and Neofetch
- Nuget package release

</details>

---

**Made with ❤️ and F#**

[tests]: https://github.com/jonas1ara/fitch/workflows/tests/badge.svg
