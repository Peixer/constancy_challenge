namespace Core.Providers

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Provider seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, name, created FROM Providers WHERE deleted is null" None
        }

    let update connectionString v (id: string) : Task<Result<Provider option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.Parse id
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! update = execute connection "UPDATE Providers SET name = @name WHERE id=@id" value
                
            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, name, created FROM Providers WHERE id=@id" v
        }

    let insert connectionString v : Task<Result<Provider option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.NewGuid()
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            let! insert = execute connection "INSERT INTO Providers(id, name, created) VALUES (@id, @name, @created)" value
        
            let v = (Some <| dict [ "id" => value.id ])
            use connection = new NpgsqlConnection(connectionString)
            return! querySingle connection "SELECT id, name, created FROM Providers WHERE id=@id" v
        }

    let delete connectionString (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = Guid.Parse id
                  name = ""
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE Providers SET deleted = @deleted WHERE id = @id" value
        }
