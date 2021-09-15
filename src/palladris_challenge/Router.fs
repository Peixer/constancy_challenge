module Router

open Saturn




let api = pipeline { plug acceptJson }


let apiRouter =
    router {
        pipe_through api

        forward "/users" Users.Controller.resource
        forward "/providers" Providers.Controller.resource
    }


let appRouter = router { forward "/api" apiRouter }