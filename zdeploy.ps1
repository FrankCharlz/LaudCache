

# Function to read XML properties
function Get-XmlProperty {
    param (
        [string]$filePath,
        [string]$propertyName
    )

    if (Test-Path $filePath) {
        $xml = [xml](Get-Content $filePath)
        $prop = $xml.SelectSingleNode("//PropertyGroup/$propertyName")
        if ($prop) {
            return $prop.InnerText
        }
    }
    return $null
}

$buildProps = "Directory.Build.props"

# Read version and packageId
$version = Get-XmlProperty $buildProps "Version"
$packageId = Get-XmlProperty $buildProps "PackageId"


if (!$version -or !$packageId) {
    Write-Error "Could not find Version or PackageId in project files"
    exit 1
}

Write-Host "Building package $packageId version $version"

# Build and pack
dotnet build --configuration Release
dotnet pack --configuration Release

# Push to NuGet feed
$package = "$packageId.$version.nupkg"
# dotnet nuget push "bin/Release/$package" -s http://192.168.200.70:30011/v3/index.json
dotnet nuget push "bin/Release/$package" -s http://mecmis.nhif.or.tz:30011/v3/index.json