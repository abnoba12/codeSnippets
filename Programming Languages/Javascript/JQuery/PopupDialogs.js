//If you are using this in MVC then use bootstrap modal and not this.

//Pass in HTML and this will display it in a modal window
function Pop_Html(html) {
	 var w = screen.availWidth;
	 var h = screen.availHeight;
	 var popW = 560;
	 var popH = 500;
	 var wf = "";
	 var leftPos = (w - popW) / 2;
	 var topPos = (h - popH) / 2;
	 wf += "width=" + popW;
	 wf += ",height=" + popH;
	 wf += ",resizable=no";
	 wf += ",scrollbars=yes";
	 wf += ",menubar=no";
	 wf += ",toolbar=no";
	 wf += ",directories=no";
	 wf += ",location=no";
	 wf += ",status=no";
	 wf += ",left=" + leftPos;
	 wf += ",top=" + topPos;
	 myWindow = window.open("", "Pop_Win", wf);
	 myWindow.document.close();
	 myWindow.document.write(html);
	 myWindow.focus()
 }

 //Pass in a URL and this will display it in a modal window
 function Pop_Url(URL) {
	 var w = screen.availWidth;
	 var h = screen.availHeight;
	 var popW = 560;
	 var popH = 500;
	 var wf = "";
	 var leftPos = (w - popW) / 2;
	 var topPos = (h - popH) / 2;
	 wf += "width=" + popW;
	 wf += ",height=" + popH;
	 wf += ",resizable=no";
	 wf += ",scrollbars=yes";
	 wf += ",menubar=no";
	 wf += ",toolbar=no";
	 wf += ",directories=no";
	 wf += ",location=no";
	 wf += ",status=no";
	 wf += ",left=" + leftPos;
	 wf += ",top=" + topPos;
	 myWindow = window.open(URL, "Pop_Win", wf);
	 myWindow.document.close();
	 myWindow.focus()
}