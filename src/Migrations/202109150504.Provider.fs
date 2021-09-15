namespace Migrations
open SimpleMigrations

[<Migration(2L, "Create Providers")>]
type CreateProviders() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Providers(
      id TEXT NOT NULL,
      name TEXT NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Providers")
