namespace Shared.BookOrders

open System
open Shared.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Shared.BookOrders.BookOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                query
                    connection
                    "SELECT id, idUser, idPair, quantity, price, status, side, created, deleted FROM BookOrders"
                    None
        }

    let getById connectionString id : Task<Result<Shared.BookOrders.BookOrder option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, idUser, idPair, quantity, price, status, side, created, deleted FROM BookOrders WHERE id=@id"
                    (Some <| dict [ "id" => id ])
        }

    let update connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "UPDATE BookOrders SET id = @id, idUser = @idUser, idPair = @idPair, quantity = @quantity, price = @price, status = @status, side = @side, created = @created, deleted = @deleted WHERE id=@id"
                    v
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    "INSERT INTO BookOrders(id, idUser, idPair, quantity, price, status, side, created, deleted) VALUES (@id, @idUser, @idPair, @quantity, @price, @status, @side, @created, @deleted)"
                    v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM BookOrders WHERE id=@id" (dict [ "id" => id ])
        }

    let getAllByIdUser connectionString idUser : Task<Result<BookOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            let v = (Some <| dict [ "idUser" => idUser ])


            return!
                query
                    connection
                    "SELECT id, idUser, idPair, quantity, price, status, side, created, deleted FROM BookOrders WHERE deleted is null and idUser=@idUser::integer"
                    v
        }

    let getAllByIdProvider connectionString idProvider : Task<Result<BookOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let v =
                (Some <| dict [ "idProvider" => idProvider ])

            return!
                query
                    connection
                    @"SELECT b.id, b.idUser, b.idPair, b.quantity, b.price, b.status, b.side, b.created FROM BookOrders b
                    LEFT JOIN Pairs p ON b.idPair = p.id
                    LEFT JOIN Providers pr ON p.idProvider = pr.id
                    WHERE b.deleted is null"
                    v
        }
