namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module ProvidersController =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Providers.Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Shared.Providers.Provider> ctx

            let validateResult =
                Shared.Providers.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Shared.Providers.Database.insert cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                return Shared.Validation.Validate.formatResult validateResult :> obj
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<Shared.Providers.Provider> ctx

            let validateResult =
                Shared.Providers.Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Shared.Providers.Database.update cnf.connectionString input id

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                return Shared.Validation.Validate.formatResult validateResult :> obj
        }

    let deleteAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Providers.Database.delete cnf.connectionString id

            match result with
            | Ok _ -> return result
            | Error ex -> return raise ex
        }



    let getOrders (ctx: HttpContext) (idProvider: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.BookOrders.Database.getAllByIdProvider cnf.connectionString idProvider

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }


    let getHistories (ctx: HttpContext) (idProvider: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.HistoryOrders.Database.getAllByIdProvider cnf.connectionString idProvider

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
