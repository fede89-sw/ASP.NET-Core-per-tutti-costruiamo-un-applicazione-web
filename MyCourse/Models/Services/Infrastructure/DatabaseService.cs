using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MyCourse.Models.Services.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        // Con AdoNet l'oggetto con un risultato query è di tipo 'DataSet'
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


            // istanza della connessione al database a cui passo come parametro il percorso del database
            // la 'connection string', ovvero come passare il parametro del percorso al database da usare può cambiare a seconda del
            // tipo di database scelto; consulta: https://www.connectionstrings.com/
            using(var connection = new SqliteConnection("Data Source=Data/MyCourse.db"))
            { 
                await connection.OpenAsync(); // mi faccio dare una connessione dal 'connection pool'
                using(var command = new SqliteCommand(query, connection)) // oggetto 'SqliteCommand' per inviare una query al database
                {
                    // aggiungo i parametri creati sopra, con il valore passato da barra URL, per proteggermi dalla Query Injection
                    command.Parameters.AddRange(sqliteParameters);
                    
                    using(var results = await command.ExecuteReaderAsync()) // esegue una query che torna un oggetto 'SqliteDataReader' da cui possiamo leggere i risultati una riga alla volta
                    {
                        // variabile che restituirò con i risultati della query da passare al servizio applicativo;
                        // Un DataSet è un oggetto che contiene una o piu tabelle di risultati che arrivano dal database,
                        //  per poi passarle ad altri componenti dell'applicazione
                        var dataSet = new DataSet(); 

                        // stratagemma per risolvere un bug del pacchetto Microsoft in 'dataTable.Load(results);'
                        // In una versione successiva lo risolvono e non serve questa riga sotto.
                        dataSet.EnforceConstraints = false;

                        // se eseguo più query contemporaneamente mi serviranno piu tabelle, una per ogni istruzione SELECT fatta;
                        // Con piu tabelle di risultati mi servono altrettanti oggetti 'DataTable' da mettere poi nel DataSet.
                        // Faccio un ciclo do - while finchè l'oggetto DataReader è aperto, in quanto il DataTable , quando invochiamo il suo metodo 'Load()'guarda
                        // se all'interno del DataReader ci sono altre tabelle di risultati da leggere; se ci sono lascia il DataReader 
                        // aperto, se no lo chiude.
                        do
                        {
                            // il DataSet è una collezione di DataTable, quindi credo la table che assegno al DataSet
                            var dataTable = new DataTable();

                            dataSet.Tables.Add(dataTable);
                            dataTable.Load(results); // legge riga per riga un oggetto 'DataReader' e li salva nella tabella del DataTable
                        }while(!results.IsClosed);

                        return dataSet;
                    }
                }
            }
        }
    }
}