module Core.Validation.Validate



let formatResult result =
    let validateResultFormatted =
        result
        |> Map.toSeq
        |> Seq.collect (fun (key, value) -> [ (key + ": " + value :> obj) ])
        |> List.ofSeq

    let error =
        {| data = validateResultFormatted
           code = 422 |}

    error