Param(
    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$tagName,

    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$releaseName,

    [Parameter(mandatory=$False, ValueFromPipeline=$False)]
    [string]$commitId = $null,

    [Parameter(mandatory=$False, ValueFromPipeline=$False)]
    [switch]$isPreRelease,

    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$releaseNotes,

    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$artifactDirectory,

    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$artifactFileName,

    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$githubOrg,

    [Parameter(mandatory=$True, ValueFromPipeline=$False)]
    [string]$githubRepo
)


function GitHub-Release($tagname, $releaseName, $commitId, $isPreRelease, $releaseNotes, $artifactDirectory, $artifactFileName, $githubOrg, $githubRepo, $githubApiKey)
{
    <#
        .SYNOPSIS
        Creates a tag and release on GitHub.

        .DESCRIPTION
        Creates a tag and release on GitHub and optionally uploads release artifacts.
        Based on https://gist.github.com/JanJoris/ee4c7f9b4289016b2216

        .PARAMETER tagName
        Name of the tag the release should be associated with.  Required.
        See commitId (below) for details of where tag is created.
        An error occurs if the tag already exists.

        .PARAMETER releaseName
        The name of the release

        .PARAMETER commitId
        The hash value the tag should be placed on.
            If commitID is blank, the tag is created on master.
            If commitID is a commit hash, the tag is created on the commit.
            If commitID is null, and the tag doesn't already exist, the tag is created on master, else the existing
                tag is used.

        .PARAMETER isPreRelease
        Boolean value, set to $True to mark the release as a pre-release, $False to
        mark it as a finalized release.

        .PARAMETER releaseNotes
        Description of the release.

        .PARAMETER artifactDirectory
        Path to where the artifact to be uploaded may be found.

        .PARAMETER artifactFileName
        Name of the file to be uploaded. This value also becomes the name of the artifact
        file to be downloaded.

        .PARAMETER githubOrg
        User or organization who owns the remote repository

        .PARAMETER githubRepo
        Name of the remote repository

        .PARAMETER githubApiKey
        GitHub personal access token with repo full control permission.
        https://github.com/settings/tokens

    #>
	
    $draft = $FALSE

    $releaseData = @{
       tag_name = $tagname
       name = $releaseName;
       body = $releaseNotes;
       draft = $draft;
       prerelease = $IsPreRelease;
    }

    # Don't want the target_commitish element unless it's set to something.
    if ($commitId -ne $null) {
        $releaseData.target_commitish = $commitId;
    }

    $auth = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($githubApiKey + ":x-oauth-basic"));

    $releaseParams = @{
       Uri = "https://api.github.com/repos/$githubOrg/$githubRepo/releases";
       Method = 'POST';
       Headers = @{
         Authorization = $auth;
       }
       ContentType = 'application/json';
       Body = (ConvertTo-Json $releaseData -Compress)
    }

    $result = Invoke-RestMethod @releaseParams
    $uploadUri = $result | Select -ExpandProperty upload_url
    Write-Host $uploadUri
    $uploadUri = $uploadUri -creplace '\{\?name,label\}'
    $uploadUri = $uploadUri + "?name=$artifactFileName"
    $uploadFile = Join-Path -path $artifactDirectory -childpath $artifactFileName

    $uploadParams = @{
      Uri = $uploadUri;
      Method = 'POST';
      Headers = @{
        Authorization = $auth;
      }
      ContentType = 'application/zip';
      InFile = $uploadFile
    }
    $result = Invoke-RestMethod @uploadParams
}

Try {
	GitHub-Release $tagname $releaseName $commitId ($IsPreRelease -eq $True)  $releaseNotes $artifactDirectory $artifactFileName $githubOrg $githubRepo $env:GITHUB_TOKEN
}
Catch {
	# Explicitly exit with an error.
	Write-Error "An error has occured $_"
	Exit 1
}

