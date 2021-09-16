namespace Providers

open Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Shared.Providers.Provider seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, name, created, deleted FROM Providers" None
        }

    let getById connectionString id : Task<Result<Shared.Providers.Provider option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, name, created, deleted FROM Providers WHERE id=@id"
                    (Some <| dict [ "id" => id ])
        }

    let update connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "UPDATE Providers SET id = @id, name = @name, created = @created, deleted = @deleted WHERE id=@id"
                    v
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "INSERT INTO Providers(id, name, created, deleted) VALUES (@id, @name, @created, @deleted)"
                    v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM Providers WHERE id=@id" (dict [ "id" => id ])
        }
