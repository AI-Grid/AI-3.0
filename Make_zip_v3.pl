# build Zipfile
# AGPL licensed, see AGPL 3.0 at https://www.gnu.org/licenses/agpl-3.0.en.html


use strict;
use warnings;
use IO::All -utf8; 
use 5.010;
use File::Copy;
use File::Path;
use File::Basename;
use File::Find;
use File::Copy::Recursive qw(dircopy);

use Cwd;
my $dir = getcwd;

# paths
my $src= "$dir/Installer_Src/Setup DreamWorld/GlobalSettings.vb";
my $Contabo = '\\\\contabo2.outworldz.com/c';
my $Fleta = '\\\\fleta/c';
my $Dest = "E:/Dropbox/Dreamworld/Zip/DreamGrid.zip";

CheckDistro();

# This requires a Authenticode Certificate to sign the files. The thumbprint comes from the cert. It is not the cert, which is privat and is saved in the windows store.
# convert the Cert to a pfx file:  .\bin\openssl pkcs12 -export -out cert.pfx -inkey 2021.key -in 2021.cer
# import the PK and the
my $thumbprint = '6f50813b6d0e1989ec44dc90714269f8404e7ab1';    # 2021

my $zip = '/Opensim/Zip/';
JustDelete($zip);

my $v = GetVersion($src);

my $Version = `git rev-parse --short HEAD `;
chomp $Version;
$Version > io('GitVersion');
PrintDate("GitVersion $Version");

my $type = '-V' . $v;


PrintDate("Building DreamGrid$type.zip");

PrintDate('Server Publish ? <p = publish, c = clean, enter = make the zip only>');
my $publish = <stdin>;
chomp $publish;


$v > io("$dir/Version.txt");


my $start = GetDate() . " " . GetTime() . "\n";
my @languages =
  qw (ar ar-SA es ja ko es-ES fa ca cs da de el en es-MX eu fa-IR fi fr ga he  is it nl-NL no pl pt pt-BR ru sv tr vi zh-cn zh-tw zh-Hans-HK  zh-Hans zh-Hant  );
foreach my $lang (@languages) {
    JustDelete($lang);
}

my @a = io->dir('.')->all;

foreach my $f (@a) {
    if ( $f->name =~ /tmp.*\.html/ ) {
        PrintDate("Delete " . $f->name);
        $f->unlink;
    }
}

DelZips();

PrintDate("Clean up fsassets");

my $todo = qq!DEL /F/Q/S "$dir/OutworldzFiles/opensim/bin/fsassets""!;
`$todo`;
$todo = qq!RMDIR /Q/S  "$dir/OutworldzFiles/opensim/bin/fsassets"!;
`$todo`;

PrintDate("Clean up opensim");


my @deletions = (
    "$dir/OutworldzFiles/AutoBackup",
    "$dir/OutworldzFiles/Opensim/WifiPages-Custom",
    "$dir/OutworldzFiles/Opensim/bin/WifiPages-Custom",
    "$dir/OutworldzFiles/Opensim/WifiPages",
    "$dir/OutworldzFiles/Opensim/bin/WifiPages",
    "$dir/OutworldzFiles/Opensim/bin/datasnapshot",
    "$dir/OutworldzFiles/Opensim/bin/assetcache",
    "$dir/OutworldzFiles/Opensim/bin/j2kDecodeCache",
    "$dir/OutworldzFiles/Opensim/bin/MeshCache",
    "$dir/OutworldzFiles/Opensim/bin/ScriptEngines",
    "$dir/OutworldzFiles/Opensim/bin/maptiles",
    "$dir/OutworldzFiles/Opensim/bin/bakes",
    "$dir/OutworldzFiles/logs/",
    "$dir/OutworldzFiles/logs/Apache",
    "$dir/OutworldzFiles/Apache/htdocs/Stats/Maps/",
    "$dir/OutworldzFiles/Apache/htdocs/TTS",
);

