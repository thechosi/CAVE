set CurrPath=%~dp0

copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-1\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-2\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-3\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-4\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-5\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-6\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-7\studentenprojektcave\StartUnity.bat
copy "%CurrPath%SLAVE_StartUnity.bat" \\ic-8\studentenprojektcave\StartUnity.bat

RMDIR /S /Q \\ic-1\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-2\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-3\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-4\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-5\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-6\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-7\studentenprojektcave\aktuellesprojekt\
RMDIR /S /Q \\ic-8\studentenprojektcave\aktuellesprojekt\

xcopy /E "%1" \\ic-1\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-2\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-3\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-4\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-5\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-6\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-7\studentenprojektcave\aktuellesprojekt\
xcopy /E "%1" \\ic-8\studentenprojektcave\aktuellesprojekt\
::xcopy /E "c:\users\icuser\Desktop\Studentenprojekt CAVE\CAVE-master\TestExe" \\ic-8\studentenprojektcave\testexe\