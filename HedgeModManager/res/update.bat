﻿# No idea why this line is not executing. Random byte at the start?; It's the UTF-8 BOM retard
REM 
@echo off
REM Clears the Screen
cls
REM Sets the title
title Updating...
REM 
echo Waiting 3 seconds to ensure process is closed...
REM Sleeps for 3 seconds
powershell start-sleep 3
REM 
echo Copying update files over the old version of the application...
REM Copies all new files from the temporary folder
xcopy /s /y ".\updateTemp" ".\"
REM 
echo done!
REM Backs up the log file
ren HedgeModManager.log HedgeModManager_prev.log
REM Cleans the temp folder
rmdir updateTemp
REM Sleeps for 1 second
powershell start-sleep 1
REM 
echo Starting application...
REM Starts the Mod Manager without waiting
start HedgeModManager.exe
REM deletes teh current script
del update.bat