foreach my $path (@deletions) {
    PrintDate($path);
    DeleteandKeep($path);
}

JustDelete('/Opensim/Zip');
DelMaps();

delPDB("$dir/OutworldzFiles/");

doUnlink ("$dir/BareTail.udm" );
doUnlink ("$dir/SET_externalIP-PrintDate.txt");
doUnlink ("$dir/OutworldzFiles/Photo.png");
doUnlink ("$dir/OutworldzFiles/XYSettings.ini");
doUnlink ("$dir/OutworldzFiles/Opensim/bin/OpensimConsoleHistory.txt");
doUnlink ("$dir/OutworldzFiles/Opensim/bin/RobustConsoleHistory.txt");
doUnlink ("$dir/OutworldzFiles/Opensim/bin/LocalUserStatistics.db");
doUnlink ("$dir/OutworldzFiles/BanList.txt");

#Setting
doUnlink ("$dir/Outworldzfiles/Settings.ini");

#logs
doUnlink ("$dir/Outworldzfiles/Icecast/PrintDate/error.PrintDate");
doUnlink ("$dir/Outworldzfiles/Icecast/PrintDate/access.PrintDate");

doUnlink ("$dir/UpdateGrid.PrintDate");
doUnlink ("$dir/OutworldzFiles/Apache/htdocs/Search/flog.PrintDate");
doUnlink ("$dir/OutworldzFiles/Opensim/bin/Robust.PrintDate");
doUnlink ("$dir/OutworldzFiles/Opensim/bin/RobustStats.PrintDate");
doUnlink ("$dir/OutworldzFiles/Opensim/bin/Opensimstats.PrintDate");
###

doUnlink ("$dir/OutworldzFiles/Logs/Restart.PrintDate");
doUnlink ("$dir/OutworldzFiles/Logs/Diagnostics.PrintDate");
doUnlink ("$dir/OutworldzFiles/Logs/Outworldz.PrintDate");
doUnlink ("$dir/OutworldzFiles/Logs/upnp.PrintDate");
doUnlink ("$dir/OutworldzFiles/Logs/http.PrintDate");
doUnlink ("$dir/OutworldzFiles/Logs/Error.PrintDate");
doUnlink ("$dir/OutworldzFiles/Logs/Teleport.PrintDate");

#zips
doUnlink ("../Zips/DreamGrid$type.zip");
doUnlink ("../Zips/Outworldz-Update$type.zip");
doUnlink ("$dir/DreamGrid.zip");

say "DLL List Build";
use File::Find;

open( OUT, ">", 'dlls.txt' );

find(
    { wanted => \&process_file, no_chdir => 1 },
    $dir . '/OutworldzFiles/Opensim/bin/'
);

# these are needed in this file even if deleted on disk.
print OUT "\\OutworldzFiles\\opensim\\bin\\jOpensimProfile.Modules.dll\n";
print OUT "\\OutworldzFiles\\opensim\\bin\\jOpensimSearch.Modules.dll\n";
print OUT "\\OutworldzFiles\\opensim\\bin\\jOpensimMoney.Modules.dll\n";

close OUT;

doUnlink ("$dir/Start.exe.lastcodeanalysissucceeded");
doUnlink ("$dir/Start.exe.CodeAnalysisLog.xml");



PrintDate("Signing Release");
my $exes = "$dir/Installer_Src/Setup DreamWorld/bin/Release";
#sign($exes);

use File::Copy::Recursive qw(dircopy);
dircopy( $exes, $dir ) or die("$!\n");

PrintDate("Signing copies");
use IO::All;
sign($dir);

say "Processing Main Zip\n";



my @files = `cmd /c dir /b `;

# Just do files, dirs are explicitly copied over
foreach my $file (@files) {
    chomp $file;
    next if -d "$dir/$file";

    next if $file =~ /^\./;
    ProcessFile("\"$dir\\$file\"");
}
PrintDate("Adding folders");

