namespace Shared.Pairs

open Shared.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Shared.Pairs.Pair seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                query connection "SELECT id, name, idProvider, status, transactionFee, created, deleted FROM Pairs" None
        }

    let getById connectionString id : Task<Result<Shared.Pairs.Pair option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, name, idProvider, status, transactionFee, created, deleted FROM Pairs WHERE id=@id"
                    (Some <| dict [ "id" => id ])
        }

    let update connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "UPDATE Pairs SET id = @id, name = @name, idProvider = @idProvider, status = @status, transactionFee = @transactionFee, created = @created, deleted = @deleted WHERE id=@id"
                    v
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "INSERT INTO Pairs(id, name, idProvider, status, transactionFee, created, deleted) VALUES (@id, @name, @idProvider, @status, @transactionFee, @created, @deleted)"
                    v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM Pairs WHERE id=@id" (dict [ "id" => id ])
        }
