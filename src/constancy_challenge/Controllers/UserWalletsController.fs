namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module UserWalletsController =

    let indexAction ctx idUser =
        task {
            let cnf = Controller.getConfig ctx

            let! result = Shared.UserWallets.Database.getAllByIdUser cnf.connectionString idUser

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) (idUser: string) =
        task {
            let! input = Controller.getModel<Shared.UserWallets.UserWallet> ctx

            let validateResult =
                Shared.UserWallets.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Shared.UserWallets.Database.insert cnf.connectionString input idUser

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                let validateResultFormatted =
                    validateResult
                    |> Map.toSeq
                    |> Seq.collect (fun (key, value) -> [ (key + ": " + value :> obj) ])
                    |> List.ofSeq

                let error =
                    {| data = validateResultFormatted
                       code = 422 |}

                return error :> obj
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<Shared.UserWallets.UserWallet> ctx

            let validateResult =
                Shared.UserWallets.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Shared.UserWallets.Database.update cnf.connectionString input id

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                let validateResultFormatted =
                    validateResult
                    |> Map.toSeq
                    |> Seq.collect (fun (key, value) -> [ (key + ": " + value :> obj) ])
                    |> List.ofSeq

                let error =
                    {| data = validateResultFormatted
                       code = 422 |}

                return error :> obj
        }

    let resource idUser =
        controller {
            index (fun ctx -> indexAction ctx idUser)
            create (fun ctx -> createAction ctx idUser)
            update (fun ctx -> updateAction ctx)
        }
