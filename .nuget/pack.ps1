# custom nuspec pack
$path = '**\*.nuspec'
$nuget = '.\.nuget\nuget.exe'
$year = [System.DateTime]::Now.Year
$specFiles = Get-ChildItem $path -Recurse

foreach($file in $specFiles)
{
	$dllbase = $file.fullname.Replace($file.Name, "")
	$dll = $dllbase + "bin\release\" + $file.Name.Replace(".nuspec", ".dll")
	#todo add file check if this doesn't exist for a multiframework project
	if([System.IO.File]::Exists($dll) -ne $true){
			$dll = $dllbase + "bin\release\net45\" + $file.Name.Replace(".nuspec", ".dll")
	}
	$versionFullString = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($dll).ProductVersion
	$version = $versionFullString.Split(" ")[0] 
	Write-Host "Found " $version " from " $dll
	& $nuget pack $file -properties "version=$version;year=$year" -symbols
}