# just dirs
ProcessDir('MSFT_Runtimes');
ProcessDir('Read.Me');
ProcessDir('Licenses_to_Content');
ProcessDir('OutworldzFiles');


foreach my $lang (@languages) {
    ProcessDir($lang);
}

PrintDate("Drop mysql files from update");

# now delete the mysql from the UPDATE


PrintDate("Drop Mysql from update");
DeleteandKeep("$zip/Outworldzfiles/mysql/Data");
PrintDate("Drop JOpensim Folder");

DeleteandKeep("$zip/Outworldzfiles/Apache/htdocs/jOpensim");

if (
    !copy(
        "$zip/Outworldzfiles/jOpensim_Files/default.htm",
        "$zip/Outworldzfiles/Apache/htdocs/jOpensim/default.htm"
    )
  )
{
    die $!;
}

PrintDate("Drop bin Folders");
DeleteandKeep("$zip/OutworldzFiles/Opensim/bin/Regions");
DeleteandKeep("$zip/OutworldzFiles/Opensim/bin/fsassets");

PrintDate("Drop Opensim Source code from update");
JustDelete("$zip/Outworldzfiles/Opensim/Opensim");
JustDelete("$zip/Outworldzfiles/Opensim/packages");
JustDelete("$zip/Outworldzfiles/Opensim/runprebuild19.sh");
JustDelete("$zip/Outworldzfiles/Opensim/jOpenSimProfile.Modules.dll");
JustDelete("$zip/Outworldzfiles/Opensim/jOpenSimSearch.Modules.dll");
JustDelete("$zip/Outworldzfiles/Opensim/clean.sh");
JustDelete("$zip/Outworldzfiles/Opensim/cleanaot.sh");
JustDelete("$zip/Outworldzfiles/Opensim/prebuild48.xml");
JustDelete("$zip/Outworldzfiles/Opensim/makeaot.sh");
JustDelete("$zip/Outworldzfiles/Opensim/runprebuild19.bat");
JustDelete("$zip/Outworldzfiles/Opensim/addon-modules");
JustDelete("$zip/Outworldzfiles/Opensim/.vs");
JustDelete("$zip/Outworldzfiles/Opensim/.nant");
JustDelete("$zip/Outworldzfiles/Opensim/bin/addin-db-002");
JustDelete("$zip/Outworldzfiles/Opensim/Doc");
JustDelete("$zip/Outworldzfiles/Opensim/Prebuild");
JustDelete("$zip/Outworldzfiles/Opensim/share");
JustDelete("$zip/Outworldzfiles/Opensim/Thirdparty");
JustDelete("$zip/Outworldzfiles/Opensim/.git");
JustDelete("$zip/Outworldzfiles/Opensim/.gitignore");
JustDelete("$zip/Outworldzfiles/Opensim/.hgignore");
JustDelete("$zip/Outworldzfiles/Opensim/BUILDING.md");
JustDelete("$zip/Outworldzfiles/Opensim/compile.bat");
JustDelete("$zip/Outworldzfiles/Opensim/makefile");
JustDelete("$zip/Outworldzfiles/Opensim/nant-color");
JustDelete("$zip/Outworldzfiles/Opensim/BUILDING.md");
JustDelete("$zip/Outworldzfiles/Opensim/Opensim.sln");
JustDelete("$zip/Outworldzfiles/Opensim/Opensim.build");
JustDelete("$zip/Outworldzfiles/Opensim/prebuild.xml");
JustDelete("$zip/Outworldzfiles/Opensim/runprebuild.bat");
JustDelete("$zip/Outworldzfiles/Opensim/runprebuild.sh");
JustDelete("$zip/Outworldzfiles/Opensim/TESTING.txt");
JustDelete("$zip/OutworldzFiles/Opensim/bin/.git");
JustDelete("$zip/OutworldzFiles/Opensim/Ezombie");

