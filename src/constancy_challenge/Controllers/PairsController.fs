namespace Controllers

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module PairsControllers =

    let indexAction (ctx: HttpContext) (idProvider: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Pairs.Database.getAll cnf.connectionString idProvider

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) (idProvider: string) =
        task {
            let! input = Controller.getModel<Shared.Pairs.Pair> ctx
            let validateResult = Shared.Pairs.Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Shared.Pairs.Database.insert cnf.connectionString input idProvider

                match result with
                | Ok _ -> return "Sucess" :> obj
                | Error ex -> return raise ex
            else
                return Shared.Validation.Validate.formatResult validateResult :> obj

        }

    let deleteAction ctx (idProvider: string) id =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Shared.Pairs.Database.delete cnf.connectionString idProvider id

            match result with
            | Ok _ -> return result
            | Error ex -> return raise ex
        }

    let resource idProvider =
        controller {
            index (fun ctx -> indexAction ctx idProvider)
            create (fun ctx -> createAction ctx idProvider)
            delete (fun ctx -> deleteAction ctx idProvider)
        }
