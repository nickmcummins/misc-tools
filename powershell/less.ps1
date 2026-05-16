$follow = $false
$currentArg = 0

if ($args[$currentArg] -eq '+F') {
    $follow = $true
    $currentArg += 1
}
$height = $Host.UI.RawUI.WindowSize.Height

$filename = $args[$currentArg]
$lineCount = (Get-Content $filename | Measure-Object -Line).Lines
$skipLines = 0
if ($lineCount -gt $height) {
    $skipLines = $lineCount - $height
    Get-Content $filename | Select-Object -Skip $skipLines
    $skipLines += $height
}

if ($skipLines -gt 0) {
    Get-Content $filename -Wait | Select-Object -Skip $skipLines
} else {
    Get-Content $filename -Wait
}
