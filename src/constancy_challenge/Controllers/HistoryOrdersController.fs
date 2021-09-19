namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module HistoryOrdersController =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.HistoryOrders.Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let showAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.HistoryOrders.Database.getById cnf.connectionString id

            match result with
            | Ok (Some result) -> return result
            | Ok None -> return! null
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Shared.HistoryOrders.HistoryOrder> ctx

            let validateResult =
                Shared.HistoryOrders.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Shared.HistoryOrders.Database.insert cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                return Shared.Validation.Validate.formatResult validateResult :> obj
        }

    let resource =
        controller {
            index indexAction
            show showAction
            create createAction
        }
