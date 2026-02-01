; Remap horizontal scroll to vertical scroll
WheelRight::
	Loop 5 ; Adjust the number (higher = faster scroll)
    {
        Send {WheelUp}
        Sleep 10 ; Adjust the delay between scrolls (lower = faster)
    }
    Return
WheelLeft::	
	Loop 5 ; Adjust the number (higher = faster scroll)
    {
        Send {WheelDown}
        Sleep 10 ; Adjust the delay between scrolls (lower = faster)
    }
    Return

; Define the scroll interval in milliseconds
scrollInterval := 40

; Scroll down while holding XButton1
XButton1::
    SetTimer, ScrollDown, %scrollInterval%
    Return

XButton1 Up::
    SetTimer, ScrollDown, Off
    Return

ScrollDown:
    Send {WheelDown}
    Return

; Scroll up while holding XButton2
XButton2::
    SetTimer, ScrollUp, %scrollInterval%
    Return

XButton2 Up::
    SetTimer, ScrollUp, Off
    Return

ScrollUp:
    Send {WheelUp}
    Return
