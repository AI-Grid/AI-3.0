#!perl.exe
use strict;
use warnings;
use lib qw(lib); # look in lib for modules and databases
binmode (STDOUT,':utf8'); # we can use unicode


use Robust::Util; # Dbix::Class stuff - set up the DSN in your ODBC driver, and put the details of the DSN in DSN.txt
my $schema = Robust::Util::mysql_connect;
$schema->storage->debug(0);   # set to 1 to see detailed database SQL as it is generated


# Select all records from the UserAccounts table into a recordset
my $RS =  $schema->resultset('Asset');


### OO interface:
#use Crypt::Digest::SHA256;
#
#open  (FILE, "<", "";)
#
#$d = Crypt::Digest::SHA256->new;
#
#$d->addfile(FILE);
#$result_raw  = $d->digest;     # raw bytes
#$result_hex  = $d->hexdigest;  # hexadecimal form
#$result_b64  = $d->b64digest;  # Base64 form
#$result_b64u = $d->b64udigest; # Base64 URL Safe form



my $counter = 1;
foreach my $r ($RS->all) #  read each row object into $contact
{
   print "$counter ". $r->id . "\n";
   if ($counter == 1) {
        <stdin>;
   }
   $counter++;
}


=cut
import "Data Source=localhost;Port=3306;Database=robust;User ID=robustuser;Password=robustpassword;Old Guids=true;default command timeout=3600" assets 1 1000


