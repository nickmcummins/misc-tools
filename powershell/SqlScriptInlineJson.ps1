param(
    [Parameter(Mandatory=$true, Position=1)]
    $SqlFile,
    [Parameter(Mandatory=$true, Position=2)]
    $InlinedSqlFile
)

$sqlScript = [System.IO.File]::ReadAllText($PWD.ToString() + '\' + $SqlFile)
$selectBulkColumnJsonClobs = Select-String -AllMatches "\(SELECT BulkColumn FROM OPENROWSET\(BULK '(.*)\.json', SINGLE_CLOB\) as \[json\]\)." -InputObject $sqlScript
foreach ($match in $selectBulkColumnJsonClobs.Matches) {
    $jsonFilename =  $match.Groups[1].Value
    $json = [System.IO.File]::ReadAllText("$PWD\$jsonFilename.json") -replace '\s+', ' ' -replace ' ', ''
    #Write-Host "(SELECT BulkColumn FROM OPENROWSET(BULK '$jsonFilename.json', SINGLE_CLOB) as [json])"
    $sqlScript = $sqlScript.Replace("(SELECT BulkColumn FROM OPENROWSET(BULK '$jsonFilename.json', SINGLE_CLOB) as [json])", "'$json'")
}

[System.IO.File]::WriteAllText($InlinedSqlFile, $sqlScript)