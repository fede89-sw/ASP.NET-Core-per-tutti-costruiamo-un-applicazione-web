using System.Data;

namespace MyCourse.Models.Services.Infrastructure
{
    public interface IDatabaseService
    {
        DataSet Query(string query);
    }
}