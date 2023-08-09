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
  Send {Left 5}
  Send {Right 5}
  Sleep, 120000  ; 2 minutes
}
Return

;Press esc to stop script execution
Escape::
ExitApp
Return