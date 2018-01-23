@echo off

rem Prompt user to enter the file and image paths on the local machine
echo Enter the full filepath of the PubsLocator folder, no trailing slash
set /P FilePath=(e.g. 'C:\MyDevFolder\PubsLocator'): 
echo Enter the full filepath of the PubsRuntimeFiles folder, no trailing slash 
set /P ImagePath=(e.g. 'C:\MyDevFolder\PubsRuntimeFiles'):

rem Run the powershell script 
powershell -ExecutionPolicy RemoteSigned ./IISSetupDev.ps1 -filePath %FilePath% -imagePath %ImagePath%

rem Do an IIS reset
iisreset

pause