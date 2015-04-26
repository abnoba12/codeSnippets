<?php
	//Came orgionally from the CBORD project file \inc\common.php


	/**
	 * Used to paginate a page. The last variable in the GET variables must always been the page number
	 * @param Integer $totalPages Number of pages for pagination
	 * @param Integer $showPages Number of pages to show at a time
	 * @param Integer $currentPage Page you are currently on
	 * @param String $destination URI for the 
	 * @return Array An array of URLs in order
	 */
	function paginate($totalPages, $showPages, $currentPage, $destination){
		$result = array();
		$start = 1;
		
		if(!isset($currentPage)) $currentPage = 1;
		if($currentPage > $showPages){
			$start = (floor($currentPage / $showPages)*10)+1;
			array_push($result, "<a href=\"".$destination.($start-1)."\"><img src=\"images/left_arrow.jpg\" alt=\"Previous\"/></a>");
		}
		
		$count = 1;
		for($x = $start; $x <= $totalPages && $count <= $showPages; $x++){
			$class = "";
			if($x == $currentPage) $class = "class=\"currentPage\"";
			array_push($result, "<a $class href=\"$destination$x\">$x</a>");
			$count++;
		}
		
		if(($start+$showPages) < $totalPages){
			array_push($result, "<a href=\"".$destination.($start+$showPages)."\"><img src=\"images/right_arrow.jpg\" alt=\"Next\"/></a>");
		}
		return $result;
	}
?>