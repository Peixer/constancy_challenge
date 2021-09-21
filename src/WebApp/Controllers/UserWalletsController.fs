namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module UserWalletsController =

    let indexAction ctx idUser =
        task {
            let cnf = Controller.getConfig ctx

            let! result = Core.UserWallets.Database.getAllByIdUser cnf.connectionString idUser

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) (idUser) =
        task {
            let! input = Controller.getModel<Core.UserWallets.UserWallet> ctx

            let validateResult =
                Core.UserWallets.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Core.UserWallets.Database.insert cnf.connectionString input idUser

                match result with
                | Ok result -> return result.Value :> obj
                | Error ex -> return raise ex
            else
                return Core.Validation.Validate.formatResult validateResult :> obj
        }

    let updateAction (ctx: HttpContext) (id) =
        task {
            let! input = Controller.getModel<Core.UserWallets.UserWallet> ctx

            let validateResult =
                Core.UserWallets.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Core.UserWallets.Database.update cnf.connectionString input id

                match result with
                | Ok result -> return result.Value :> obj
                | Error ex -> return raise ex
            else
                return Core.Validation.Validate.formatResult validateResult :> obj
        }

    let resource idUser =
        controller {
            index (fun ctx -> indexAction ctx idUser)
            create (fun ctx -> createAction ctx idUser)
            update (fun ctx -> updateAction ctx)
        }
