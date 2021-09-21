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
                    "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders WHERE id = @id"
                    (Some <| dict [ "id" => Guid.Parse(id.ToString()) ])
        }

    let insert connectionString v : Task<Result<HistoryOrder option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.NewGuid()
                  idUser = v.idUser
                  idPair = v.idPair
                  quantity = v.quantity
                  price = v.price
                  side = v.side
                  created = DateTime.Now }

            let! insert =
                    execute
                        connection
                        "INSERT INTO HistoryOrders(id, idUser, idPair, quantity, price, side, created) VALUES (@id, @idUser, @idPair, @quantity, @price, @side, @created)"
                        value
            
            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders WHERE id=@id" v
        }

    let getAllByIdUser connectionString idUser : Task<Result<HistoryOrder seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            let v = (Some <| dict [ "idUser" => idUser ])

            return!
                query
                    connection
                    "SELECT id, idUser, idPair, quantity, price, side, created FROM HistoryOrders WHERE idUser=@idUser"
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