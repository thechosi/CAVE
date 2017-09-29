@echo off
@set path_tcp=%~dp0TCP_Send.exe

if [%1] == [] goto :EOF
if /i [%1]==[red] goto :RED
if /i [%1]==[green] goto :GREEN

goto :EOF


:RED
set glasses=RED Glasses - IMSYS
set config=config20170710121332857
goto :SET_CONFIG


:GREEN
set glasses=GREEN Glasses - ART
set config=config2017071012135557


:SET_CONFIG
echo Execute option set config to '%glasses%'
echo ... 1) Stop tracking
%path_tcp% 192.168.0.50 50105 -in:txt -out:hex -log:on "dtrack2 tracking stop"
echo ... 2) Set Config
%path_tcp% 192.168.0.50 50105 -in:txt -out:hex -log:on "dtrack2 set config active_config %config%"
echo ... 3) Start tracking
%path_tcp% 192.168.0.50 50105 -in:txt -out:hex -log:on "dtrack2 tracking start"
::pause