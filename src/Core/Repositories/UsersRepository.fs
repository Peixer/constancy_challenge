namespace Core.Users

open System
open Core.Database
open System.Threading.Tasks
open Npgsql
open FSharp.Control.Tasks.ContextInsensitive

module Database =
    let getAll connectionString : Task<Result<User seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, name, created FROM Users WHERE deleted is null" None
        }

    let update connectionString v (id: string) : Task<Result<User option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.Parse id
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! update = execute connection "UPDATE Users SET name = @name WHERE id=@id" value

            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, name, created FROM Users WHERE id=@id" v
        }

    let insert connectionString v : Task<Result<User option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.NewGuid()
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! insert = execute connection "INSERT INTO Users(id, name, created) VALUES (@id, @name, @created)" value

            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, name, created FROM Users WHERE id=@id" v
        }

    let delete connectionString (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.Parse id
                  name = ""
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE Users SET deleted = @deleted WHERE id = @id" value
        }
