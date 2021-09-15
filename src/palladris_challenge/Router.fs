module Router

open Saturn
open Giraffe.ResponseWriters



 let api = pipeline {
     plug acceptJson
     set_header "x-pipeline-type" "Api"
 }

 let apiRouter = router {
     not_found_handler (text "Api 404")
     pipe_through api

     forward "/users" Users.Controller.resource
 }

let appRouter = router {
    forward "/api" apiRouter
}