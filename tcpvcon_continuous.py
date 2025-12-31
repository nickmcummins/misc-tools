import os
from datetime import datetime
import json
import yaml

EXCLUDE_PROCESSES = {'nxplayer.bin', 'X410.exe', 'WinStore.App.exe', 'nxrunner.bin', 'GitHubDesktop.exe', 'OUTLOOK.EXE',
                     'adb.exe', 'pycharm64.exe', 'mDNSResponder.exe', 'MouseWithoutBorders.exe', 'firefox.exe',
                     'cef_server.exe', 'LockApp.exe', 'msrdc.exe', 'full-line-inference.exe', 'MsMpEng.exe',
                     'MsTeamsVdi.exe'}

class TCPConnection:
    def __init__(self, timestamp, line):
        cols = line.split(',')
        self.timestamp = timestamp
        self.protocol = cols[0]
        self.process_name = cols[1]
        self.pid = cols[2]
        self.state = cols[3]
        self.local = cols[4]
        self.remote = cols[5]

    def __str__(self):
        return f'{self.timestamp},{self.protocol},{self.process_name},{self.pid},{self.state},{self.local},{self.remote}'


def include_line(tcp_connection):
    if EXCLUDE_PROCESSES.__contains__(tcp_connection.process_name):
        return False
    return True

pids = dict()

with open('tcpvcon_processes.yaml', 'a') as processesinfo:
    with open('tcpvcon.log', 'a') as log:
        while True:
            timestamp = datetime.now()
            tcp_connection_csvs = list(filter(lambda line: len(line.strip()) > 0, os.popen('tcpvcon64.exe -c -a -nobanner').read().split('\n')))
            tcp_connections = list(filter(lambda tcpconnection: include_line(tcpconnection), map(lambda line: TCPConnection(timestamp, line), tcp_connection_csvs)))
            print(f'existing pids: {pids.keys()}')
            new_pids = list(set(map(lambda tcpconnection: tcpconnection.pid, tcp_connections)).difference(pids.keys()))
            print('new pids:', new_pids)

            if len(new_pids) > 0:
                new_processes_json = os.popen(f"pwsh.exe -File C:/bin/GetProcesses.ps1 " + '"' + ','.join(new_pids) + '"').read()
                if (len(new_processes_json)) > 0:
                    new_processes = json.loads(new_processes_json)
                    if len(new_pids) == 1:
                        new_processes = [new_processes]

                    for new_process in new_processes:
                        pids[str(new_process['ProcessId'])] = new_process

                    new_processes_yaml = yaml.dump(new_processes, default_flow_style=False)
                    processesinfo.write(new_processes_yaml)

            log.write('\n'.join(map(lambda tcp: str(tcp), tcp_connections)) + '\n')