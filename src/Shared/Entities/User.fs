namespace Shared.Entities

open System

module User =

    type User =
        { Id: Guid
          Name: string
          Created: DateTime
          Deleted: DateTime }

    type UserWallet =
        { Id: Guid
          IdPair: Guid
          IdUser: Guid
          Amount: float
          Created: DateTime
          Deleted: DateTime }