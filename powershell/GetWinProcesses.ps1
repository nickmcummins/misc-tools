param(
    [Parameter(Mandatory=$true, Position=0)]
    $PidsStr
)

$selectWin32ProcessQuery = "SELECT * FROM Win32_Process WHERE "
$pids = $PidsStr.Split(',')
for ($i = 0; $i -lt $pids.Length; $i++) {
	if ($i -gt 0) {
		$selectWin32ProcessQuery += " OR "
	}
	$selectWin32ProcessQuery += ("ProcessId = " + $pids[$i])
}
Get-CimInstance -Query $selectWin32ProcessQuery | ConvertTo-Json