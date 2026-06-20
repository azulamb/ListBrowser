@echo off
cd /d "%~dp0ListBrowser"
dotnet build -c Release -v quiet
start "" "bin\Release\net10.0-windows\ListBrowser.exe"
