@echo off
SETLOCAL

@set PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files\MSBuild\Microsoft\Windows Workflow Foundation\v3.5;C:\Program Files (x86)\Microsoft.NET\RedistList;%PATH%
@set LIBPATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;%LIBPATH%

ECHO Building PubsLocator
ECHO Todo: fix msbuild.log write permissions
msbuild /verbosity:detailed /target:ALL "%WORKSPACE%\tools\build\BuildPubsLocatorCode.xml"
ECHO Done building PubsLocator hashtaguniquecomment

REM Set environment variables
SET WORKSPACE=%WORKSPACE%

pause