JustDelete("$zip/Make_zip_v3.pl");
JustDelete("$zip/Start.vshost.exe.manifest");
JustDelete("$zip/Start.vshost.exe.config");
JustDelete("$zip/Start.vshost.exe");

say "Make zip\n";
doUnlink ("/Opensim/Zips/DreamGrid$type.zip");

my $dest = "/Opensim/Zips/DreamGrid$type.zip";

use Archive::Zip qw( :ERROR_CODES :CONSTANTS );

my $finalzip = Archive::Zip->new();
$finalzip->addTree($zip);

unless ( $finalzip->writeToFileNamed($dest) == AZ_OK ) {
    die 'write error';
}

Copyall:

if ( $publish =~ /p/ ) {

    PrintDate("Publishing now");
   
    CheckDistro();
    CopyManuals();
        
        
    ################################################
        
    doUnlink("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Older Versions/DreamGrid/DreamGrid-Update$type.zip");
    doUnlink("$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/Older Versions/DreamGrid/DreamGrid-Update$type.zip");
    
    doUnlink("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Older Versions/DreamGrid/DreamGrid$type.zip");
    doUnlink("$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/Older Versions/DreamGrid/DreamGrid$type.zip");
    
    PrintDate("Copy Other Versions/DreamGrid$type.zip");
    if (!copy("../Zips/DreamGrid$type.zip",
            "$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Other Versions/DreamGrid$type.zip")){
        die $!;
    }
    if (!copy("../Zips/DreamGrid$type.zip",
              "$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/Other Versions/DreamGrid$type.zip")){
        die $!;
    }
    
    
    ################################################

    PrintDate("Copy Other Versions/DreamGrid-Update$type.zip");
    if (!copy("../Zips/DreamGrid$type.zip",
              "$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Other Versions/DreamGrid-Update$type.zip")){
        die $!;
    }
    
    if (!copy("../Zips/DreamGrid$type.zip",
              "$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/Other Versions/DreamGrid-Update$type.zip")){
        die $!;
    }
    
    
    
    ################################################
    
    
    doUnlink ("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip");
    doUnlink ("$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip");
    
    doUnlink("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update.zip");
    doUnlink("$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update.zip");    

    say("Copy /Read.me/Revisions.txt");
            
    
    if (!copy("$dir/Read.me/Revisions.txt", 
              "$Fleta/Inetpub/Secondlife/Outworldz_Installer/Revisions.txt"))
    {
        die $!;
    }
    
    if (!copy("$dir/Read.Me/Revisions.txt",
            "$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Revisions.txt"))
    {
        die $!;
    }
    
    
    ################################################
    
    say("Copy DreamGrid.zip");     
    
    doUnlink("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip");
    
    if (!copy("../Zips/DreamGrid$type.zip",
            "$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip"))
    {
        die $!;
    }
    
    if (!copy("../Zips/DreamGrid$type.zip",
            "$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip"))
    {
        die $!;
    }
    
    ################################################
    
    
    say("Copy DreamGridUpdate.zip"); 
    
    if (!copy("../Zips/DreamGrid$type.zip",
            "$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update.zip"))
    {
        die $!;
    }
    
    if (!copy("../Zips/DreamGrid$type.zip",
            "$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update.zip"))
    {
        die $!;
    }
    
    
    ################################################
    
    say("Copy DropBox"); 
    
    if (
        !copy(
            "../Zips/DreamGrid$type.zip",
            $Dest
        )
     )
    {
        die $!;
    }

    ################################################
    
    
    say "Manual";

    if (!copy('outworldzfiles/Help/Dreamgrid Manual.pdf',
            "$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Dreamgrid Manual.pdf"))
    {
        die $!;
    }
    
    if (!copy('outworldzfiles/Help/Dreamgrid Manual.pdf',
            "$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/Dreamgrid Manual.pdf"))
    {
        die $!;
    }
    
    
    $v > io("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Grid/Version.txt");
    $v > io("$Fleta/Inetpub/Secondlife/Outworldz_Installer/Grid/Version.txt");

}


