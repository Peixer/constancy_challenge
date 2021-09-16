namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn
open FSharp.Json

module UserWalletsController =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.UserWallets.Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let showAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.UserWallets.Database.getById cnf.connectionString id

            match result with
            | Ok (Some result) -> return result
            | Ok None -> return! null
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Shared.UserWallets.UserWallet> ctx
            let validateResult = Shared.UserWallets.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Shared.UserWallets.Database.insert cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess"
                | Error ex -> return raise ex
            else
                return Json.serialize validateResult
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<Shared.UserWallets.UserWallet> ctx
            let validateResult = Shared.UserWallets.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Shared.UserWallets.Database.update cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess"
                | Error ex -> return raise ex
            else
                return Json.serialize validateResult
        }

    let resource userdId =
        controller {
            index indexAction
            show showAction
            create createAction
            update updateAction
        }
