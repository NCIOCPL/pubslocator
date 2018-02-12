# File path and image location path are passed in from batch file. 
Param(
  [Parameter(Mandatory=$True)]
  [string]$filePath,
  [Parameter(Mandatory=$True)]  
  [string]$imagePath
)

function Main() {
    # The WebAdministration module requires elevated privileges.
    $isAdmin = Is-Admin
    if( $isAdmin ) {
    
        Write-Host -foregroundcolor 'green' "Starting..."
        Import-Module WebAdministration
        Write-Host -foregroundcolor 'green' "File path: $filePath"
        Write-Host -foregroundcolor 'green' "Image path: $imagePath"
        
        Write-Host -foregroundcolor 'magenta' "Removing old sites and AppPools..."
        ClearOldSites
        Write-Host -foregroundcolor 'magenta' "Done."
        
        Write-Host -foregroundcolor 'green' "Adding AppPools..."
        AddAppPools
        Write-Host -foregroundcolor 'green' "Done."
        
        Write-Host -foregroundcolor 'green' "Adding sites..."
        AddSites
        Write-Host -foregroundcolor 'green' "Done."
    } 
    else {
        Write-Host -foregroundcolor 'red' "This script must be run from an AA account."
    }   
}

# Add and configure Application Pools
function AddAppPools {
    # Create GUAM AppPool with settings
    New-WebAppPool -Name "GUAM"
    $GuamAppPool = Get-Item IIS:\AppPools\GUAM
    $GuamAppPool.managedRuntimeVersion = "v4.0"
    $GuamAppPool.enable32BitAppOnWin64 = "True"
    $GuamAppPool | Set-Item
    
    # Create PubsLocator AppPool with settings
    New-WebAppPool -Name "PubsLocator"
    $PubsAppPool = Get-Item IIS:\AppPools\PubsLocator
    $PubsAppPool.managedRuntimeVersion = "v2.0"
    $PubsAppPool.enable32BitAppOnWin64 = "True"
    $PubsAppPool | Set-Item

    # Create ApplicationManagement AppPool with settings
    New-WebAppPool -Name "ApplicationManagement"
    $AppMgtAppPool = Get-Item IIS:\AppPools\ApplicationManagement
    $AppMgtAppPool.managedRuntimeVersion = "v4.0"
    $AppMgtAppPool.enable32BitAppOnWin64 = "True"
    $AppMgtAppPool | Set-Item
    
    # Create PubsLocator Admin AppPool with settings    
    New-WebAppPool -Name "PubsAdmin"
    $PubsAdminAppPool = Get-Item IIS:\AppPools\PubsAdmin
    $PubsAdminAppPool.managedRuntimeVersion = "v2.0"
    $PubsAdminAppPool.enable32BitAppOnWin64 = "True"
    $PubsAdminAppPool | Set-Item
}

