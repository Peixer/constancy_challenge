namespace Migrations
open SimpleMigrations

[<Migration(1L, "Create Users")>]
type CreateUsers() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Users(
      id SERIAL NOT NULL PRIMARY KEY,
      name TEXT NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Users")
