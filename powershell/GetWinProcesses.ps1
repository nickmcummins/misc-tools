param(
    [Parameter(Mandatory=$true, Position=0)]
    $PidsStr
)

$selectWin32ProcessQuery = "SELECT * FROM Win32_Process WHERE "
$pids = $PidsStr.Split(',')
for ($i = 0; $i -lt $pids.Length; $i++) {
    if ($i -gt 0)
    {
        $selectWin32ProcessQuery += " OR "
    }
	$selectWin32ProcessQuery += ("ProcessId = " + $pids[$i])
}
Get-CimInstance -Query $selectWin32ProcessQuery | Select-Object ProcessName,Handles,VM,WS,Path,Caption,Description,InstallDate,Name,Status,CreationClassName,CreationDate,ExecutionState,Handle,KernelModeTime,Priority,TerminationDate,UserModeTime,WorkingSetSize,CommandLine,ExecutablePath,HandleCount,MaximumWorkingSetSize,MinimumWorkingSetSize,OtherOperationCount,OtherTransferCount,PageFaults,PageFileUsage,ParentProcessId,PeakPageFileUsage,PeakVirtualSize,PeakWorkingSetSize,PrivatePageCount,ProcessId,QuotaNonPagedPoolUsage,QuotaPagedPoolUsage,QuotaPeakNonPagedPoolUsage,QuotaPeakPagedPoolUsage,ReadOperationCount,ReadTransferCount,SessionId,ThreadCount,VirtualSize,WindowsVersion,WriteOperationCount,WriteTransferCount |  ConvertTo-Json -Depth 10