<?php
	//Came orgionally from the CBORD project file \inc\notify.php

//This uses a series of session variables to display messages in the header.
	//** One time message display.
	if (isset($_SESSION['singleMessage']) && !empty($_SESSION['singleMessage'])) {
		echo '<div id="notification"><h3>';
		echo 	$_SESSION['singleMessage']; 
		echo '</h3></div>';
		
		unset($_SESSION['singleMessage']);
	}
	
	//** A persistant message that must me manually removed.
	if (isset($_SESSION['perMessage']) && !empty($_SESSION['perMessage'])) {
		echo '<div id="perMessage"><h3>';
		echo 	$_SESSION['perMessage']; 
		echo '</h3></div>';
	}
?>
