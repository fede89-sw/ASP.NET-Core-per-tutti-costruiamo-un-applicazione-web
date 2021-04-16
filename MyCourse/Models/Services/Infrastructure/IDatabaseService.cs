using System;
using System.Data;

namespace MyCourse.Models.Services.Infrastructure
{
    public interface IDatabaseService
    {
        DataSet Query(FormattableString query);
    }
}