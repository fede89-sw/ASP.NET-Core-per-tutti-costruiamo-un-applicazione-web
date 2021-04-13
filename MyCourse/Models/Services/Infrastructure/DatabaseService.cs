using System.Data;
using Microsoft.Data.Sqlite;

namespace MyCourse.Models.Services.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        // Con AdoNet l'oggetto con un risultato query è di tipo 'DataSet'
        public DataSet Query(string query)
        {
            // istanza della connessione al database a cui passo come parametro il percorso del database
            // la 'connection string', ovvero come passare il parametro del percorso al database da usare può cambiare a seconda del
            // tipo di database scelto; consulta: https://www.connectionstrings.com/
            using(var connection = new SqliteConnection("Data Source=Data/MyCourse.db"))
            { 
                connection.Open(); // mi faccio dare una connessione dal 'connection pool'
                using(var command = new SqliteCommand(query, connection)) // oggetto 'SqliteCommand' per inviare una query al database
                {
                    using(var results = command.ExecuteReader()) // esegue una query che torna un oggetto 'SqliteDataReader' da cui possiamo leggere i risultati una riga alla volta
                    {
                        // variabile che restituirò con i risultati della query da passare al servizio applicativo;
                        // Un DataSet è un oggetto che contiene una o piu tabelle di risultati che arrivano dal database,
                        //  per poi passarle ad altri componenti dell'applicazione
                        var dataSet = new DataSet(); 
                        // il DataSet è una collezione di DataTable, quindi credo la table che assegno al DataSet
                        var dataTable = new DataTable();

                        // stratagemma per risolvere un bug del pacchetto Microsoft in 'dataTable.Load(results);'
                        // In una versione successiva lo risolvono e non serve questa riga sotto.
                        dataSet.EnforceConstraints = false;

                        dataSet.Tables.Add(dataTable);
                        dataTable.Load(results); // legge riga per riga un oggetto 'DataReader' e li salva nella tabella del DataTable
                        return dataSet;
                    }
                }
            }
        }
    }
}