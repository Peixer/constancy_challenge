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
                (Some <| dict [ "idProvider" => Guid.Parse(idProvider.ToString()) ])

            return!
                query
                    connection
                    "SELECT id, name, idProvider, status, transactionFee, created FROM Pairs WHERE deleted is null and idProvider=@idProvider"
                    v
        }

    let insert connectionString v (idProvider: string) : Task<Result<Pair option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.NewGuid()
                  name = v.name
                  idProvider = Guid.Parse idProvider
                  status = v.status
                  transactionFee = v.transactionFee
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! insert =
                execute
                    connection
                    "INSERT INTO Pairs(id, name, idProvider, status, transactionFee, created) VALUES (@id, @name, @idProvider, @status, @transactionFee, @created)"
                    value

            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, name, idProvider, status, transactionFee, created FROM Pairs WHERE id = @id" v

        }

    let delete connectionString (idProvider: string) id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.Parse(id.ToString())
                  name = ""
                  idProvider = Guid.Parse idProvider
                  status = 0
                  transactionFee = 0.0
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return!
                execute
                    connection
                    "UPDATE Pairs SET deleted = @deleted WHERE id = @id and idProvider = @idProvider"
                    value
        }
