namespace Users

open Microsoft.AspNetCore.Http
open Config
open Saturn
open FSharp.Control.Tasks
open FSharp.Json

module Controller =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.getAll cnf.connectionString

            match result with
            | Ok result -> return result
            | Error ex -> return raise ex
        }

    let showAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.getById cnf.connectionString id

            match result with
            | Ok (Some result) -> return result
            | Ok None -> return! null
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<User> ctx
            let validateResult = Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Database.insert cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess"
                | Error ex -> return raise ex
            else
                return Json.serialize validateResult
        }

    let updateAction (ctx: HttpContext) (id: string) =
        task {
            let! input = Controller.getModel<User> ctx
            let validateResult = Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Database.update cnf.connectionString input

                match result with
                | Ok _ -> return "Sucess"
                | Error ex -> return raise ex
            else
                return Json.serialize validateResult
        }

    let deleteAction (ctx: HttpContext) (id: string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.delete cnf.connectionString id

            match result with
            | Ok _ -> return result
            | Error ex -> return raise ex
        }

    let resource =
        controller {
            index indexAction
            show showAction
            create createAction
            update updateAction
            delete deleteAction
        }
