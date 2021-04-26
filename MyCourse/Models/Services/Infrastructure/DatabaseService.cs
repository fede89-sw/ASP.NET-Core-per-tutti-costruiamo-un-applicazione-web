using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.ValueTypes;

namespace MyCourse.Models.Services.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        public readonly IOptionsMonitor<ConnectionStringOptions> ConnectionStringOptions;
        public readonly ILogger<DatabaseService> Logger;

        public DatabaseService(IOptionsMonitor<ConnectionStringOptions> ConnectionStringOptions, ILogger<DatabaseService> logger)
        {
            this.Logger = logger;
            this.ConnectionStringOptions = ConnectionStringOptions;

        }
        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            Logger.LogInformation(formattableQuery.Format, formattableQuery.GetArguments());

            var queryArguments = formattableQuery.GetArguments();
            var sqliteParameters = new List<SqliteParameter>();
            for (var i = 0; i < queryArguments.Length; i++)
            {
                if(queryArguments[i] is Sql){
                    // se l'oggetto è di tipo Sql, classe creata da me, non renderlo un parametro;
                    // vedi AdoNetCourseService in getCourses()
                    continue;
                }
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]);
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@" + i;
            }
            string query = formattableQuery.ToString();

            string connectionString = ConnectionStringOptions.CurrentValue.Default;
            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddRange(sqliteParameters);

                    using (var results = await command.ExecuteReaderAsync())
                    {
                        var dataSet = new DataSet();
                        dataSet.EnforceConstraints = false;

                        do
                        {
                            var dataTable = new DataTable();

                            dataSet.Tables.Add(dataTable);
                            dataTable.Load(results);
                        } while (!results.IsClosed);

                        return dataSet;
                    }
                }
            }
        }
    }
}