namespace Migrations
open SimpleMigrations

[<Migration(6L, "Create UserWallets")>]
type CreateUserWallets() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE UserWallets(
      id UUID DEFAULT gen_random_uuid()  NOT NULL PRIMARY KEY,
      idUser UUID NOT NULL,
      idPair UUID NOT NULL,
      amount real NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp
    )")
               
  override __.Down() =
    base.Execute(@"DROP TABLE UserWallets")
