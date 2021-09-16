module Router

open Controllers
open Saturn
let api = pipeline { plug acceptJson }

let apiRouter =
    router {
        pipe_through api

        forward "/users" UsersController.resource
        forward "/providers" ProvidersController.resource
    }

let appRouter = router { forward "/api" apiRouter }