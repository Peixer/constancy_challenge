namespace Migrations
open SimpleMigrations

[<Migration(3L, "Create Pairs")>]
type CreatePairs() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Pairs(
      id UUID DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
      name TEXT NOT NULL,
      idProvider UUID NOT NULL,
      status integer NOT NULL,
      transactionFee real NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Pairs")
