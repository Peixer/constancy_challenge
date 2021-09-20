namespace Core.HistoryOrders

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<HistoryOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders" None
        }

    let getById connectionString id : Task<Result<HistoryOrder option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders WHERE id = @id::integer"
                    (Some <| dict [ "id" => id ])
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
                  side = v.side
                  created = DateTime.Now }

            return!
                execute
                    connection
                    "INSERT INTO HistoryOrders(idUser, idPair, quantity, price, side, created) VALUES (@idUser, @idPair, @quantity, @price, @side, @created)"
                    value
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

    let getAllByIdProvider connectionString idProvider : Task<Result<HistoryOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let v =
                (Some <| dict [ "idProvider" => idProvider ])

            return!
                query
                    connection
                    @"SELECT h.id, h.idUser, h.idPair, h.quantity, h.price, h.side, h.created FROM HistoryOrders h
                    LEFT JOIN Pairs p ON h.idPair = p.id
                    LEFT JOIN Providers pr ON p.idProvider = pr.id"
                    v
        }