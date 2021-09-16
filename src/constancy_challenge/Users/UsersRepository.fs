namespace Users

open Database
open System.Threading.Tasks
open Npgsql
open FSharp.Control.Tasks.ContextInsensitive

module Database =
    let getAll connectionString : Task<Result<User seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, name, created, deleted FROM Users" None
        }

    let getById connectionString id : Task<Result<User option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, name, created, deleted FROM Users WHERE id=@id"
                    (Some <| dict [ "id" => id ])
        }

    let update connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "UPDATE Users SET id = @id, name = @name, created = @created, deleted = @deleted WHERE id=@id"
                    v
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "INSERT INTO Users(id, name, created, deleted) VALUES (@id, @name, @created, @deleted)"
                    v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM Users WHERE id=@id" (dict [ "id" => id ])
        }
