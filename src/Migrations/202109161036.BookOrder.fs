namespace Migrations
open SimpleMigrations

[<Migration(4L, "Create BookOrders")>]
type CreateBookOrders() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE BookOrders(
      id UUID DEFAULT gen_random_uuid()  NOT NULL PRIMARY KEY,
      idUser UUID NOT NULL,
      idPair UUID NOT NULL,
      quantity real NOT NULL,
      price real NOT NULL,
      status integer NOT NULL,
      side integer NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE BookOrders")
