namespace Core.Providers

open System
open Core.Database
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

module Database =
    let getAll connectionString : Task<Result<Core.Providers.Provider seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection "SELECT id, name, created, deleted FROM Providers WHERE deleted is null" None
        }

    let update connectionString v (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = id |> int
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE Providers SET  name = @name WHERE id=@id" value
        }

    let insert connectionString v : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = 0
                  name = v.name
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "INSERT INTO Providers(name, created) VALUES (@name, @created)" value
        }

    let delete connectionString (id: string) : Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            let value =
                { id = id |> int
                  name = ""
                  created = DateTime.Now
                  deleted = DateTime.Now }

            return! execute connection "UPDATE Providers SET deleted = @deleted WHERE id = @id" value
        }
