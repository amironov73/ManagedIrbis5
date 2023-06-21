@echo off

rem %1 - mask for files
rem %2 - description file

rem Example:
rem desc4all E:\Temp\TextInversion\*.png D:\Projects\Utils\Description4All\description.txt

if "%1%" == "" (echo Mask for files not specified!
    exit)
if "%2%" == "" (echo Description file not specified!
    exit)

if not exist %2% (echo Description file not exist!
    exit)

for %%i in (%1%) do (
   copy /y "%2"  "%%~dpni%~x2"
)
