;Press the windows Button + O to fire this
;Move the mouse randomly once a min for 8 hours 
;Press esc to stop
#o::
SysGet, MonitorPrimary, MonitorPrimary
SysGet, Monitor, Monitor, %MonitorPrimary%
loop 480
{
  x = 1
  y = 1
  Random, x, 0, %MonitorRight%
  Random, y, 0, %MonitorBottom%
  MouseMove x, y
  Sleep, 60000  ; 60 seconds
}
Return

;Press esc to stop script execution
Escape::
ExitApp
Return