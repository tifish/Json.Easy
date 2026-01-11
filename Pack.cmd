@echo off
setlocal enabledelayedexpansion
cd /d "%~dp0"

rd /s /q "Json.Easy\bin\Release"

dotnet pack Json.Easy -c Release || pause

set localNugetPath=%USERPROFILE%\LocalNuget
mkdir "%localNugetPath%"
dotnet nuget add source "%localNugetPath%" -n LocalTest
copy /y "Json.Easy\bin\Release\*.nupkg" "%localNugetPath%" || pause

endlocal
