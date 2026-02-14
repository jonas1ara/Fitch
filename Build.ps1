# Fitch Multi-Platform Build Script for Windows
# Builds fitch for multiple platforms (Linux and Windows)

Write-Host " "
Write-Host "===================================================================== "
Write-Host " "
Write-Host "Building FITCH for multiple platforms ..."
Write-Host " "
Write-Host "===================================================================== "
Write-Host " "

Push-Location Cli

# Build for Windows x64
Write-Host "Building for Windows x64..."
dotnet publish -c Release -r win-x64
Write-Host "✓ Windows x64 build complete: bin/Release/net8.0/win-x64/publish/"

# Build for Windows ARM64
Write-Host ""
Write-Host "Building for Windows ARM64..."
dotnet publish -c Release -r win-arm64
Write-Host "✓ Windows ARM64 build complete: bin/Release/net8.0/win-arm64/publish/"

# Build for Linux x64
Write-Host ""
Write-Host "Building for Linux x64..."
dotnet publish -c Release -r linux-x64
Write-Host "✓ Linux x64 build complete: bin/Release/net8.0/linux-x64/publish/"

Pop-Location

Write-Host ""
Write-Host "===================================================================== "
Write-Host "All builds complete!"
Write-Host "===================================================================== "
Write-Host ""
