@echo off
echo *** Building Microsoft.Azure.CognitiveServices.LUIS.Programmatic
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions

if not exist ..\nuget mkdir ..\nuget
if exist ..\nuget\Microsoft.Azure.CognitiveServices.LUIS.Programmatic*.nupkg erase /s ..\nuget\Microsoft.Azure.CognitiveServices.LUIS.Programmatic*.nupkg

..\tools\NuGet.exe pack Microsoft.Azure.CognitiveServices.LUIS.Programmatic.nuspec -symbols -properties version=2.0.0 -OutputDirectory ..\nuget

set error=%errorlevel%
set packageName=Microsoft.Azure.CognitiveServices.LUIS.Programmatic
if %error% NEQ 0 (
	echo *** Failed to build %packageName%
	exit /b %error%
) else (
	echo *** Succeeded to build %packageName%
)