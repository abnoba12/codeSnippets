;Win+Z
#z::
SetKeyDelay, 60, 50

Loop 2
{
	TwoTitleDoubleRow()
}

return
;*************************************************************************************
;Will process move to the nest row of data and notepad++
;won't have a title in the next line
NextRowNoTitle()
{
	WinActivate, ahk_class MozillaUIWindowClass
	WinWaitActive, ahk_class MozillaUIWindowClass

	WinActivate, ahk_class Notepad++
	WinWaitActive, ahk_class Notepad++
	Send ^f
	WinWaitActive, ahk_class #32770		
	Send </p>				
	Send {Enter}		
	Send {Enter}		
	Send {Esc}		
	Send {Left}
	Sleep 200
	WinActivate, ahk_class OpusApp
	WinWaitActive, ahk_class OpusApp
}

