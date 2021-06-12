import os
import xml.etree.ElementTree as ET

xlaunch_file = 'XDMCP-mode.xlaunch'

ifconfig = os.popen('ifconfig').read()
ipaddress = ifconfig.split("\n")[1].strip().split(" ")[1]

update = False
with open(xlaunch_file, 'r') as f: 
    xlaunch_xml = f.read()
    if ipaddress in xlaunch_xml:
        print(f'IP address {ipaddress} in xlaunch already up-to-date.')
    else:
        print(f'Updating IP address in xlaunch to {ipaddress}.')
        update = True

if update:        
    xlaunch = ET.parse(xlaunch_file).getroot()
    xlaunch.set('XDMCPHost', ipaddress)

    with open(xlaunch_file, 'wb') as f:
        f.write(ET.tostring(xlaunch))
