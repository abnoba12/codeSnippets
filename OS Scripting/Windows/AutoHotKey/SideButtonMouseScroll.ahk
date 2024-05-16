; Define the scroll interval in milliseconds
scrollInterval := 30

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
