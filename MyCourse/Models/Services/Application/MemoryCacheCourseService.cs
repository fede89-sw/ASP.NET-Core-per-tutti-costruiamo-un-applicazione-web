using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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

        public Task<List<CourseViewModel>> getCoursesAsync()
        {
            // chiedo se esiste in memoria RAM un oggetto con Key = $"Courses";
            // se c'è lo restituirà, altrimenti con la lambda lo vado a prendere dal DB
            // settando la durata dell'oggetto in Cache a 60 secondi
            return MemoryCache.GetOrCreateAsync($"Courses", cacheEntry =>
            {
                // avendo impostato l'uso massimo di RAM in appsettings.json, devo specificare la dimensione
                // degli oggetti che vi ci metto. Invece di usare Mb, visto che devo impostare io le dimensioni,
                // uso le unità: su 1000 unità massime da mettere in RAM, la lista corsi conta per 1.
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration)); // setto 60 sec di cache
                return CourseService.getCoursesAsync(); // prendo dal DB i corsi se non sono già presenti in RAM
            });
        }

        public Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            // chiedo se esiste in memoria RAM un oggetto con Key = $"Course{id}";
            // se c'è lo restituirà, altrimenti con la lambda lo vado a prendere dal DB
            // settando la durata dell'oggetto in Cache a 60 secondi
            return MemoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>
            {
                 // avendo impostato l'uso massimo di RAM in appsettings.json, devo specificare la dimensione
                 // degli oggetti che vi ci metto. Invece di usare Mb, visto che devo impostare io le dimensioni,
                 // uso le unità: su 1000 unità massime da mettere in RAM, il corso conta per 1.
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration));
                return CourseService.getCourseDetailAsync(id);
            });
        }
    }
}