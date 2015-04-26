;Win+Z

#z::
Loop 5
{
	Loop 2
	{
		Loop 17
		{	
			Sleep 100
			SendInput {Tab}
			Sleep 100
			SendInput ^c
			ClipWait
			SendInput {Alt Down}{Tab} ; Press down the tab key.
			Sleep 1000  ; Keep it down for two second.
			SendInput {Alt Up}
			Sleep 2000
			SendInput ^f
			Sleep 100
			SendInput </p>
			SendInput {Enter}
			SendInput {Enter}
			SendInput {Esc}
			SendInput {Left}
			SendInput ^+{Left}
			Sleep 100
			SendInput %clipboard%
			Sleep 200
			SendInput {Alt down}{Tab} ; Press down the tab key.
			Sleep 1000  ; Keep it down for two second.
			SendInput {Alt up}
			Sleep 200
		}
		Sleep 200
		SendInput {Tab}
		SendInput {Tab}
		SendInput {Alt down}{Tab} ; Press down the tab key.
		Sleep 1000  ; Keep it down for two second.
		SendInput {Alt up}
		Sleep 200
		SendInput ^f
		Sleep 100
		SendInput </p>		
		SendInput {Enter}
		Sleep 200
		SendInput {Enter}
		SendInput {Esc}
		SendInput {Left}
		Sleep 200
		SendInput {Alt down}{Tab} ; Press down the tab key.
		Sleep 1000  ; Keep it down for two second.
		SendInput {Alt up}
		Sleep 200
	}
	Sleep 200
	SendInput {Alt down}{Tab} ; Press down the tab key.
	Sleep 1000  ; Keep it down for two second.
	SendInput {Alt up}
	Sleep 200
	SendInput ^f	
	Sleep 100
	SendInput </p>		
	SendInput {Enter}
	SendInput {Enter}
	SendInput {Esc}
	SendInput {Up}
	Sleep 200
	SendInput {Alt down}{Tab} ; Press down the tab key.
	Sleep 1000  ; Keep it down for two second.
	SendInput {Alt up}
	Sleep 200
}
return

