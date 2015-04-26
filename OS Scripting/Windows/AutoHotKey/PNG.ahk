;-------Variables---------
project = 80 342 ;Project location in tree
;pageTemplate = 141 457 ;Location of the page template in the new project
;carouselItemTemplate = 118 276 ;Location of the Carousel Item template in the new project

;----------Main-----------
;Page Template
Numpad1::
changeTemplate() ;Click on change template
openProject(project) ;Close project & open the png shared
Sleep, 500
Click 97 437
sleep, 250
Send, {END}
Click 133 321
NNF() ;Next, Next, Finish
return

;Carousel Item template
Numpad2::
changeTemplate() ;Click on change template
openProject(project) ;Close Aurora & open the project
Send, {Down}
Sleep, 200
Send, {Down}
Sleep, 200
Send, {Down}
Sleep, 500
Click %carouselItemTemplate% ;Click Carousel Item
NNF() ;Next, Next, Finish
return

;delete item
NumpadDot::
del()
Send, {enter}
Sleep, 2000
Send, {enter}
return

;publish Item
NumpadMult::
publish()
return

;open change template dialog
NumpadSub::
changeTemplate() ;Click on change template
openProject(project) ;Close Aurora & open the project
return

;Call NNF
NumpadAdd::
	NNF()
return

;---------Reusable functions---------------
;Configure -> Change Template
changeTemplate(){
	WinActivate, Sitecore - Windows Internet Explorer ahk_class IEFrame
	Send, !{c}
	Send, {enter}
	Sleep, 1000
	Click 468 180 ;Change button
	Sleep, 1500
}

del(){
	WinActivate, Sitecore - Windows Internet Explorer ahk_class IEFrame
	Send, !{h}
	Send, {enter}
	Sleep, 1000
	Click 482 185 ;delete button
	Sleep, 1500
}

publish(){
WinActivate, Sitecore - Windows Internet Explorer ahk_class IEFrame
	Send, !{p}
	Send, {enter}
	Sleep, 1000
	Send, ^{s}
	Sleep, 3000
	Click 180 205 ;publish dropdown
	Sleep, 1000
	WinActivate, Sitecore - Windows Internet Explorer ahk_class Alternate Modal Top Most
	WinActivate, Sitecore - Windows Internet Explorer ahk_class Alternate Modal Top Most
	Click 146 245 ;publish item
	Sleep, 1000
	Send, {enter}
	Sleep, 1000
	Send, {enter}
}

;open project
openProject(project){
	WinActivate, Sitecore - Windows Internet Explorer ahk_class Alternate Modal Top Most
	Click %project% ;Close project x
	Sleep, 700
	Click  83 360 ;Open png shared
	Sleep, 500
}          
           
;Next, Next, Finish
NNF(){     
	Sleep, 400
	Send, {enter} ;Click next
	Sleep, 600
	Send, {enter} ;Click next
}          