module Server

open Saturn
open Config

let endpointPipe =
    pipeline {
        plug head
        plug requestId
    }

let app =
    application {
        pipe_through endpointPipe

        use_router Router.appRouter
        url "http://0.0.0.0:8085/"
        memory_cache
        use_static "static"
        use_gzip

        use_config
            (fun _ ->
                { connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=WAhBRV2qHNA9c8yd744zH2w4;Database=postgres" })
    }

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code