namespace Core.Users

open System

[<CLIMutable>]
type User =
    { id: Guid
      name: string
      created: DateTime
      deleted: DateTime }

module Validation =
    let validate v =
        let validators =
            [ fun u ->
                  if String.IsNullOrEmpty u.name then
                      Some("name", "Name shouldn't be empty")
                  else
                      None ]

        validators
        |> List.fold
            (fun acc e ->
                match e v with
                | Some (k, v) -> Map.add k v acc
                | None -> acc)
            Map.empty