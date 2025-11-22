param(
    [Parameter(Mandatory=$true, Position=0)]
    $GarminMapImg
)

mkdir tmp && cd tmp
if ($GarminMapImg.Contains(" ")) {	
	$GarminMapImgTmp = $GarminMapImg.Replace(' ', '-')
	cp ../$GarminMapImg $GarminMapImgTmp
	$GarminMapImg = $GarminMapImgTmp
}

unzip $GarminMapImg

$imgFiles = (ls *.img)
$imgFilename = $imgFiles[0].Filename
if ($imgFilename.Contains(' ')) {
	$imgFileRenamed = $imgFilename.Replace(' ', '-')
	mv $imgFilename $imgFileRenamed
	$imgFilename = $imgFilenameRenamed
}

$imgFileRenamed = $imgFilename.Replace(".zip")
mv $imgFilename 