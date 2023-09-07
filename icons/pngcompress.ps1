param(
    [Parameter(Mandatory=$true, Position=0)]
    $PngFile
)

pngout $PngFile
optipng -silent -o7 $PngFile
advpng -z4 $PngFile