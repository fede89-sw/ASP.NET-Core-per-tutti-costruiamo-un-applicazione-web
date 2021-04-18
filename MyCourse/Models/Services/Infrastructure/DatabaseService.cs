using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MyCourse.Models.Services.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            var queryArguments = formattableQuery.GetArguments(); 
            var sqliteParameters = new List<SqliteParameter>();
            for (var i = 0; i < queryArguments.Length; i++)
            {
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]); 
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@" + i;
            }
            string query = formattableQuery.ToString();

            using(var connection = new SqliteConnection("Data Source=Data/MyCourse.db"))
            { 
                await connection.OpenAsync();
                using(var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddRange(sqliteParameters);
                    
                    using(var results = await command.ExecuteReaderAsync())
                    {
                        var dataSet = new DataSet(); 
                        dataSet.EnforceConstraints = false;

                        do
                        {
                            var dataTable = new DataTable();

                            dataSet.Tables.Add(dataTable);
                            dataTable.Load(results);
                        }while(!results.IsClosed);

                        return dataSet;
                    }
                }
            }
        }
    }
}