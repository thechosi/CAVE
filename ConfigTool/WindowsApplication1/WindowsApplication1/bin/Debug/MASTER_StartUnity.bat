echo "MASTER: Start unity"

::tasklist | find /i "vrpn_server.exe">nul && taskkill /F /IM "vrpn_server.exe"

cd c:\IMSYS\control

perl perl\IC-X_StartUnity.pl ic-1
perl perl\IC-X_StartUnity.pl ic-2
perl perl\IC-X_StartUnity.pl ic-3
perl perl\IC-X_StartUnity.pl ic-4
perl perl\IC-X_StartUnity.pl ic-5
perl perl\IC-X_StartUnity.pl ic-6
perl perl\IC-X_StartUnity.pl ic-7
perl perl\IC-X_StartUnity.pl ic-8

timeout 1
:: Start Unity-Exe-File on Master-PC
if [%1] NEQ [] (
	if exist %1 start %1
)


