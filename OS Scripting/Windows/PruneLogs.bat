NET USE V: \\USTX04VMWS016\Users\Public\CCA_Logs
forfiles /p "V:" /s /d -14 /c "cmd /c del @path /Q"