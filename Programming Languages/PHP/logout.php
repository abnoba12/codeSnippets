<?php	
	session_start();	
	$saveMsg = $_SESSION['alertMessage'];
		
	setcookie("cbordUser","",time() - 3600, "/");
//	setcookie("PHPSESSID","",time() - 3600, "/");
	session_unset();
	session_destroy();
	
	session_start();
	$_SESSION = array();
	
	$_SESSION['alertMessage'] = $saveMsg;
	Header("Location: login.php");
?>
