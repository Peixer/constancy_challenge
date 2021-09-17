namespace Migrations
open SimpleMigrations

[<Migration(3L, "Create Pairs")>]
type CreatePairs() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Pairs(
      id SERIAL NOT NULL PRIMARY KEY,
      name TEXT NOT NULL,
      idProvider SERIAL NOT NULL,
      status integer NOT NULL,
      transactionFee real NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Pairs")
