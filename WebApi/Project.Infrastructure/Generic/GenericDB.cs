using Dapper;
using Project.Infrastructure.DbFactory;
using System.Data;

namespace Project.Infrastructure.Generic;

public interface IGenericDB
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null);
    Task<int> ExecuteAsync(string sql, object? parameters = null);
    Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null);
}

public class GenericDB(ISqlDbConnectionFactory connectionFactory) : IGenericDB
{
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        using var connection = connectionFactory.CreateConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        using var connection = connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using var connection = connectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }

    public async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null)
    {
        using var connection = connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<T>(sql, parameters);
    }
}