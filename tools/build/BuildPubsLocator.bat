@echo off
SETLOCAL

@set PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files\MSBuild\Microsoft\Windows Workflow Foundation\v3.5;C:\Program Files (x86)\Microsoft.NET\RedistList;%PATH%
@set LIBPATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;%LIBPATH%

REM Check environment variables
set FAIL=
IF "%WORKSPACE%"=="" set FAIL=True
IF "%TEMP%"=="" set FAIL=True
IF "%BUILD_NUMBER%"=="" set FAIL=True
IF "%FAIL%" NEQ "" (
	ECHO Fail. Fix your stuff.
	GOTO :EOF
)

REM Set environment variables
SET WORKSPACE=%WORKSPACE%
SET GH_ORGANIZATION_NAME=daquinohd
SET GH_REPO_NAME=pubslocator

REM Determine the current Git commit hash.
FOR /f %%a IN ('git rev-parse --verify HEAD') DO SET COMMIT_ID=%%a

REM Do the build
REM Todo: fix msbuild.log write permissions
ECHO Building PubsLocator...
msbuild /verbosity:detailed /target:ALL "%WORKSPACE%\tools\build\BuildPubsLocatorCode.xml"
ECHO Completed building PubsLocator 

PAUSE