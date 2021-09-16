module Router

open Saturn

let api = pipeline { plug acceptJson }

let apiRouter =
    router {
        pipe_through api

        forward "/users" Users.Controller.resource
        forward "/providers" Providers.Controller.resource
        forward "/pairs" Pairs.Controller.resource
        forward "/orders" BookOrders.Controller.resource
        forward "/histories" HistoryOrders.Controller.resource
        forward "/wallets" UserWallets.Controller.resource        
    }

let appRouter = router { forward "/api" apiRouter }