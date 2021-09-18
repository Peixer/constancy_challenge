namespace Shared.Pairs

open System

[<CLIMutable>]
type Pair =
    { id: int
      name: string
      idProvider: int
      status: int
      transactionFee: float
      created: DateTime
      deleted: DateTime }

module Validation =
    let validate v =
        let validators =
            [ fun u ->
                if String.IsNullOrEmpty u.name then
                    Some("name", "Name shouldn't be empty")
                else
                    None

              fun u ->
                  if 0.0 = u.transactionFee then
                      Some("transactionFee", "TransactionFee shouldn't be empty")
                  else
                      None

              fun u ->
                  if 0 = u.status then
                      Some("status", "Status shouldn't be empty")
                  else
                      None ]

        validators
        |> List.fold
            (fun acc e ->
                match e v with
                | Some (k, v) -> Map.add k v acc
                | None -> acc)
            Map.empty
