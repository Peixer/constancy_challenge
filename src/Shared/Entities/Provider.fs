namespace Shared.Entities

open System

module Exchange =
    type Status =
        | ONLINE = 0
        | OFFLINE = 1

    type Provider =
        { Id: Guid
          Name: string
          Created: DateTime
          Deleted: DateTime }

    type Pair =
        { Id: Guid
          IdProvider: Guid
          Name: string
          Status: Status
          TransactionFee: float
          Created: DateTime
          Deleted: DateTime }
