namespace BookOrders

[<CLIMutable>]
type BookOrder =
    { id: string
      idUser: string
      idPair: string
      quantity: float
      price: float
      status: int
      side: int
      created: System.DateTime
      deleted: System.DateTime }

module Validation =
    let validate v =
        let validators =
            [ fun u ->
                  if isNull u.id then
                      Some("id", "Id shouldn't be empty")
                  else
                      None ]

        validators
        |> List.fold
            (fun acc e ->
                match e v with
                | Some (k, v) -> Map.add k v acc
                | None -> acc)
            Map.empty
