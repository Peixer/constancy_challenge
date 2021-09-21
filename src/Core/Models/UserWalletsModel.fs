namespace Core.UserWallets

open System

[<CLIMutable>]
type UserWallet =
    { id: Guid
      idUser: Guid
      idPair: Guid
      amount: float
      created: DateTime
      deleted: DateTime }

module Validation =
    let validate v =
        let validators =
            [ fun u ->
                if Guid.Empty = u.idPair then
                    Some("idPair", "IdPair shouldn't be empty")
                else
                    None

              fun u ->
                  if 0.0 = u.amount then
                      Some("amount", "Amount shouldn't be empty")
                  else
                      None ]

        validators
        |> List.fold
            (fun acc e ->
                match e v with
                | Some (k, v) -> Map.add k v acc
                | None -> acc)
            Map.empty
