namespace Core.BookOrders

[<CLIMutable>]
type BookOrder =
    { id: int
      idUser: int
      idPair: int
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
                if 0 = u.idUser then
                    Some("idUser", "IdUser shouldn't be empty")
                else
                    None

              fun u ->
                  if 0 = u.idPair then
                      Some("idPair", "IdPair shouldn't be empty")
                  else
                      None
              fun u ->
                  if 0.0 = u.quantity then
                      Some("quantity", "Quantity shouldn't be empty")
                  else
                      None

              fun u ->
                  if 0.0 = u.price then
                      Some("price", "Price shouldn't be empty")
                  else
                      None

              fun u ->
                  if 0 = u.side then
                      Some("side", "Side shouldn't be empty")
                  else
                      None ]

        validators
        |> List.fold
            (fun acc e ->
                match e v with
                | Some (k, v) -> Map.add k v acc
                | None -> acc)
            Map.empty
