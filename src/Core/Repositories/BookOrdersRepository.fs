namespace Core.BookOrders

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<BookOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                query
                    connection
                    "SELECT id, idUser, idPair, quantity, price, status, side, created, deleted FROM BookOrders WHERE deleted is null"
                    None
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = 0
                  idUser = v.idUser
                  idPair = v.idPair
                  quantity = v.quantity
                  price = v.price
                  status = 0
                  side = v.side
                  created = DateTime.Now
                  deleted = DateTime.Now  }
                
            return!
                execute
                    connection
                    "INSERT INTO BookOrders(idUser, idPair, quantity, price, status, side, created) VALUES (@idUser, @idPair, @quantity, @price, @status, @side, @created)"
                    value
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM BookOrders WHERE id = @id::integer" (dict [ "id" => id ])
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
