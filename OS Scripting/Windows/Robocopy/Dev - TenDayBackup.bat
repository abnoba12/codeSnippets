@ECHO ON
REM V.7
REM Ten day rolling Backup version
REM -------------------------------------------------------------------------------
REM Set what environment this batch file is for based on this file name. File names first three letters must start with: "Dev", "Tes", "Sta", or "Pro".
set batName=%~n0
Set first3=%batName:~0,3%
set env=%first3%

REM pull the configuration parameters from the location.ini file
set INIFILE="%~dp0location.ini"
set PUB_LOG="%~dp0Publish.log"
call:getvalue %INIFILE% "siteName" "" siteName
call:getvalue %INIFILE% "%env%Environment" "" environment
call:getvalue %INIFILE% "%env%Source" "" source
call:getvalue %INIFILE% "%env%Destination" "" destination
REM -------------------------------------------------------------------------------

REM Output to a file: > will overwrite and >> will append
REM 2>&1 writes to the file as well as the console
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO  %siteName% %environment% Environment Install %Date% %Time% >> %PUB_LOG% 2>&1
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO. >> %PUB_LOG% 2>&1

REM Delete backups older then thirty days before making the backup
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO  Deleting backups older then 10 days from %~dp0%environment%_backup >> %PUB_LOG% 2>&1
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO. >> %PUB_LOG% 2>&1
FORFILES /D -10 /P %~dp0%environment%_backup /C "cmd /c IF @isdir == TRUE rd /S /Q @PATH" >> %PUB_LOG% 2>&1

REM backup the files at the destination
SET formattedDateTime=%date:~10,4%%date:~4,2%%date:~7,2%_%Time:~0,2%%Time:~3,2%%Time:~6,2%
SET backup=%~dp0%environment%_backup\%formattedDateTime%
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO  Backing up %destination% to %backup% >> %PUB_LOG% 2>&1
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
robocopy "%destination%" "%backup%" /E /XO /XF *.jpg /XF *.png /XF *.jpeg /XF *.gif /XF *.pdf /MIR /NFL /NDL /TEE /LOG+:%PUB_LOG%
ECHO. >> %PUB_LOG% 2>&1

REM Copy over new files from the source to destination
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO  Deploying files from %source% to %destination% >> %PUB_LOG% 2>&1
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
robocopy "%source%" "%destination%" /MIR /XX /XF *.config /TEE /LOG+:%PUB_LOG%
ECHO. >> %PUB_LOG% 2>&1

ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
ECHO  Deployment to %siteName% %environment% completed at %Date% %Time% >> %PUB_LOG% 2>&1
ECHO ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ >> %PUB_LOG% 2>&1
REM add newlines to the end of the entry
ECHO. >> %PUB_LOG% 2>&1
ECHO. >> %PUB_LOG% 2>&1
ECHO. >> %PUB_LOG% 2>&1
ECHO. >> %PUB_LOG% 2>&1
ECHO. >> %PUB_LOG% 2>&1
goto:eof

REM getvalue(INIFILE, SearchTerm, "", ResultVar);
REM This function reads a value from an INI file and stored it in a variable
REM %1 = name of ini file to search in.
REM %2 = search term to look for
REM %3 = group name (not currently used)
REM %4 = variable to place search result
:getvalue
FOR /F "eol=; eol=[ tokens=1,2* delims==" %%i in ('findstr /b /l /i %~2= %1') DO set %~4=%%~j
goto:eof