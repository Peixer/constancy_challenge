namespace Shared.Users

open System
open Shared.Database
open System.Threading.Tasks
open Npgsql
open FSharp.Control.Tasks.ContextInsensitive

module Database =
    let getAll connectionString : Task<Result<User seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, name, created, deleted FROM Users WHERE deleted is null" None
        }

    let update connectionString v (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = int id
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE Users SET name = @name WHERE id=@id" value
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = 0
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "INSERT INTO Users(name, created) VALUES ( @name, @created)" value
        }

    let delete connectionString (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = id |> int
                  name = ""
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE Users SET deleted = @deleted WHERE id = @id" value
        }
