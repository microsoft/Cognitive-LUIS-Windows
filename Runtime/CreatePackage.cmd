@echo off
echo *** Building Microsoft.Azure.CognitiveServices.LUIS
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions

if not exist ..\nuget mkdir ..\nuget
if exist ..\nuget\Microsoft.Azure.CognitiveServices.LUIS*.nupkg erase /s ..\nuget\Microsoft.Azure.CognitiveServices.LUIS*.nupkg

..\tools\NuGet.exe pack Microsoft.Azure.CognitiveServices.LUIS.nuspec -symbols -properties version=2.0.0 -OutputDirectory ..\nuget

set error=%errorlevel%
set packageName=Microsoft.Azure.CognitiveServices.LUIS
if %error% NEQ 0 (
	echo *** Failed to build %packageName%
	exit /b %error%
) else (
	echo *** Succeeded to build %packageName%
)