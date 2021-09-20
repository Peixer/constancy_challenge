namespace Core.Pairs

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString idProvider : Task<Result<Pair seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let v =
                (Some <| dict [ "idProvider" => idProvider ])

            return!
                query
                    connection
                    "SELECT id, name, idProvider, status, transactionFee, created, deleted FROM Pairs WHERE deleted is null and idProvider=@idProvider::integer"
                    v
        }

    let insert connectionString v (idProvider: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = 0
                  name = v.name
                  idProvider = int idProvider
                  status = v.status
                  transactionFee = v.transactionFee
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return!
                execute
                    connection
                    "INSERT INTO Pairs(name, idProvider, status, transactionFee, created) VALUES (@name, @idProvider, @status, @transactionFee, @created)"
                    value
        }

    let delete connectionString (idProvider:string) id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = id |> int
                  name = ""
                  idProvider = idProvider |> int
                  status = 0
                  transactionFee = 0.0
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return!
                execute
                    connection
                    "UPDATE Pairs SET deleted = @deleted WHERE id = @id::integer and idProvider = @idProvider::integer"
                    value
        }
