#!perl
# author - Fred Beckhusen
# AGPL 3.0

use strict;
use  warnings;
my $debug = 0;


	$|=1;
 	#use CGI::Carp('fatalsToBrowser');
	use CGI qw(:standard);
	use Config::IniFiles;
	use File::BOM;  # fixes a bug in Perl with UTF-8
	# get the path to the Settings.ini
	use Cwd;
	my $path = getcwd();
	
	$path =~ /(.*?\/Outworldzfiles)/i;
	my $file = $1 . '/Settings.ini';
	
	 # Read the Right Thing from a unicode file with BOM:
	open(CONFIG , '<:via(File::BOM)', $file);   
	my $Config = Config::IniFiles->new(-file => *CONFIG);
	
	if (! $Config)  {
	   print "Cannot read INI";
	   return;
	#   
	#   foreach my $line (@Config::IniFiles::errors)
	#   {
	#      print $line;
	#   }
	}
 
	my $public = $Config->val('Data','SimVisitPublic')|| '';
	if (lc($public) ne 'public') {
		 my $env = $ENV{REMOTE_ADDR} || '127.0.0.1' ;
		 if ($env ne '127.0.0.1')
		 {
			 print header;		
			 exit;
		 }
	}

	
	use URI::Escape;    	
	use lib qw(lib .);
	use MYSQL;
	
	my  $Data = new MYSQL();
	my  $Data1 = new MYSQL();	

	my  $Input = CGI->new();
	my $sim;
	my $Slurl;
	my $totalvisits;
    
	my $q  = uri_unescape($Input->param('q')) || '';
	my $person  = uri_unescape($Input->param('person')) || '';
	my $s;
	my $e;

	my @sims ;
	my $text ;			
	my @response ;
	
	my $start  = uri_unescape($Input->param('Start')) || '';
	my $end  = uri_unescape($Input->param('End')) || '';
	
	if ($debug) {
		$q = 'Virunga';		
	}
	
	$s = $start;
	$e = $end;

	
	# mm-dd-yyyy to yyyy-mm-dd
	if ($start =~ /(\d+)\/(\d+)\/(\d+)/) {
		$start = $3 . '-' . $1 . '-' . $2 ;
	} else {
		my $thirty_ago = DateTime->today->subtract(days => 30);		
		$start = $thirty_ago->ymd('/')
	}

	if($end =~ /(\d+)\/(\d+)\/(\d+)/) {
		$end = $3. '-' . $1. '-' . $2;
	} else {
		my $tomorrow = DateTime->today->add(days => 1);		
		$end = $tomorrow->ymd('/');
	}
	
	
	my $sql;
	

	if ($person)
	{
		my $sql = qq!select count(*) as count, name from Visitor
		where
		regionname = ?
		and dateupdated >= ?
		and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
		and name = ?
		group by name
		!			;

		if ($Data->Prepare($sql))						{&Print_ODBC_Error($Data,__FILE__,__LINE__);	}
		if ($Data->Execute($q, $start,$end, $person))	{&Print_ODBC_Error($Data,__FILE__,__LINE__);	}

	}
	else
	{
		$sql = qq!select count(*) as count , name from Visitor
		where
		regionname = ?
		and dateupdated >= ?
		and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
		group by name
		order by name
		!;

		if ($Data->Prepare($sql))	{	    	    &Print_ODBC_Error($Data,__FILE__,__LINE__);	}
		if ($Data->Execute($q, $start,$end))	{	    	    &Print_ODBC_Error($Data,__FILE__,__LINE__);	}
	}
	my $time = 0;
	my $visits = 0;

	while ($Data->FetchRow())
	{

		my %Data = $Data->DataHash();
		$totalvisits ++;
		$visits = $Data{count};
		my $name = $Data{name};

		if ($person)
		{
			my $sql = qq!select LocationX,LocationY from Visitor
				where
				name = ?
				and regionname = ? 
				and dateupdated >= ?
				and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
				order by dateupdated
				!;

			if ($Data1->Prepare($sql))						{	    	    &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
			if ($Data1->Execute($name, $q, $start,$end))	{	    	    &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
		}
		else
		{
			my $sql = qq!select LocationX,LocationY from Visitor
				where
				name = ?
				and regionname= ? 
				and dateupdated >= ?
				and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
				order by dateupdated
				!;

			if ($Data1->Prepare($sql))						{	    	    &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
			if ($Data1->Execute($name, $q, $start,$end))	{	    	    &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
		}
		my @vectors;
		while ($Data1->FetchRow())
		{
			my %Data = $Data1->DataHash();

			push @vectors,   {
				X => int($Data{LocationX}),
				Y => int($Data{LocationY}),
				Z => 20,
			};
			$time++;
		}

		push @response,   {
				name => $name,
				visits => $visits,
				vectors => \@vectors,
			};

	}
	
	if (scalar @response == 0) {
		my @vectors;
		push @response,   {
				name => 'none',
				visits => 0,
				vectors => \@vectors,
		};
	}
	

	if ($person)
	{
		$sql = qq!select distinct name,year(dateupdated) as yy,month(dateupdated) as mm, day(dateupdated) as dd
					from Visitor where
					regionname = ? 
					and dateupdated >= ?
					and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
					and name = ?
				group by  name,year(dateupdated),month(dateupdated), day(dateupdated)

		!;
		if ($Data->Prepare($sql))						{	    	    &Print_ODBC_Error($Data,__FILE__,__LINE__);	}
		if ($Data->Execute( $q, $start,$end, $person))	{	    	    &Print_ODBC_Error($Data,__FILE__,__LINE__);	}
	}
	else
	{
		$sql = qq!select distinct name,year(dateupdated) as yy,month(dateupdated) as mm, day(dateupdated) as dd
					from Visitor where
					regionname = ?
					and dateupdated >= ?
					and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
				group by  name,year(dateupdated),month(dateupdated), day(dateupdated)

		!;
		if ($Data->Prepare($sql))						{	    	    &Print_ODBC_Error($Data,__FILE__,__LINE__);	}
		if ($Data->Execute( $q, $start,$end))	{	    	    &Print_ODBC_Error($Data,__FILE__,__LINE__);	}
	}
	my %daycount;
	my @timespent ;

	while ($Data->FetchRow())
	{
		my %Data = $Data->DataHash();
		my $name = $Data{name};
		my $mm = $Data{mm};
		my $dd = $Data{dd};
		my $yy = $Data{yy};

		if ($person)
		{
			my $sql = qq!
				select count(*) as count from Visitor where
					regionname = ? 
					and month(dateupdated) = ?
					and day(dateupdated) =?
					and year(dateupdated) =?
					and name = ?

			!;
			if ($Data1->Prepare($sql))						{ &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
			if ($Data1->Execute( $q, $mm,$dd, $yy, $name))			{ &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
		}
		else
		{
			my $sql = qq!
				select count(*) as count
				from Visitor
				where
					regionname = ? 
					and month(dateupdated) = ?
					and day(dateupdated) =?
					and year(dateupdated) =?


			!;
			if ($Data1->Prepare($sql))						{ &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
			if ($Data1->Execute( $q,$mm,$dd, $yy))			{ &Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
		}

		if ($Data1->FetchRow())
		{
			my %Data = $Data1->DataHash();
			$daycount{"$mm-$dd-$yy"} ++;
		}
	}

	foreach my $date (sort keys %daycount)
	{
		push @timespent, {
							date=> $date ,
							count => $daycount{$date},
						};
	}


	######################### DAILY

	if ($person)
	{
		$sql = qq!select count(*) as count, month(dateupdated) as mm,day(dateupdated) as dd, year(dateupdated) as yy
				from Visitor where
				regionname = ?
				and dateupdated >= ?
				and dateupdated < DATE_ADD(? , INTERVAL 1 DAY)
				and name = ?
				group by year(dateupdated) ,month(dateupdated) , day(dateupdated)
				order  by  year(dateupdated) ,month(dateupdated) , day(dateupdated)

		!;
		if ($Data1->Prepare($sql))								{&Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
		if ($Data1->Execute($q,$start,$end,$person))			{&Print_ODBC_Error($Data1,__FILE__,__LINE__);	}

	}
	else
	{
		$sql = qq!select count(*) as count, month(dateupdated) as mm,day(dateupdated) as dd, year(dateupdated) as yy
					from Visitor where
					regionname = ? 
					and dateupdated >= ?
					and dateupdated <DATE_ADD(? , INTERVAL 1 DAY)

					group by year(dateupdated) ,month(dateupdated) , day(dateupdated)
					order  by  year(dateupdated) ,month(dateupdated) , day(dateupdated)

			!;
		if ($Data1->Prepare($sql))						{&Print_ODBC_Error($Data1,__FILE__,__LINE__);	}
		if ($Data1->Execute($q,$start,$end))			{&Print_ODBC_Error($Data1,__FILE__,__LINE__);	}

	}
	my @daily ;
	while ($Data1->FetchRow())
	{
		my %Data = $Data1->DataHash();
		push @daily, {
						 date => "$Data{mm}-$Data{dd}-$Data{yy}",
						 count => $Data{count} + 0
						};
	}
	my $title;
	if ($person)
	{
		$title = " for $person";
	}
	else
	{
		$title = ' for all people';
	}
	
	
	use lib qw(lib .);
	use Util;
	my $schema = Util::mysql_connect;
	$schema->storage->debug(0);		
	
	
	my $rs = $schema->resultset('Sim')
			->search({regionname => $q})
			->first;


	my $xsize = 255;
	my $ysize = 255;
	my $XCoord;
	my $YCoord;
	
	if ($rs)
	{
		my $regionsize = $rs->regionsize;
		
		$xsize = $regionsize;
		$ysize = $regionsize;
		$XCoord = $rs->locationX;
		$YCoord = $rs->locationY;
	}
	
	#$end =~ /(\d+)-(\d+)-(\d+)/;
	#$end = $2 . '/' . $3 . '/' . $1 ;
	#
	#$start =~ /(\d+)-(\d+)-(\d+)/;
	#$start = $2 . '/' . $3 . '/' . $1 ;
	#
		
	use JSON;
	print header('application/json');
	print to_json({ data=> \@response,
					sims=> \@sims ,
					sim=>$q,
					time=>$time,
					s=>$s,
					e=>$e,
					visits => $totalvisits,
					start=>$start,
					end =>$end,
					timespent =>\@daily,
					daily=>\@timespent,
					title=>$title,
					xsize => $xsize,
					ysize => $ysize,						
					text => $text,
					XCoord => $XCoord,
					YCoord => $YCoord,
				});	exit;
	


sub GetDate
{
	use DateTime;
	return DateTime->now->set_time_zone('America/Los_Angeles')->ymd('/');
}

sub GetTime
{
	use DateTime;
	return DateTime->now->set_time_zone('America/Los_Angeles')->hms(':');
}






sub Print_ODBC_Error
{
	use strict;
    my($Data,$File,$Line,$ExitandDie) = @_;

    my $Errornum;
	$Errornum = $Data->Error();
	my $sql = $Data->GetSQL();
	our $error;
	if ($Errornum)
    {        
		$error =  "PID $$:Fatal Error at Line $Line in $File $Errornum\n";
		errorLOG("$sql, $error, $Line in $File");	
    }
	else
	{
		$error =  "PID $$:Unknown ODBC Error at Line $Line in $File\n";        		
		errorLOG("$sql. $error, $Line in $File");
	}

    exit ;  # just leave

}




sub errorLOG
{
	use strict;
    my $Object = shift;
	my $Date = GetDate();
	$Date =~ s/\//-/g;

    open (LOG, ">>:utf8", "$Date-error.log");
    my $Time = GetTime();
    print LOG "$Time:\t$0\t$Object\n";
	close LOG;
}
