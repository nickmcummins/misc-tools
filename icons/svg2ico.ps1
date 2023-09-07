param(
    [Parameter(Mandatory=$true,Position=0)]
    [Alias("s")]
    $SvgFilename,
    [Parameter(Mandatory=$false,Position=1)]
    $IcoFilename = ""
)
if (-not [System.IO.Directory]::Exists("tmp")) {
    New-Item -Type Directory -Path tmp
}
$sizes = (16, 24, 32, 48, 64, 128, 256)
$filename = [System.IO.Path]::GetFileNameWithoutExtension($SvgFilename)

for ($i = 0; $i -lt $sizes.length; $i++) {
    $size = $sizes[$i];
    Write-Host $sizes[$i]
    Write-Host $filename
    inkscape -z -e tmp/${filename}_${size}.png -w $size -h $size -d 300 $SvgFilename
    pngcompress tmp/${filename}_${size}.png
}

if ($IcoFilename.length -eq 0) {
    $IcoFilename = $SvgFilename.Replace(".svg", ".ico")
}
if (-not $IcoFilename.EndsWith(".ico")) {
    $IcoFilename = "${IcoFilename}/${filename}.ico"
}
Write-Host "Writing $IcoFilename"
convert tmp/${filename}_16.png tmp/${filename}_24.png tmp/${filename}_32.png tmp/${filename}_48.png tmp/${filename}_64.png tmp/${filename}_128.png tmp/${filename}_256.png ${IcoFilename}
Remove-Item -Path tmp -Recurse
Write-Host "Successfully wrote $IcoFilename"