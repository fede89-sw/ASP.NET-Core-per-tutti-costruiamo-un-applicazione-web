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
            return MemoryCache.GetOrCreateAsync($"Courses", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration));
                return CourseService.getCoursesAsync();
            });
        }

        public Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            return MemoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedLifeOptions.CurrentValue.Duration));
                return CourseService.getCourseDetailAsync(id);
            });
        }
    }
}