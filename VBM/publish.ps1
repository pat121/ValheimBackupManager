function RemoveDirIfExists($x)
{
    if ([System.IO.Directory]::Exists($x))
    {
        rm -Recurse "$x"
    }
}

function RemoveFileIfExists($x)
{
    if ([System.IO.File]::Exists($x))
    {
        rm "$x"
    }
}

$output="$Env:USERPROFILE\Desktop\Release"
$win="VBM-windows-x64"
$linux = "VBM-linux-x64"

RemoveDirIfExists "$output\$win"
RemoveDirIfExists "$output\$linux"
RemoveFileIfExists "$output\$win.zip"
RemoveFileIfExists "$output\$linux.zip"

Write-Output "Removed old builds"

dotnet publish -c release -o "$output\$win" -r win-x64 --self-contained false
dotnet publish -c release -o "$output\$linux" -r linux-x64 --self-contained false

if ([System.IO.Directory]::Exists("$output\$win"))
{
    Compress-Archive -Path "$output\$win" -DestinationPath "$output\$win.zip"
    if ([System.IO.File]::Exists("$output\$win.zip"))
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

if ([System.IO.Directory]::Exists("$output\$linux"))
{
    Compress-Archive -Path "$output\$linux" "$output\$linux.zip"
    if ([System.IO.File]::Exists("$output\$linux.zip"))
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