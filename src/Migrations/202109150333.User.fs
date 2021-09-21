namespace Migrations
open SimpleMigrations

[<Migration(1L, "Create Users")>]
type CreateUsers() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Users(
      id UUID DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
      name TEXT NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Users")
