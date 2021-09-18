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
            [ fun u -> if 0 = u.id then None else None ]

        validators
        |> List.fold
            (fun acc e ->
                match e v with
                | Some (k, v) -> Map.add k v acc
                | None -> acc)
            Map.empty
