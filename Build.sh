#!/bin/bash

# Fitch Multi-Platform Build Script
# Builds fitch for multiple platforms (Linux and Windows)

echo " "
echo "===================================================================== "
echo " "
echo "Building FITCH for multiple platforms ..."
echo " "
echo "===================================================================== "
echo " "

# Build for Linux x64
echo "Building for Linux x64..."
cd Cli
dotnet publish -c Release -r linux-x64
echo "✓ Linux x64 build complete: bin/Release/net8.0/linux-x64/publish/"

# Build for Windows x64
echo ""
echo "Building for Windows x64..."
dotnet publish -c Release -r win-x64
echo "✓ Windows x64 build complete: bin/Release/net8.0/win-x64/publish/"

# Build for Windows ARM64
echo ""
echo "Building for Windows ARM64..."
dotnet publish -c Release -r win-arm64
echo "✓ Windows ARM64 build complete: bin/Release/net8.0/win-arm64/publish/"

# Back to root
cd ..

echo ""
echo "===================================================================== "
echo "All builds complete!"
echo "===================================================================== "
echo ""
