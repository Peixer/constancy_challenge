namespace Migrations
open SimpleMigrations

[<Migration(202109161051L, "Create UserWallets")>]
type CreateUserWallets() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE UserWallets(
      id TEXT NOT NULL,
      idUser TEXT NOT NULL,
      idPair TEXT NOT NULL,
      amount real NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE UserWallets")
