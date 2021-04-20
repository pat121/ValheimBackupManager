$output="$Env:USERPROFILE\Desktop\Release"

rm "$output\windows-x64" "$output\linux-x64"
Write-Output "Removed old builds"

dotnet publish -c release -o "$output\windows-x64" -r win-x64 --self-contained false
dotnet publish -c release -o "$output\linux-x64" -r linux-x64 --self-contained false

if ([System.IO.Directory]::Exists("$output\windows-x64"))
{
    Compress-Archive -Path "$output\windows-x64" -DestinationPath "$output\windows-x64.zip"
    if ([System.IO.File]::Exists("$output\windows-x64"))
    {
        Write-Output "Windows build zipped successfully"
    }
    else
    {
        Write-Output "Failed to zip the windows build"
    }
}
else
{
    Write-Output "Failed to publish windows build"
}

if ([System.IO.Directory]::Exists("$output\linux-x64"))
{
    Compress-Archive -Path "$output\linux-x64" "$output\linux-x64.zip"
    if ([System.IO.File]::Exists("$output\linux-x64.zip"))
    {
        Write-Output "Linux build zipped successfully"
    }
    else
    {
        Write-Output "Failed to zip the linux build"
    }
}
else
{
    Write-Output "Failed to publish linux build"
}