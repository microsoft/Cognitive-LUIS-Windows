@echo off
set dir=%1

rem Enter dir specified as 1st argument (e.g. .NET)
IF NOT [%dir%] == [] IF NOT EXIST %dir%\NUL GOTO NODIR

IF [%dir%] == [] IF NOT EXIST .\Runtime\NUL GOTO NOSPECDIR

echo .%dir%.

:DIROK
pushd .\%dir%

rem Go through Sub-packages
pushd Runtime
call CreatePackage.cmd
popd
pushd Programmatic
call CreatePackage.cmd
popd
rem Exit .NET dir
popd

GOTO END

:NOSPECDIR
echo "Please specify a directory where the package directories containing .cmd scripts should be found. Abort."

GOTO BADEND

:NODIR
echo "Dir %dir% not found. Abort."

rem exit 1
GOTO BADEND

:BADEND
exit /b 1

GOTO END

:END