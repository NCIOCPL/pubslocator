Param(
    [Parameter(mandatory=$True, ValueFromPipeline=$False, Position=0)]
    [string]$sourcePath,

    [Parameter(mandatory=$True, ValueFromPipeline=$False, Position=0)]
    [string]$destinationPath
)

<#
    .SYNOPSIS
    Creates a ZIP file.

    .DESCRIPTION
    Compresses the content of sourcePath, creating a new zip at the location
    specified by destinationPath.

    .PARAMETER sourcePath
    Location of the files to be compressed.  This may be a directory, or an individual filespec (wildcards are supported).


    .PARAMETER destinationPath
    The name and path of the ZIP file to be created.
#>

Write-Host "Creating '${destinationPath}' from '${sourcePath}'."
$source = ${sourcePath}
$archive = ${destinationPath}
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($source, $archive)
<# https://stackoverflow.com/questions/40692024/zip-and-unzip-file-in-powershell-4 #>