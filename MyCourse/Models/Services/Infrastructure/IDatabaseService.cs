using System;
using System.Data;
using System.Threading.Tasks;

namespace MyCourse.Models.Services.Infrastructure
{
    public interface IDatabaseService
    {
        Task<DataSet> QueryAsync(FormattableString query);
    }
}