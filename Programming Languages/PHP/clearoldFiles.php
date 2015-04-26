<?php
$days = "15"; // delete all files older than this many days
$seconds = ({days}*24*60*60);

$dir    = 'c:/delete/';
$files = scandir($dir);

foreach ($files as $num => $fname){
	if (file_exists("{$dir}{$fname}") && ((time() - filemtime("{$dir}{$fname}")) > $seconds)) {
		$mod_time = filemtime("{$dir}{$fname}");
		if (unlink("{$dir}{$fname}")){$del = $del + 1; echo "Deleted: {$del} - {$fname} --- ".(time()-$mod_time)." seconds old";}
	}
}
?>