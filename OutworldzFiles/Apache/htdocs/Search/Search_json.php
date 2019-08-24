<?php
// AGPL 3.0 by Fred Beckhusen
require( "flog.php" );

include("databaseinfo.php");
include("../Metromap/includes/config.php");
  
  
  // Attempt to connect to the database
  try {
    $db = new PDO("mysql:host=$DB_HOST;port=$DB_PORT;dbname=$DB_NAME", $DB_USER, $DB_PASSWORD);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_SILENT);
  }
  catch(PDOException $e)
  {
    echo "Error connecting to database\n";
    file_put_contents('../../../PHPLog.log', $e->getMessage() . "\n-----\n", FILE_APPEND);
    exit;
  }
     
  $text = $_GET['query'] || "";
  $text = "%$text%";
  $sqldata['text1'] = $text;
  
  

  $rc = intval($_GET['rp']);
   
  if ($rc == "") {
      $rc = 100;
  }
  
 
  $sort = $_GET['sortname'];
  if ($sort == 'Name') {
    $sort = 'Name';
  }else{
    $sort = 'Description';
  }
  
  $ord = $_GET['sortorder'];
  if ($ord == 'asc') {
    $ord = 'asc';
  } else {
    $ord = 'desc';
  }
  
  $qtype = $_GET['qtype'];
  if ($qtype == 'Name') {
    $qtype = 'Name';
  } else {
    $qtype = 'Description';
  }
  
  $total = 0;
   
  $page =  $_GET['page'] || 1;
  if ($page == "" ) {
    $page = 1;
  }
  
  $stack = array();

  $q = "SELECT Name, Description, Location, Regions.Regionname as  Regioname FROM Objects INNER JOIN Regions ON Objects.regionuuid = Regions.regionuuid 
    where " . $qtype . "  like :text1 
    order by " . $sort . ' ' .  $ord ;
    //. " limit " . $lim1 . "," . $lim2;     


    $query = $db->prepare($q);
    $result = $query->execute($sqldata);
    
    class OUT {}
    class Row {}
    
    $out = new OUT();
    
    while ($row = $query->fetch(PDO::FETCH_ASSOC))
    {
        
        $location = $row["Location"];
        #$location = str_replace("/","+", $location );
        
        # need grid name and region name in a url
        
        $hop  = "<a href=\"hop://". $DB_GRIDNAME . "/" . $row["Regioname"] . "/" . $location .  "/\"  class=\"hop\"><img src=\"images/Hop.png\" height=\"25\"></a>";
        
        $hop = "<a href=\"secondlife://http|!!". $DB_GRIDNAME . "+" . $row["Regioname"] . "/" . $location .  "\"  class=\"hop\"><img src=\"images/Hop.png\" height=\"25\"></a>";
        #secondlife://http|!!breath-grid.info|8002+IT+IS+ALL+FREE
        $row = array("hop"=>$hop, "Name"=>$row["Name"],"Description"=>$row["Description"],"Regionname"=>$row["Regioname"],"Location"=>$location);
        
        $rowobj = new Row();
        $rowobj->cell = $row;
        
       # foreach($rowobj->cell as $x=>$x_value)
       # {
       #   echo "Key=" . $x . ", Value=" . $x_value;
       #   echo "<br>";
       # }
  
        
        #$myJSON = json_encode($rowobj);
        #echo $myJSON . "<br>";
        if ($total >= (($page-1) *$rc) && $total < ($page) *$rc) {
          array_push($stack, $rowobj);
        }
        $total++;
        
    }
    if ($total == 0) {
	  flog("Nothing found");
      $row = array("hop"=>"", "Name"=>"No records","Description"=>"No records","Regionname"=>"No records","Location"=>"No records");
      $rowobj = new Row();
      $rowobj->cell = $row;
      array_push($stack, $rowobj);
    }

    $out->domain = $CONF_domain;
    $out->port = $CONF_port;
    $out->page  = $page;
    $out->total = $total;
    $out->rows  = $stack;
        
    $myJSON = json_encode($out);

    echo $myJSON;
   
   
?>
