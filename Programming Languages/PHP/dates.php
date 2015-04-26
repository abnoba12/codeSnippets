<?php
	//Came orgionally from the CBORD project file \full\history.php

	//Set the Date vars
	$now = getdate();       

	$date = new DateTime();                                                 
	$date->setDate($now['year'], $now['mon'], $now['mday']);
	
	$after = new DateTime();                                                        
	$after->setDate($now['year'], $now['mon'], $now['mday']);
	
	//Go back 6 months
	$date->modify('-6 month');
	
	$start = $date->format("Y-m-d");
	$end = $after->format("Y-m-d");
	
	
	
	//date selector of months
	$date->setDate($now['year'], $now['mon'], "01");        
	for($i = 0; $i < 7; $i++){
		$month = $date->format("n");
		$month = $month_names[$month];
		$default = "";
		if(isset($_POST['date']) && $_POST['date'] == $date->format("Y-m-d")) $default = "selected=\"selected\"";
		echo '<option value="'.$date->format("Y-m-d").'" ',$default,'>', $month," ", $date->format("Y"), '</option>';
		$date->modify('-1 month');
	}

?>