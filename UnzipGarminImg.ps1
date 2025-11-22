param(
    [Parameter(Mandatory=$true, Position=0)]
    $GarminMapImgZip
)

$garminMapName = $GarminMapImgZip.Replace('.zip', '')

mkdir tmp && cd tmp

unzip ../$GarminMapImgZip -d . 
Write-Host "Extracted $GarminMapImgZip to $(pwd)."

$imgFile = (ls *.img)
$imgFilename = $imgFile
$imgName = $imgFilename.Replace('.img', '')
$imgFileRenamed = $imgFilename
if ($garminMapName.Contains($imgName)) {
	$imgFilenameSuffix = $garminMapName.Replace($imgName, '')
	$imgFileRenamed = $imgFileRenamed.Replace('.img', [System.String]::Concat($imgFilenameSuffix, '.img'))
	Write-Host $imgFileRenamed
}

if ($imgFileRenamed.Contains(' ')) {
	$imgFileRenamed = $imgFileRenamed.Replace(' ', '_')
	Write-Host $imgFileRenamed
	mv $imgFilename $imgFileRenamed
}

mv $imgFileRenamed .. 
cd ..
rm -rf tmp
