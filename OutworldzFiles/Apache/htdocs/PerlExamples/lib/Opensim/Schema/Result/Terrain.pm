use utf8;
package Opensim::Schema::Result::Terrain;

# Created by DBIx::Class::Schema::Loader
# DO NOT MODIFY THE FIRST PART OF THIS FILE

=head1 NAME

Opensim::Schema::Result::Terrain

=cut

use strict;
use warnings;

use base 'DBIx::Class::Core';

=head1 TABLE: C<terrain>

=cut

__PACKAGE__->table("terrain");

=head1 ACCESSORS

=head2 regionuuid

  data_type: 'varchar'
  is_nullable: 1
  size: 255

=head2 revision

  data_type: 'integer'
  is_nullable: 1

=head2 heightfield

  data_type: 'longblob'
  is_nullable: 1

=cut

__PACKAGE__->add_columns(
  "regionuuid",
  { data_type => "varchar", is_nullable => 1, size => 255 },
  "revision",
  { data_type => "integer", is_nullable => 1 },
  "heightfield",
  { data_type => "longblob", is_nullable => 1 },
);


# Created by DBIx::Class::Schema::Loader v0.07042 @ 2014-10-14 12:09:34
# DO NOT MODIFY THIS OR ANYTHING ABOVE! md5sum:2dVOpXwUJxbEzZtLQAE5wQ


# You can replace this text with custom code or comments, and it will be preserved on regeneration
1;
