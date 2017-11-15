@echo ------------------------------------------------------------------------------------------------
@echo BEGIN
@echo ------------------------------------------------------------------------------------------------

@set /a startstamp=%time:~0,2%*3600000 + %time:~3,2%*60000 + %time:~6,2%*1000 + %time:~9,2%*10 

set CurrPath=%~dp0
set PrjPath=%1
set PrjName=%2
set IsUpdate=%3

for /l %%i in (1, 1, 8) do (
	COPY "%CurrPath%SLAVE_StartUnity.bat" \\ic-%%i\studentenprojektcave\StartUnity.bat
)


if "%IsUpdate%"=="update" goto copyFiles

for /l %%i in (1, 1, 8) do (
	RMDIR /S /Q \\ic-%%i\studentenprojektcave\aktuellesprojekt\
)


:copyFiles
@set /a startstamp=%time:~0,2%*3600000 + %time:~3,2%*60000 + %time:~6,2%*1000 + %time:~9,2%*10 

::for /l %%i in (1, 1, 8) do (
::    xcopy /E /Y /D %PrjPath% \\ic-%%i\studentenprojektcave\aktuellesprojekt\
::)

for /l %%i in (1, 1, 8) do (
	XCOPY /E /Y /D %PrjPath%\%PrjName%.exe \\ic-%%i\studentenprojektcave\aktuellesprojekt\
	XCOPY /E /Y /D %PrjPath%\%PrjName%_Data \\ic-%%i\studentenprojektcave\aktuellesprojekt\%PrjName%_Data\
)

::for /l %%i in (1, 1, 8) do (
::    robocopy "%PrjPath%" \\ic-%%i\studentenprojektcave\aktuellesprojekt\ /MIR
::)

@set /a stopstamp=%time:~0,2%*3600000 + %time:~3,2%*60000 + %time:~6,2%*1000 + %time:~9,2%*10
@set /a diff=(%stopstamp% - %startstamp%)
@set /a tSek=%diff%/1000
@echo 
@echo ------------------------------------------------------------------------------------------------
@echo END. Duration: %tSek%,%diff:~-3,-1% sek.
@echo ------------------------------------------------------------------------------------------------
@echo.