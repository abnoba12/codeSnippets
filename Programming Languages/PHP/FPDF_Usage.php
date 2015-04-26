<?php
	//Came orgionally from the CBORD project file \full\historyPDF.php

require_once('../lib/Fpdf.php');

class PDF extends FPDF {
	//** Page header.
	function Header(){
		$institName = $_SESSION['institution']->return->name;
		$logo = $_SESSION['institution_logo'];
		
		if(isset($logo)){
			//** Logo
			$this->Image($logo->url, 10 ,8 ,33);
		} else {
			//** Arial bold 15
			$this->SetFont('Arial','B',15);
			//** Move to the right
			$this->Cell(80);
			//** Title
			$this->Cell(30, 10, $institName, 1, 0, 'C');
		}
		$userInfo = $_SESSION['_u'];
		$full_name = $userInfo->firstName . ' ' . $userInfo->middleName . ' ' . $userInfo->lastName;
		$this->SetXY(100, 12);
		$this->MultiCell(100, 6, $full_name, 0, 'R');
		$today = date('F j, Y');
		$this->SetXY(100, 18);
		$this->MultiCell(100, 6, $today, 0, 'R');
		
		$this->Line(10, 33, 200, 33);
		//** Line break
		$this->Ln(18);
	}

	//** Page footer.
	function Footer(){
		//** Position at 1.5 cm from bottom.
		$this->SetY(-15);
		//** Arial italic 8.
		$this->SetFont('Arial','I',8);
		$institName = $_SESSION['institution']->return->name;
		$this->Cell(30, 10, $institName, 0, 0, 'L');
		
		$this->SetXY(50, -15);
		$this->MultiCell(40, 4, "Direct Inquiries To:", 0, 'L');
		$this->SetXY(80, -15);
		$this->MultiCell(40, 4, "ManageMyID \nCBORD University 1234 Brown Rd, Ithaca NY 14850", 0, 'L');
		$this->SetXY(120, -15);
		$this->MultiCell(40, 4, "Phone: 202.987.6543", 0, 'L');
		
		//** Page number.
		$this->Cell(0,10,'Page '.$this->PageNo(),0,0,'R');
		
	}

	/**
	 * Convert a hexa decimal color code to its RGB equivalent
	 *
	 * @param string $hexStr (hexadecimal color value)
	 * @param boolean $returnAsString (if set true, returns the value separated by the separator character. Otherwise returns associative array)
	 * @param string $seperator (to separate RGB values. Applicable only if second parameter is true.)
	 * @return array or string (depending on second parameter. Returns False if invalid hex color value)
	 */                                                                                                
	function hex2RGB($hexStr, $returnAsString = false, $seperator = ',') {
	    $hexStr = preg_replace("/[^0-9A-Fa-f]/", '', $hexStr); // Gets a proper hex string
	    $rgbArray = array();
	    if (strlen($hexStr) == 6) { //If a proper hex code, convert using bitwise operation. No overhead... faster
	        $colorVal = hexdec($hexStr);
	        $rgbArray['red'] = 0xFF & ($colorVal >> 0x10);
	        $rgbArray['green'] = 0xFF & ($colorVal >> 0x8);
	        $rgbArray['blue'] = 0xFF & $colorVal;
	    } elseif (strlen($hexStr) == 3) { //if shorthand notation, need some string manipulations
	        $rgbArray['red'] = hexdec(str_repeat(substr($hexStr, 0, 1), 2));
	        $rgbArray['green'] = hexdec(str_repeat(substr($hexStr, 1, 1), 2));
	        $rgbArray['blue'] = hexdec(str_repeat(substr($hexStr, 2, 1), 2));
	    } else {
	        return false; //Invalid hex color code
	    }
	    return $returnAsString ? implode($seperator, $rgbArray) : $rgbArray; // returns the rgb string or the associative array
	}
	
	//** Colored table
	function FancyTable($header,$data) {		
		//** Colors, line width and bold font.
		
		//Get the school color
		foreach($_SESSION['header']->return->string as $item){
			if($item->name == "school_color") $schoolColor = $item->value;
		}
		if(isset($schoolColor)){
			$rgb = $this->hex2RGB($schoolColor);		
			$this->SetFillColor($rgb['red'], $rgb['green'], $rgb['blue']);
		}else{
			$this->SetFillColor(255,0,0);
		}		
		$this->SetTextColor(255);
		$this->SetDrawColor(200,200,200);
		$this->SetLineWidth(.3);
		$this->SetFont('Arial','B');
		$this->SetFontSize(10);

		//** Header
		$w=array(45,55,55,35);
		for($i=0;$i<count($header);$i++)
			$this->Cell($w[$i],7,$header[$i],1,0,'C',true);
		$this->Ln();

		//** Color and font restoration
		$this->SetFillColor(244,244,244);
		$this->SetTextColor(0);
		$this->SetFont('Arial');
		$this->SetFontSize(8);
		
		//** Data
		$fill=false;
		foreach($data->Transaction as $row) {
			$this->SetTextColor(41, 87, 162);
			$this->Cell($w[0], 6, $row->accountName, 'LR', 0, 'L', $fill);	
			$this->SetTextColor(0);		
			$tmp = date('F j, Y', strtotime($row->postedDate)) . " | " . date('g:iA', strtotime($row->postedDate));
			$this->Cell($w[1],6,$tmp,'LR',0,'C',$fill);
			$this->Cell($w[2],6,$row->locationName,'LR',0,'C',$fill);
			if($row->transactionType == 1 || $row->transactionType == 4) {
				$this->SetTextColor(190, 45, 45);
				$tmp =  '-' . $row->amount;
			} else {
				$this->SetTextColor(66, 127, 26);
				$tmp =  '+' . $row->amount;
			}				
			$this->Cell($w[3],6,$tmp,'LR',0,'C',$fill);
			$this->SetTextColor(0);
			$this->Ln();
			$fill = !$fill;
		}
		$this->Cell(array_sum($w),0,'','T');
	}
	
