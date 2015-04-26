<?php

/**
 * Select list helper
 *
 * Different types of select lists can be populated using this helper.
 * Use: 
 * 1. Register helper with controller. public $helpers = array('SelectList');
 * 2. Add the list to the input. echo $this->Form->input('state', array('options' => array($this->SelectList->getStateList())));
 */
class SelectListHelper extends AppHelper {
    
	/**
	 * Provides an array of all the states in the US
	 */
	public function getStateList(){
		return array('AL' => 'Alabama', 
			'AK' => 'Alaska', 
			'AZ' => 'Arizona', 
			'AR' => 'Arkansas', 
			'CA' => 'California', 
			'CO' => 'Colorado', 
			'CT' => 'Connecticut', 
			'DE' => 'Delaware', 
			'DC' => 'District of Columbia', 
			'FL' => 'Florida', 
			'GA' => 'Georgia', 
			'HI' => 'Hawaii', 
			'ID' => 'Idaho', 
			'IL' => 'Illinois', 
			'IN' => 'Indiana', 
			'IA' => 'Iowa', 
			'KS' => 'Kansas', 
			'KY' => 'Kentucky', 
			'LA' => 'Louisiana', 
			'ME' => 'Maine', 
			'MT' => 'Montana', 
			'NE' => 'Nebraska', 
			'NV' => 'Nevada', 
			'NH' => 'New Hampshire', 
			'NJ' => 'New Jersey', 
			'NM' => 'New Mexico', 
			'NY' => 'New York', 
			'NC' => 'North Carolina', 
			'ND' => 'North Dakota', 
			'OH' => 'Ohio', 
			'OK' => 'Oklahoma', 
			'OR' => 'Oregon', 
			'MD' => 'Maryland', 
			'MA' => 'Massachusetts', 
			'MI' => 'Michigan', 
			'MN' => 'Minnesota', 
			'MS' => 'Mississippi', 
			'MO' => 'Missouri', 
			'PA' => 'Pennsylvania', 
			'RI' => 'Rhode Island', 
			'SC' => 'South Carolina', 
			'SD' => 'South Dakota', 
			'TN' => 'Tennessee', 
			'TX' => 'Texas', 
			'UT' => 'Utah', 
			'VT' => 'Vermont', 
			'VA' => 'Virginia', 
			'WA' => 'Washington', 
			'WV' => 'West Virginia', 
			'WI' => 'Wisconsin', 
			'WY' => 'Wyoming');		
    }
}
