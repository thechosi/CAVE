@echo off

set name=%1
::TODO: sourcepath as second parameter
set sourcepath=c:\studentenprojektcave\
set targetpath=where\do\you\want\it\
set conffile=file.txt
IF "%name%" == "" (
	echo usage: give name of project as 1st and path as 2nd parameter.
	echo example: deploy.cmd Test c:\studentenprojektcave\
	pause
) ELSE (
	echo script for deploying files to CAVE-clients.
	echo called by deployment UI or manually.
	echo copies folder "%name%" from master to render-clients, adjusts IMSYS' win.conf and restarts their JVM
	set path=%sourcepath%%name%
	echo checking win.conf for entry "[START%name%]".
	FOR /F "eol=; tokens=* delims=, " %%i in (%conffile%) do (
		IF "%%i" == "[START%name%]" goto END
		IF "%%i" NEQ "[START%name%]" (
			set check=false
		)
	)
	

	
	IF "%check%" == "false" (
		::This might have to move to target pc... try accessing files on target pc.
		echo no such entry. generating.
		echo [START%name%]>>%conffile%
		echo 	TYPE = SCRIPT>>%conffile%
		::TODO: Target path
		echo 	CMD = %targetpath%projectname.exe>>%conffile%
		echo restart JVM.
		::TODO restart
		start Notepad.exe 'C:\studentenprojektcave\afile.txt'
		echo Notepad started.
		::for /F "TOKENS=1,2,*" %%a in ('c:\windows\system32\cmd.exe /c c:\windows\system32\TASKLIST.exe /FI "IMAGENAME eq Notepad.exe"') do set MyPID=%%b
		::echo MyPID: %MyPID%
		::pause
		c:\windows\system32\cmd.exe /c c:\windows\system32\TASKKILL.exe /F /IM "Notepad.exe"
		echo killed Notepad.
		echo restart completed.
	)
	echo copying files to target...
	::xcopy %path% \\ic-1\%targetpath%
	echo done with ic-1.
	::xcopy %path% \\ic-2\%targetpath% ...
	echo done with ic-2.
	echo ...done copying.
	echo use "startunity.bat %name%" to start your project on all clients.
	goto FINALEND
	
	:END
	echo already exists. nothing to do. exit.
	pause 
)
:FINALEND