foreach my $lang (@languages) {
    JustDelete($lang);
}

say $start . "\n";
print GetDate() . " " . GetTime() . "\n";
say "Done!";


sub PrintDate {
    my $msg = shift;
    say (dt() . $msg);
}

sub dt {
    return GetDate() . " " . GetTime() . ' ';
}

sub Write {
    my $dest    = shift;
    my $content = shift;
    open OUT, ">$dest" || die $!;
    print OUT $content;
    close OUT;
}

sub ProcessFile {
    my $file = shift;

    my $x = `xcopy $file ..\\Zip\\`;
    $x =~ s/\n//g;
    if ( $x =~ /File\(s\) copied/ ) {
        say "$file ok\n";
    }
    else {
        say "$file Fail: $x\n";
        exit;
    }

}

sub ProcessDir {
    my $file = shift;
    return if $file =~ /\.rtf$/;

    my $x =`xcopy /E /I /C \\Opensim\\Outworldz_Dreamgrid\\$file  \\Opensim\\zip\\$file`;
    my $y = $x;
    $x =~ s/\n//g;
    if ( $x =~ /(\d+) File\(s\) copied/ ) {
        say "$y\n";
        if ( $1 == 0 ) {
            PrintDate( $file . " failed to copy\n $y" );
            die;
        }
    }
    else {
        say "$file Fail: $y\n";
        exit;
    }

}

sub rm {

    my $path = shift;

    my $errors;
    while ( $_ = glob("'$path/*'") ) {
        rmtree($_)
          or ++$errors, warn("Can't remove $_: $!");
    }

    #exit(1) if $errors;
}

sub JustDelete {

    my $path = shift;
    if ( !-d $path ) {
        doUnlink ($path);
        return;
    }

    use File::Path;
    rmtree $path;
}

sub DeleteandKeep {

    my $path = shift;
    use File::Path;
    rmtree $path || die;;
    while ( -e $path ) {
        rmtree $path;
        say "Directory '$path' still exists\n";
        sleep(1);
    }

    mkdir $path;
    open( FILE, '>', $path . '/.keep' ) or die;
    print FILE
"git will not save empty folders unless there is a file in it. This is a placeholder only\n";
    close FILE;

}

# for importers
sub Perlunzip {

    use Archive::Zip qw(:ERROR_CODES :CONSTANTS);
    use Exporter 'import';

    my ( $zip_file, $out_file, $filter ) = @_;
    $zip_file = $dir . '/Outworldzfiles/' . $zip_file;
    $out_file = $dir . '/Outworldzfiles/' . $out_file;

    my $zip = Archive::Zip->new($zip_file);
    unless ( $zip->extractTree( $filter || '', $out_file ) == AZ_OK ) {
        warn "unzip not successful: $!\n";
    }
}

sub DelZips {
    while ( $_ = glob("$dir/*.zip") ) {
        doUnlink($_) or die("Can't remove $_: $!");
    }
}

sub DelMaps {
    while ( $_ = glob("$dir/Outworldzfiles/opensim/bin/MAP-*.png") ) {
        doUnlink($_) or die("Can't remove $_: $!");
    }
}

