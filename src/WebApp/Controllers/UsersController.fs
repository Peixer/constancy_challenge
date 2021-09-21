namespace Controllers

open Microsoft.AspNetCore.Http
open Config
open Saturn
open FSharp.Control.Tasks

module UsersController =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.Users.Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Core.Users.User> ctx
            let validateResult = Core.Users.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Core.Users.Database.insert cnf.connectionString input

                match result with
                | Ok result -> return result.Value :> obj
                | Error ex -> return raise ex
            else
                return Core.Validation.Validate.formatResult validateResult :> obj
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<Core.Users.User> ctx
            let validateResult = Core.Users.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Core.Users.Database.update cnf.connectionString input id

                match result with
                | Ok result -> return result.Value :> obj
                | Error ex -> return raise ex
            else
                return Core.Validation.Validate.formatResult validateResult :> obj
        }

    let deleteAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.Users.Database.delete cnf.connectionString id

            match result with
            | Ok _ -> return result
            | Error ex -> return raise ex
        }


    let getOrders (ctx: HttpContext) (idUser: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.BookOrders.Database.getAllByIdUser cnf.connectionString idUser

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }


    let getHistories (ctx: HttpContext) (idUser: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.HistoryOrders.Database.getAllByIdUser cnf.connectionString idUser

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }
        
    let ordersController idUser =
        controller { index (fun ctx -> getOrders ctx idUser) }

    let historiesController idUser =
        controller { index (fun ctx -> getHistories ctx idUser) }

    let resource =
        controller {
            subController "/wallets" UserWalletsController.resource
            subController "/orders" ordersController
            subController "/histories" historiesController

            index indexAction
            create createAction
            update updateAction
            delete deleteAction
        }
