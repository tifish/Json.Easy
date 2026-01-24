@echo off
setlocal enabledelayedexpansion
cd /d "%~dp0"

set projectName=Json.Easy

rd /s /q "%projectName%\bin\Release"

dotnet pack %projectName% -c Release || pause

set localNugetPath=%USERPROFILE%\LocalNuget
mkdir "%localNugetPath%"
dotnet nuget add source "%localNugetPath%" -n LocalTest
copy /y "%projectName%\bin\Release\*.nupkg" "%localNugetPath%" || pause

endlocal
