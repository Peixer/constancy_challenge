namespace Shared.UserWallets

[<CLIMutable>]
type UserWallet =
    { id: int
      idUser: int
      idPair: int
      amount: float
      created: System.DateTime
      deleted: System.DateTime }

module Validation =
    let validate v =
        let validators =
            [ fun u ->
                if 0 = u.idPair then
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
