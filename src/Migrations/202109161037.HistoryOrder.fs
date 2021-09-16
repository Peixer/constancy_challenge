namespace Migrations
open SimpleMigrations

[<Migration(5L, "Create HistoryOrders")>]
type CreateHistoryOrders() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE HistoryOrders(
      id TEXT NOT NULL,
      idUser TEXT NOT NULL,
      idPair TEXT NOT NULL,
      quantity real NOT NULL,
      price real NOT NULL,
      side integer NOT NULL,
      created timestamp NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE HistoryOrders")
