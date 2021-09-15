namespace Migrations
open SimpleMigrations

[<Migration(202109150333L, "Create Users")>]
type CreateUsers() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Users(
      id TEXT NOT NULL,
      name TEXT NOT NULL,
      created DATETIME NOT NULL,
      deleted DATETIME NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Users")
