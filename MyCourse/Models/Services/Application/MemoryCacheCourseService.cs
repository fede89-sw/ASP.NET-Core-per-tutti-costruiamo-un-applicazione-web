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

        public Task<List<CourseViewModel>> getCoursesAsync(CourseListInputModel model)
        {
            // imposto la chiave dinamica $"Courses{search}", altrimenti se faccio una ricerca
            // mi ritorna cmq la lista dei corsi completa
            // UPDATE: per lo stesso motivo sopra, aggiungo anche -{page} avendo messo la paginazione.
            // E' importante modificare la chiave Cache in base od ogni paramentro che mi fa generare la 
            // lista dei corsi all'interno del catalogo, altrimenti per la cache sono tutte la stessa pagina.
            // UPDATE: aggiunto anche -{orderby} e -{ascending}; la chiave sta diventando importante, con 
            // innumerevoli combinazioni possibili; la memoria RAM potrebbe risentirne..vedremo poi di ragiornarci su 
            return MemoryCache.GetOrCreateAsync($"Courses{model.Search}-{model.Page}-{model.OrderBy}-{model.Ascending}", cacheEntry =>
            {
                // cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration));
                return CourseService.getCoursesAsync(model);
            });
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