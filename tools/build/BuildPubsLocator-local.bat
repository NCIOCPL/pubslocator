@echo off
SETLOCAL

@set PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files\MSBuild\Microsoft\Windows Workflow Foundation\v3.5;C:\Program Files (x86)\Microsoft.NET\RedistList;%PATH%
@set LIBPATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;%LIBPATH%



REM Set workspace var

REM Set environment variables
SET WORKSPACE=..\..
SET GH_ORGANIZATION_NAME=NCIOCPL
SET GH_REPO_NAME=pubslocator

REM Determine the current Git commit hash.
FOR /f %%a IN ('git rev-parse --verify HEAD') DO SET COMMIT_ID=%%a

REM Do the build
REM Todo: fix msbuild.log write permissions
ECHO Building PubsLocator...
msbuild /verbosity:detailed /target:ALL BuildPubsLocatorCode.xml
ECHO Completed building PubsLocator 

PAUSE