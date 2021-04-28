using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyCourse.Models.InputModels;
using MyCourse.Models.Options;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class MemoryCacheCourseService : ICachedCourseService
    {
        public ICourseService CourseService { get; }
        public IMemoryCache MemoryCache { get; }
        public IOptionsMonitor<CachedLifeOptions> CachedLifeOptions { get; }

        public MemoryCacheCourseService(ICourseService courseService, IMemoryCache memoryCache, IOptionsMonitor<CachedLifeOptions> cachedLifeOptions)
        {
            this.CachedLifeOptions = cachedLifeOptions;
            this.MemoryCache = memoryCache;
            this.CourseService = courseService;

        }

        public Task<ListViewModel<CourseViewModel>> getCoursesAsync(CourseListInputModel model)
        {
            //Metto in cache i risultati solo per le prime 5 pagine del catalogo, che reputo essere
            //le più visitate dagli utenti, e che perciò mi permettono di avere il maggior beneficio dalla cache.
            //E inoltre, metto in cache i risultati solo se l'utente non ha cercato nulla.
            //In questo modo riduco drasticamente il consumo di memoria RAM
            bool canCache = model.Page <= 5 && string.IsNullOrEmpty(model.Search);


            // imposto la chiave dinamica $"Courses{search}", altrimenti se faccio una ricerca
            // mi ritorna cmq la lista dei corsi completa
            // UPDATE: per lo stesso motivo sopra, aggiungo anche -{page} avendo messo la paginazione.
            // E' importante modificare la chiave Cache in base od ogni paramentro che mi fa generare la 
            // lista dei corsi all'interno del catalogo, altrimenti per la cache sono tutte la stessa pagina.
            // UPDATE: aggiunto anche -{orderby} e -{ascending}; la chiave sta diventando importante, con 
            // innumerevoli combinazioni possibili; la memoria RAM potrebbe risentirne..vedremo poi di ragiornarci su 
            
            //Se canCache è true, sfrutto il meccanismo di caching
            if (canCache)
            {
                return MemoryCache.GetOrCreateAsync($"Courses{model.Page}-{model.OrderBy}-{model.Ascending}", cacheEntry =>
                {
                    // cacheEntry.SetSize(1);
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration));
                    return CourseService.getCoursesAsync(model);
                });
            }
            //Altrimenti uso il servizio applicativo sottostante, che recupererà sempre i valori dal database
            return CourseService.getCoursesAsync(model);
        }

        public Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            return MemoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>
            {
                // cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration));
                return CourseService.getCourseDetailAsync(id);
            });
        }
    }
}