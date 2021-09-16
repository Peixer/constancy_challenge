namespace Shared.UserWallets

open Shared.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Shared.UserWallets.UserWallet seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, idUser, idPair, amount, created, deleted FROM UserWallets" None
        }

    let getById connectionString id : Task<Result<Shared.UserWallets.UserWallet option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, idUser, idPair, amount, created, deleted FROM UserWallets WHERE id=@id"
                    (Some <| dict [ "id" => id ])
        }

    let update connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "UPDATE UserWallets SET id = @id, idUser = @idUser, idPair = @idPair, amount = @amount, created = @created, deleted = @deleted WHERE id=@id"
                    v
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "INSERT INTO UserWallets(id, idUser, idPair, amount, created, deleted) VALUES (@id, @idUser, @idPair, @amount, @created, @deleted)"
                    v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM UserWallets WHERE id=@id" (dict [ "id" => id ])
        }
