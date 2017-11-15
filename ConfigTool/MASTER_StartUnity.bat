@echo off & setlocal

echo ##### Start unity clients
cd c:\IMSYS\control
for /l %%i in (1, 1, 8) do (
	perl perl\IC-X_StartUnity.pl ic-%%i
	echo. - on Client: IC-%%i
)

echo.
timeout 1


if /i "%2" == "/StartOnMaster:auto" (
	echo ##### Start Unity project on Master-PC
	if exist %1 (
		call %1
	) else (
		echo   ... #ERROR# bad path: %1!
	)
) else (
	echo ##### The user should start the unity project on Master-PC manual !!
)

endlocal