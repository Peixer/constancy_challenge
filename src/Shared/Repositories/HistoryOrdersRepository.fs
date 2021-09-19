namespace Shared.HistoryOrders

open Shared.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Shared.HistoryOrders.HistoryOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders" None
        }

    let getById connectionString id : Task<Result<Shared.HistoryOrders.HistoryOrder option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders WHERE id=@id"
                    (Some <| dict [ "id" => id ])
        }

    let update connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "UPDATE HistoryOrders SET id = @id, idUser = @idUser, idPair = @idPair, quantity = @quantity, price = @price, side = @side, created = @created WHERE id=@id"
                    v
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "INSERT INTO HistoryOrders(id, idUser, idPair, quantity, price, side, created) VALUES (@id, @idUser, @idPair, @quantity, @price, @side, @created)"
                    v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM HistoryOrders WHERE id=@id" (dict [ "id" => id ])
        }

    let getAllByIdUser connectionString idUser : Task<Result<HistoryOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            let v = (Some <| dict [ "idUser" => idUser ])

            return!
                query
                    connection
                    "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders WHERE idUser=@idUser::integer"
                    v
        }