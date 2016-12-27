# Sets AppVeyor variables for custom assembly info patching
$path = 'src\**\AssemblyInfo.*'
$build = $env:APPVEYOR_BUILD_NUMBER
$commit = $env:APPVEYOR_REPO_COMMIT
$infos = Get-ChildItem $path -Recurse

foreach($file in $infos)
{
    Write-Host "Patching assembly info file: " $file.fullname
    Write-Host "For {build} = $build and {commit} = $commit"
    $x = Get-Content $file.fullname
    $x.Replace("{build}",$build).Replace("{commit}", $commit) | Set-Content $file.fullName
}

Write-Host $infos.Count