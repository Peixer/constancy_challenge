namespace Shared.Entities

open System

module Order =
    type Status =
        | OPEN = 0
        | CLOSE = 1
        | CANCEL = 2

    type Side =
        | SELL = 0
        | BUY = 1

    type BookOrder =
        { Id: Guid
          IdPair: Guid
          IdUser: Guid
          Quantity: float
          Price: float
          Status: Status
          Side: Side
          Created: DateTime
          Deleted: DateTime }

    type HistoryOrder =
        { Id: Guid
          IdPair: Guid
          IdUser: Guid
          Quantity: float
          Side: Side
          Created: DateTime }