	function emptyTable($text){
		$this->Cell(200,6, $text,'LR',0,'C', false);
	}
}
	
	if(!isset($_SESSION['login'])) Header("Location: login.php");
	
	//** Default labels.
	$account_name			= "Account Name";
	$date_time				= "Date &amp; Time";
	$activity_details		= "Activity Details";
	$amount_points			= "Amount ($ / Meals)";
	$statement_period		= "Statement Period:";
	$to						= "To";
	$begin_balance			= "Beginning Balance:";
	$end_balance			= "Ending Balance:";
	$direct_inquires		= "Direct Inquiries To:";
	$phone					= "Phone:";
	$other_questions		= "Other Questions?";
	$statement_for			= "Showing Statement For:";
	$no_transactions		= "No Transactions";
	
	$content = new Content();
	$screenText = $content->retrieveStringList($_SESSION['institutionId'], "en-US", "get_web_gui", "view_history_screen");
	if(isset($screenText->return->string)){
		//** Load the label variables from the list.
		foreach($screenText->return->string as $element){
			if($element->name == 'account_name')			$account_name = htmlspecialchars($element->value);
			if($element->name == 'date_time')				$date_time = htmlspecialchars($element->value);
			if($element->name == 'activity_details')		$activity_details = htmlspecialchars($element->value);
			if($element->name == 'amount_points')			$amount_points = htmlspecialchars($element->value);
			if($element->name == 'statement_period')		$statement_period = htmlspecialchars($element->value);
			if($element->name == 'to')						$to = htmlspecialchars($element->value);
			if($element->name == 'begin_balance')			$begin_balance = htmlspecialchars($element->value);
			if($element->name == 'end_balance')				$end_balance = htmlspecialchars($element->value);
			if($element->name == 'direct_inquires')			$direct_inquires = htmlspecialchars($element->value);
			if($element->name == 'phone')					$phone = htmlspecialchars($element->value);
			if($element->name == 'other_questions')			$other_questions = htmlspecialchars($element->value);
			if($element->name == 'alert_message_1')			$alert_message_1 = htmlspecialchars($element->value);
			if($element->name == 'found_card_link')			$found_card_link = htmlspecialchars($element->value);
			if($element->name == 'statement_for')			$statement_for = htmlspecialchars($element->value);
			if($element->name == 'no_transactions')			$no_transactions = htmlspecialchars($element->value);
		}
	} else {
		displayError(NULL, $error_message['localization'], $screenText, NULL);
	}

	if(isset($_GET['account'])) $currentAccount	= $_GET['account']; else $currentAccount = NULL;
	if(isset($_GET['dateS'])) $currentDateStart = $_GET['dateS']; else $currentDateStart = NULL; 
	if(isset($_GET['dateE'])) $currentDateEnd = $_GET['dateE']; else $currentDateEnd = NULL;
	$commerce = new Commerce();
	$transactions = $commerce->retrieveCashlessTransactionHistory($userInfo->institutionId, $userInfo->id, $currentAccount, $currentDateStart, $currentDateEnd, 35, NULL);
	$numberOfTransactions = $transactions->totalCount;
	$pages = ceil($numberOfTransactions / 35);

	$pdf=new PDF();
	//** Column titles
	$header=array('Account Name:','Date & Time','Activity Details','Amount ($ / meals)');	
	for($x = 1; $x <= $pages; $x++){
		set_time_limit (10);
		$transactions = $commerce->retrieveCashlessTransactionHistory($userInfo->institutionId, $userInfo->id, $currentAccount, $currentDateStart, $currentDateEnd, 35, ($x-1) * 35);
		//** Data loading
		$pdf->SetFont('Arial','',10);
		$pdf->AddPage();
		$pdf->FancyTable($header, $transactions);
	}
	if($pages < 1){
		//** Data loading
		$pdf->SetFont('Arial','',10);
		$pdf->AddPage();
		//$pdf->emptyTable($no_transactions);
		$pdf->Text(10, 45, $no_transactions);
		
	}
	$pdf->Output();
?>