# Add and configure Application Pools
function AddSites {

    $RootFilePath = "$filePath"
    $RootImagePath = "$imagePath\cissecure\ncipubs\prodimages"
    $PDFPath = "$imagePath\cissecure\PDF"    
    $WinAuthentication = '/system.WebServer/security/authentication/windowsAuthentication'
    $AnonAuthentication = '/system.WebServer/security/authentication/anonymousAuthentication'
    $BasicAuthentication = '/system.WebServer/security/authentication/basicAuthentication'    
    
    # Create NCIPL site and HTTPS binding    
    Write-Host -foregroundcolor 'blue' "NCIPL / External setup"
    New-Website -Name "PubsLocator" -Port 443 -IPAddress "*" -HostHeader "xxxx.xxx.fakegov" -PhysicalPath "$RootFilePath\root" -ApplicationPool "PubsLocator" -Ssl
    Set-WebConfigurationProperty -Filter $AnonAuthentication -Name "enabled" -Value true -Location "PubsLocator"
    Set-WebConfigurationProperty -Filter $WinAuthentication -Name "enabled" -Value false -Location "PubsLocator"
        New-WebApplication -Name "NCIPL" -Site "PubsLocator" -PhysicalPath "$RootFilePath\NCIPL" -ApplicationPool "PubsLocator"
            New-WebVirtualDirectory -Name "pubimages" -Site "PubsLocator" -Application "NCIPL" -PhysicalPath $RootImagePath 
            New-WebApplication -Name "GUAM" -Site "PubsLocator\NCIPL" -PhysicalPath "$RootFilePath\GUAM" -ApplicationPool "GUAM"
        New-WebApplication -Name "NCIPLEX" -Site "PubsLocator" -PhysicalPath "$RootFilePath\NCIPLEX" -ApplicationPool "PubsLocator"    
            New-WebVirtualDirectory -Name "pubimages" -Site "PubsLocator" -Application "NCIPLEX" -PhysicalPath $RootImagePath 
        New-WebApplication -Name "VPR" -Site "PubsLocator" -PhysicalPath "$RootFilePath\KIOSK" -ApplicationPool "PubsLocator"
            New-WebVirtualDirectory -Name "pubimages" -Site "PubsLocator" -Application "VPR" -PhysicalPath $RootImagePath 
        New-WebVirtualDirectory -Name "pdf" -Site "PubsLocator" -PhysicalPath $PDFPath
    Start-Website -Name "PubsLocator"

    # Create ApplicationManagement site and HTTPS binding
    Write-Host -foregroundcolor 'blue' "ApplicationManagement setup"
    New-Website -Name "ApplicationManagement" -Port 443 -IPAddress "*" -HostHeader "xxxx.xxx.fakegov" -PhysicalPath "$RootFilePath\ApplicationManagement" -ApplicationPool "ApplicationManagement" -Ssl     
    Set-WebConfigurationProperty -Filter $AnonAuthentication -Name "enabled" -Value true -Location "ApplicationManagement"
    Set-WebConfigurationProperty -Filter $WinAuthentication -Name "enabled" -Value false -Location "ApplicationManagement"
    Start-Website -Name "ApplicationManagement"

    # Create NCIPLAdmin site and HTTPS binding
    Write-Host -foregroundcolor 'blue' "NCIPLAdmin / Internal setup"
    New-Website -Name "PubsAdmin" -Port 443 -IPAddress "*" -HostHeader "xxxx.xxx.fakegov" -PhysicalPath "$RootFilePath\root" -ApplicationPool "PubsAdmin" -Ssl     
    Set-WebConfigurationProperty -Filter $AnonAuthentication -Name "enabled" -Value true -Location "PubsAdmin"
    Set-WebConfigurationProperty -Filter $WinAuthentication -Name "enabled" -Value false -Location "PubsAdmin"
        New-WebApplication -Name "NCIPLCC" -Site "PubsAdmin" -PhysicalPath "$RootFilePath\NCIPLCC" -ApplicationPool "PubsAdmin"
            New-WebVirtualDirectory -Name "pubimages" -Site "PubsAdmin" -Application "NCIPLCC" -PhysicalPath $RootImagePath 
        New-WebApplication -Name "NCIPLLM" -Site "PubsAdmin" -PhysicalPath "$RootFilePath\NCIPLLM" -ApplicationPool "PubsAdmin"
            New-WebVirtualDirectory -Name "pubimages" -Site "PubsAdmin" -Application "NCIPLLM" -PhysicalPath $RootImagePath 
        New-WebApplication -Name "NCIPLAdmin" -Site "PubsAdmin" -PhysicalPath "$RootFilePath\NCIPLAdmin" -ApplicationPool "PubsAdmin" 
        New-WebApplication -Name "PERT" -Site "PubsAdmin" -PhysicalPath "$RootFilePath\PERT" -ApplicationPool "PubsAdmin"
        New-WebVirtualDirectory -Name "pdf" -PhysicalPath $PDFPath -Site "PubsAdmin"
    Start-Website -Name "PubsAdmin"
}

# Remove old sites and AppPools to prevent errors on site creation
function ClearOldSites {
    if (Test-Path "IIS:\AppPools\PubsLocator") { Remove-WebAppPool -Name "PubsLocator" }
    if (Test-Path "IIS:\AppPools\GUAM") { Remove-WebAppPool -Name "GUAM" }
    if (Test-Path "IIS:\AppPools\ApplicationManagement") { Remove-WebAppPool -Name "ApplicationManagement" }
    if (Test-Path "IIS:\AppPools\PubsAdmin") { Remove-WebAppPool -Name "PubsAdmin" }
    if (Test-Path "IIS:\Sites\PubsLocator") { Remove-Website -Name "PubsLocator" }
    if (Test-Path "IIS:\Sites\ApplicationManagement") { Remove-Website -Name "ApplicationManagement" }
    if (Test-Path "IIS:\Sites\PubsAdmin") { Remove-Website -Name "PubsAdmin" }
}

# Verify that the script is being run with admin privileges
function Is-Admin {
    $id = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
    $id.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}

# Run the Main() function
Main
