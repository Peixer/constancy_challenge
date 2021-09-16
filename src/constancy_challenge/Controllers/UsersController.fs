namespace Controllers

open Microsoft.AspNetCore.Http
open Config
open Saturn
open FSharp.Control.Tasks
open FSharp.Json

module UsersController =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Users.Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let showAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Users.Database.getById cnf.connectionString id

            match result with
            | Ok (Some result) -> return result
            | Ok None -> return! null
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Shared.Users.User> ctx
            let validateResult = Shared.Users.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Shared.Users.Database.insert cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess"
                | Error ex -> return raise ex
            else
                return Json.serialize validateResult
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<Shared.Users.User> ctx
            let validateResult = Shared.Users.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Shared.Users.Database.update cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess"
                | Error ex -> return raise ex
            else
                return Json.serialize validateResult
        }

    let deleteAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Users.Database.delete cnf.connectionString id

            match result with
            | Ok _ -> return result
            | Error ex -> return raise ex
        }

    let resource =
        controller {
            subController "/wallets" UserWalletsController.resource
            subController "/orders" BookOrdersController.resource
            subController "/histories" HistoryOrdersController.resource   
            
            index indexAction
            show showAction
            create createAction
            update updateAction
            delete deleteAction
        }
