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

            //Creiamo dei SqliteParameter a partire dalla FormattableString
            var queryArguments = formattableQuery.GetArguments(); // ottengo i parametri della f-string
            var sqliteParameters = new List<SqliteParameter>(); // creiamo una lista di SqliteParameter per aggiungerci i parametri
            for (var i = 0; i < queryArguments.Length; i++) // ciclo per il numero di parametri della f-string
            {
                // creo parametro di nome 'i' con valore del parametro passato in posizione 'i' (es: "0" con valore id corso=2)
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]); 

                // aggiungo il parametro alla lista di SqliteParameter(es. al parametro "0" corrisponde 2)
                sqliteParameters.Add(parameter);

                // aggiungo la '@' al parametro. In questo modo in 'formattableQuery' il valore passato come attributo verrà visulizzato
                // per es. con @0, a cui corrisponde valore 2; Per accedere a questo parametro DEVE esserci la chiocciola davanti al nome
                // del parametro
                // prima infatti 'formattableQuery' = "SELECT bla,bla FROM bla WHERE id={0}" -> 'queryArgument' della f-string 'formattableQuery'
                // dopo invece avrò in 'query' = "SELECT bla,bla FROM bla WHERE id=@0" -> parametro da leggere il 'sqliteParameters'
                queryArguments[i] = "@" + i;
            }
            string query = formattableQuery.ToString();

            using(var connection = new SqliteConnection("Data Source=Data/MyCourse.db"))
            { 
                await connection.OpenAsync(); // mi faccio dare una connessione dal 'connection pool'
                using(var command = new SqliteCommand(query, connection)) // oggetto 'SqliteCommand' per inviare una query al database
                {
                    // aggiungo i parametri creati sopra, con il valore passato da barra URL, per proteggermi dalla Query Injection
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