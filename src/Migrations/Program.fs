module Program

open System.Reflection
open SimpleMigrations
open SimpleMigrations.DatabaseProvider
open SimpleMigrations.Console
open Npgsql


[<EntryPoint>]
let main argv =
    let assembly = Assembly.GetExecutingAssembly()
    use db = new NpgsqlConnection "Host=127.0.0.1;Port=5432;Username=postgres;Password=WAhBRV2qHNA9c8yd744zH2w4;Database=postgres"
    let provider = PostgresqlDatabaseProvider(db)
    let migrator = SimpleMigrator(assembly, provider)
    let consoleRunner = ConsoleRunner(migrator)
    consoleRunner.Run(argv) |> ignore
    0