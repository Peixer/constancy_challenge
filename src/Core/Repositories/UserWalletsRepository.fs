namespace Core.UserWallets

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAllByIdUser connectionString idUser : Task<Result<Core.UserWallets.UserWallet seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            let v = (Some <| dict [ "idUser" => idUser ])

            return!
                query
                    connection
                    "SELECT id, idUser, idPair, amount, created, deleted FROM UserWallets WHERE deleted is null and idUser=@idUser::integer"
                    v
        }

    let update connectionString v (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = int id
                  idUser = 0
                  idPair = v.idPair
                  amount = v.amount
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE UserWallets SET idPair = @idPair, amount = @amount WHERE id = @id" value
        }

    let insert connectionString v (idUser: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = 0
                  idUser = idUser |> int
                  idPair = v.idPair
                  amount = v.amount
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return!
                execute
                    connection
                    "INSERT INTO UserWallets(idUser, idPair, amount, created) VALUES (@idUser, @idPair, @amount, @created)"
                    value
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM UserWallets WHERE id=@id" (dict [ "id" => id ])
        }
