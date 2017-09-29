@echo off & setlocal
echo ### MASTER ####################################################################

echo Start unity clients
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
echo Start Unity project on Master-PC
if exist %1 (
	call %1
) else (
	echo   ... #ERROR# bad path: %1!
)