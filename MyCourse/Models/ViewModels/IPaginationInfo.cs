namespace MyCourse.Models.ViewModels
{
    // interfaccia per le proprietà necessarie al ViewComponent 'PaginationBarViewComponent',
    // in modo da avere quelle e nessuna extra in più che non serve.
    public interface IPaginationInfo
    {
        int CurrentPage { get; }
        int TotalResults { get; }
        int ResultsPerPage { get; }

        string Search { get; }
        string OrderBy { get; }
        bool Ascending { get; }

    }
}