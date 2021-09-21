namespace Core.UserWallets

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAllByIdUser connectionString idUser : Task<Result<UserWallet seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            let v = (Some <| dict [ "idUser" => idUser ])

            return!
                query
                    connection
                    "SELECT id, idUser, idPair, amount, created FROM UserWallets WHERE deleted is null and idUser=@idUser"
                    v
        }

    let update connectionString v (id: string) : Task<Result<UserWallet option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.Parse id
                  idUser = Guid.Empty
                  idPair = v.idPair
                  amount = v.amount
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! update =  execute connection "UPDATE UserWallets SET idPair = @idPair, amount = @amount WHERE id = @id" value
        
            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, idUser, idPair, amount, created FROM UserWallets WHERE id=@id" v
        }

    let insert connectionString v (idUser: string) : Task<Result<UserWallet option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.NewGuid()
                  idUser = Guid.Parse idUser
                  idPair = v.idPair
                  amount = v.amount
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! insert =
                    execute
                        connection
                        "INSERT INTO UserWallets(id, idUser, idPair, amount, created) VALUES (@id, @idUser, @idPair, @amount, @created)"
                        value

            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, idUser, idPair, amount, created FROM UserWallets WHERE id=@id" v
        }

    let delete connectionString id : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection "DELETE FROM UserWallets WHERE id=@id" (dict [ "id" => id ])
        }