sub sign {

    my @files = io->dir(shift)->all(0);

    my @signs;
    foreach my $file (@files) {
        my $name = $file->name;
        next
          if $name =~
/Microsoft|Debug|\.git|baretail|Downloader|Bouncy|Google|Tuple|packages/;

        if ( $name =~ /Start\.exe/i ) {
            my $bp = 1;
        }

        if ( $name =~ /dll$|exe$/ ) {

            my $r = qq!../Certs/sigcheck64.exe "$name"!;
            say $r. "\n";
            my $result1 = `$r`;

            say $result1;

            if ( $result1 !~ /Unsigned/ ) {
                next;
            }
            $result1 =~ s/\n//g;
            if ( $result1 =~ /Verified(.*)/i ) {
                push( @signs, $name );
            }

            my $f =
qq!../Certs/digicertutil.exe sign /noInput /sha1 $thumbprint "$name"!;
            say $f . "\n";
            my $result = `$f`;
            say $result. "\n";
            $result =~ s/\n/|/g;

            $r = qq!../Certs/sigcheck64.exe "$name"!;
            say $r. "\n";
            $result1 = `$r`;

            say $result1;

            if ( $result1 !~ /Verified:	Signed/ ) {
                PrintDate("***** Failed to sign!");
                die;
            }

            if ( $result !~ /success/ ) {
                PrintDate("***** Failed to sign!");
                die;
            }
        }
    }

    PrintDate( join( "\n", @signs ) );
}

sub process_file {

    # if (-f $_) {
    # print "This is a file: $_\n";
    #} else {
    # print "This is not file: $_\n";
    # }
    if ( $_ !~ /dll$/ ) {
        return;
    }
    my $fullpath = $_;
    $fullpath =~ s/$dir//g;
    $fullpath =~ s/\//\\/g;

    print OUT $fullpath . "\n";
}

sub GetDate {
    use DateTime;
    return DateTime->now->set_time_zone('America/Chicago')->ymd('/');
}

sub GetTime {
    use DateTime;
    return DateTime->now->set_time_zone('America/Chicago')->hms(':');
}

sub doUnlink {
    
    my $file = shift;
    if (-e $file) {
        unlink $file || die "Cannot unlink $file";
    }
    
    if (-e $file) {
        die ;
    }
}


sub CheckDistro
{
   
    if (! -d $Contabo) {
        die 'Cannot reach Contabo2';
    }
   
    if (! -d $Fleta) {
        die 'Cannot reach Fleta';
    }
}


sub CopyManuals
{
    
    PrintDate('Copy Manuals');
        
    my @manuals = io->dir($dir . '/OutworldzFiles/Help/')->all;
    
    foreach my $src (@manuals) {
        
        if ($src !~ /\.htm$/) {next};
        
        say($src);
        my @data = io->file($src)->slurp;
        my $output;
        my $ctr = 0 ;
        foreach my $l (@data)
        {
            
            if ($l !~ /liquidscript/) {
            
                if ($l =~ /<\/head>/i)
                {
                    $l = '<!--#include virtual="/cgi/scripts.plx?ID=liquidscript" --></head><!--#include virtual="/cgi/scripts.plx?ID=liquidmenu" -->';
                }
                if ($l =~ /<\/body>/i)
                {
                    $l = '<!--#include virtual="/cgi/scripts.plx?ID=liquidfooter" --></body>';
                }
            } else
            {
                say "skipped";
            }
            $output .= $l;
        }
        
        my $filename = basename($src);
        $output > io("$Contabo/Inetpub/Secondlife/Outworldz_Installer/Help/$filename");
        $output > io("$Fleta/Inetpub/Secondlife/Outworldz_Installer/Help/$filename");
        
    }
}

sub GetVersion
{    
    my $s1 = shift;
    my $contents = io->file($s1)->slurp;
    $contents =~ s/\n//;
    $contents =~ /_MyVersion As String = "(.*?)"/;
    my $ver = $1;
    if ( !$ver ) {
        PrintDate('no version!');
        exit;
    }
    return $ver   ;
}



sub delPDB
{
    my @pdb = io(shift)->all;

    foreach my $file (@pdb)
    {
        say ($file->name);
        if (-d $file->name )
        {
            delPDB($file->name);            
        }
        
        if ($file->name =~ /\.pdb$/i)
        {
            unlink $file->name;
        }
    }
}
