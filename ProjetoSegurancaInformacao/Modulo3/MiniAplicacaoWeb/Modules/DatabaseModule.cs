using Microsoft.Data.Sqlite;
using MiniAplicacaoWeb.Models;

namespace MiniAplicacaoWeb.Modules;

public static class DatabaseModule
{
    private const string ConnectionString = "Data Source=app.db";

    public static void InitializeDatabase()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Usuarios (
                Login TEXT PRIMARY KEY,
                SenhaHash TEXT NOT NULL,
                Tipo TEXT NOT NULL
            );
        ";
        command.ExecuteNonQuery();

        // Criar admin e comum padrões se a base estiver vazia
        if (GetUser("admin") == null)
        {
            CreateUser("admin", SecurityModule.HashPassword("123"), "Admin");
            CreateUser("user", SecurityModule.HashPassword("123"), "Comum");
        }
    }

    // Consultas parametrizadas (Prevenindo SQL Injection)
    public static void CreateUser(string login, string senhaHash, string tipo)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Usuarios (Login, SenhaHash, Tipo) VALUES (@login, @hash, @tipo)";
        command.Parameters.AddWithValue("@login", login);
        command.Parameters.AddWithValue("@hash", senhaHash);
        command.Parameters.AddWithValue("@tipo", tipo);
        command.ExecuteNonQuery();
    }

    public static UsuarioRecord? GetUser(string login)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT Login, SenhaHash, Tipo FROM Usuarios WHERE Login = @login";
        command.Parameters.AddWithValue("@login", login);
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new UsuarioRecord(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2)
            );
        }
        return null;
    }
}