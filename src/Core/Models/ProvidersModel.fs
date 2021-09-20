namespace Core.Providers

open System

[<CLIMutable>]
type Provider =
    { id: int
      name: string
      created: System.DateTime
      deleted: System.DateTime }

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
