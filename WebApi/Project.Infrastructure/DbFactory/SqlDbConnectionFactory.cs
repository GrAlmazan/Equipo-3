using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Project.Infrastructure.DbFactory;

public interface ISqlDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public class SqlDbConnectionFactory(IConfiguration configuration) : ISqlDbConnectionFactory
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException(nameof(configuration));

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}