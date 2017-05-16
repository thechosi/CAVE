use strict;
use IO::Socket;

my $host = shift || $ARGV[0];
my $port = 1025;
my $name = shift || $ARGV[1];
my $buf;

# Socket konstruieren und initialisieren
my $sock = new IO::Socket::INET(PeerAddr => $host, PeerPort => $port, Proto => 'tcp');
$sock || die "no socket :$!";

print $sock "start START" + $name + "\n";
read ($sock, $buf, 10);
printf $buf;

# Socket schlieﬂen
close $sock;

