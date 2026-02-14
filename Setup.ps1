# Fitch Setup Script for Windows
# This script builds and installs fitch as a global dotnet tool

Write-Host " "
Write-Host "===================================================================== "
Write-Host " "
Write-Host "This script configures FITCH on your system ..."
Write-Host " "
Write-Host "===================================================================== "
Write-Host " "

# Restore tools
dotnet tool restore

# Restore paket dependencies
dotnet paket restore

# Navigate to CLI project
cd Cli

# Publish in Release mode for Windows
dotnet publish -c Release

# Create the package
dotnet pack

# Uninstall old version if it exists
dotnet tool uninstall -g fitch

# Install the new version from local source
dotnet tool install -g fitch --add-source nupkg

# Back to root
cd ..

Write-Host " "
Write-Host "Installation complete!"
Write-Host "You can now run 'fitch' from anywhere in your terminal."
Write-Host " "
