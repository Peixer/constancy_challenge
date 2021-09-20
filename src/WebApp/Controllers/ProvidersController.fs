namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module ProvidersController =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.Providers.Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Core.Providers.Provider> ctx

            let validateResult =
                Core.Providers.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Core.Providers.Database.insert cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                return Core.Validation.Validate.formatResult validateResult :> obj
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<Core.Providers.Provider> ctx

            let validateResult =
                Core.Providers.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Core.Providers.Database.update cnf.connectionString input id

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                return Core.Validation.Validate.formatResult validateResult :> obj
        }

    let deleteAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.Providers.Database.delete cnf.connectionString id

            match result with
            | Ok _ -> return result
            | Error ex -> return raise ex
        }



    let getOrders (ctx: HttpContext) (idProvider: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.BookOrders.Database.getAllByIdProvider cnf.connectionString idProvider

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }


    let getHistories (ctx: HttpContext) (idProvider: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Core.HistoryOrders.Database.getAllByIdProvider cnf.connectionString idProvider

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }
        
    let ordersController idProvider =
        controller { index (fun ctx -> getOrders ctx idProvider) }

    let historiesController idProvider =
        controller { index (fun ctx -> getHistories ctx idProvider) }

    let resource =
        controller {

            subController "/pairs" PairsControllers.resource
            subController "/orders" ordersController
            subController "/histories" historiesController

            index indexAction
            create createAction
            update updateAction
            delete deleteAction

        }
