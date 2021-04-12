using System.Data;

namespace MyCourse.Models.Services.Infrastructure
{
    public class DatabaseService : IDatabaseService
    {
        // Con AdoNet l'oggetto con un risultato query è di tipo 'DataSet'
        DataSet IDatabaseService.Query(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}