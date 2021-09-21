namespace Migrations
open SimpleMigrations

[<Migration(2L, "Create Providers")>]
type CreateProviders() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Providers(
      id UUID DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
      name TEXT NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Providers")
