#!/bin/bash

echo " "
echo "===================================================================== "
echo " "
echo "This script configures FITCH on your system ..."
echo " "
echo "===================================================================== "
echo " "

# Verificar dependencias opcionales para GPU detection
echo "Checking optional dependencies..."
echo " "

if ! command -v lspci &> /dev/null; then
    echo "⚠️  'lspci' not found - GPU detection will not work"
    echo " "
    echo "To enable GPU detection, install pciutils:"
    
    # Detectar el gestor de paquetes
    if command -v apt &> /dev/null; then
        echo "  sudo apt install pciutils"
    elif command -v dnf &> /dev/null; then
        echo "  sudo dnf install pciutils"
    elif command -v pacman &> /dev/null; then
        echo "  sudo pacman -S pciutils"
    elif command -v zypper &> /dev/null; then
        echo "  sudo zypper install pciutils"
    elif command -v apk &> /dev/null; then
        echo "  sudo apk add pciutils"
    else
        echo "  Install 'pciutils' using your package manager"
    fi
    
    echo " "
    read -p "Continue installation anyway? (y/n) " -n 1 -r
    echo " "
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        echo "Installation cancelled."
        exit 1
    fi
else
    echo "✓ lspci found - GPU detection enabled"
fi

echo " "
echo "Installing FITCH..."
echo " "

dotnet tool restore
dotnet paket restore
cd Cli
dotnet publish -c Release
dotnet pack
dotnet tool uninstall -g fitch
dotnet tool install -g fitch --add-source nupkg

# Back to root
cd ..

echo " "
echo "===================================================================== "
echo "Installation complete!"
echo " "
echo "You can now run 'fitch' from anywhere in your terminal."
echo " "
if ! command -v lspci &> /dev/null; then
    echo "Note: GPU detection is disabled. Install pciutils to enable it."
    echo " "
fi
echo "===================================================================== "
echo " "
