namespace Migrations
open SimpleMigrations

[<Migration(202109161051L, "Create UserWallets")>]
type CreateUserWallets() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE UserWallets(
      id SERIAL NOT NULL PRIMARY KEY,
      idUser SERIAL NOT NULL,
      idPair SERIAL NOT NULL,
      amount real NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp
    )")
               
  override __.Down() =
    base.Execute(@"DROP TABLE UserWallets")
