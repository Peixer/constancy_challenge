namespace Migrations
open SimpleMigrations

[<Migration(3L, "Create Pairs")>]
type CreatePairs() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Pairs(
      id TEXT NOT NULL,
      name TEXT NOT NULL,
      idProvider TEXT NOT NULL,
      status integer NOT NULL,
      transactionFee real NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Pairs")
