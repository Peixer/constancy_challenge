namespace Migrations
open SimpleMigrations

[<Migration(4L, "Create BookOrders")>]
type CreateBookOrders() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE BookOrders(
      id TEXT NOT NULL,
      idUser TEXT NOT NULL,
      idPair TEXT NOT NULL,
      quantity real NOT NULL,
      price real NOT NULL,
      status integer NOT NULL,
      side integer NOT NULL,
      created timestamp NOT NULL,
      deleted timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE BookOrders")
