using Library.Models.DTO;
using Library.Controllers;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Library.Models.Entities;

public class TableService
{
    private readonly LibraryContext _context;

    public TableService(LibraryContext context)
    {
        _context = context;
    }

    // Отримання списку таблиць
    public async Task<List<string>> GetTableNamesAsync()
    {
        var tableNames = new List<string>();
        var connection = _context.Database.GetDbConnection();
        var databaseName = connection.Database;

        Console.WriteLine(databaseName);

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT table_name FROM information_schema.tables " +
                                $"WHERE table_schema = '{databaseName}';";
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }
        }

        return tableNames;
    }


    // Виконання SQL-запиту
    public async Task ExecuteSqlAsync(string sql)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }

    // Отримання списку полів таблиці
    public async Task<TableDTO> GetTableAsync(string tableName)
    {
        var columns = new List<TableColumnDTO>();
        var connection = _context.Database.GetDbConnection();
        var databaseName = connection.Database; // Отримуємо назву бази даних

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE " +
                                $"FROM INFORMATION_SCHEMA.COLUMNS " +
                                $"WHERE TABLE_NAME = @TableName AND TABLE_SCHEMA = @DatabaseName";
            
            var tableNameParam = command.CreateParameter();
            tableNameParam.ParameterName = "@TableName";
            tableNameParam.Value = tableName;
            command.Parameters.Add(tableNameParam);
            
            var databaseNameParam = command.CreateParameter();
            databaseNameParam.ParameterName = "@DatabaseName";
            databaseNameParam.Value = databaseName;
            command.Parameters.Add(databaseNameParam);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    columns.Add(new TableColumnDTO
                    {
                        ColumnName = reader.GetString(0),
                        DataType = Enum.Parse<TableDataType>(reader.GetString(1), true),
                        IsNullable = reader.GetString(3) == "YES"
                    });
                }
            }
        }

        var table = new TableDTO
        {
            TableName = tableName,
            Columns = columns
        };

        return table;
    }



    public async Task AddColumn(string tableName, TableColumnDTO column)
    {
        var sql = $"ALTER TABLE {tableName} ADD {column.ColumnName} {Enum.GetName(typeof(TableDataType), column.DataType)}" +
        $"{(column.DataType == TableDataType.NVARCHAR || column.DataType == TableDataType.VARCHAR ? $"(255)" : "")} " +
        $"{(column.IsNullable ? "NULL" : "NOT NULL")} DEFAULT {DefaultValue(column.DataType)};";

        await ExecuteSqlAsync(sql);
    }

    public async Task DeleteColumn(string tableName, string columnName)
    {
        var t = await GetTableAsync(tableName);
        
        if(t != null && t.Columns.Count == 1)
            await DeleteTable(tableName);
        else{
            var sql = $"ALTER TABLE {tableName} DROP COLUMN {columnName};";
            await ExecuteSqlAsync(sql);
        }
    }

    public async Task EditColumn(string tableName, TableColumnDTO column){
        await DeleteColumn(tableName, column.ColumnName);
        await AddColumn(tableName, column);
    }
    
    public async Task AddTable(AddTableRequest request){
        var sql = "CREATE TABLE " + request.TableName + $"({request.TableName}Id INT PRIMARY KEY AUTO_INCREMENT);";
        await ExecuteSqlAsync(sql);
    }

    public async Task DeleteTable(string tableName){
        var sql = "DROP TABLE " + tableName;
        await ExecuteSqlAsync(sql); 
    }

    public async Task<bool> TableExists(string tableName){
        var tableNames = await GetTableNamesAsync();
        return tableNames.Any(x => x == tableName);
    }

    private string DefaultValue(TableDataType type) => type switch
    {
        TableDataType.INT => "42",
        TableDataType.BIGINT => "42",
        TableDataType.NVARCHAR => "'default'",
        TableDataType.VARCHAR => "'default'",
        TableDataType.LONGTEXT => "'something'",
        TableDataType.CHAR => "'d'",
        TableDataType.DATETIME => "'1900-01-01'",
        TableDataType.DATE => "'1900-01-01'",
        _ => "'something'"
    };
}
