using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MyCourse.Models.ViewModels;
using Newtonsoft.Json;

// Versione di MemoryCacheCourseService con IDistributedCache invece di IMemoryCache
// che risolve il problema dovuto allora scalare l'applicazione orizzontalmente
namespace MyCourse.Models.Services.Application
{
    public class DistributedCacheCourseService : ICachedCourseService
    {
        private readonly ICourseService courseService;
        private readonly IDistributedCache distributedCache;
        public DistributedCacheCourseService(ICourseService courseService, IDistributedCache distributedCache)
        {
            this.courseService = courseService;
            this.distributedCache = distributedCache;
        }
        public async Task<CourseDetailViewModel> getCourseDetailAsync(int id)
        {
            string key = $"Course{id}";

            //Proviamo a recuperare l'oggetto dalla cache
            string serializedObject = await distributedCache.GetStringAsync(key);

            //Se l'oggetto esisteva in cache (cioè se è diverso da null)
            if (serializedObject != null) {
                //Allora lo deserializzo e lo restituisco
                return Deserialize<CourseDetailViewModel>(serializedObject);
            }

            //Se invece non esisteva, lo andiamo a recuperare dal database
            CourseDetailViewModel course = await courseService.getCourseDetailAsync(id);

            //Prima di restituire l'oggetto al chiamante, lo serializziamo.
            //Cioè ne creiamo una rappresentazione stringa o binaria
            serializedObject = Serialize(course);

            //Impostiamo la durata di permanenza in cache
            var cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            //Aggiungiamo in cache l'oggetto serializzato 
            await distributedCache.SetStringAsync(key, serializedObject, cacheOptions);

            //Lo restituisco
            return course;
        }

        public async Task<List<CourseViewModel>> getCoursesAsync()
        {
            // imposto la Key per la Cache
            string key = $"Courses";
            // prendo l'oggetto in cache con chiave 'key' che è serializzato
            string serializedObject = await distributedCache.GetStringAsync(key);

            if (serializedObject != null) {
                // se c'è l'oggetto in RAM con chiave 'key', lo deserializzo
                return Deserialize<List<CourseViewModel>>(serializedObject);
            }
            
            // se l'oggetto non c'è in RAM lo prendo dal DB e lo serializzo
            List<CourseViewModel> courses = await courseService.getCoursesAsync();
            serializedObject = Serialize(courses);

            // setto le opzioni della DistributedCache, mettendo l'Absolute Expiration a 60 sec
            var cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            // scrivo l'oggetto serializzato nella cache distribuita
            await distributedCache.SetStringAsync(key, serializedObject, cacheOptions);
            return courses;
        }

        private string Serialize(object obj) 
        {
            //Convertiamo un oggetto in una stringa JSON
            return JsonConvert.SerializeObject(obj);
        }

        private T Deserialize<T>(string serializedObject)
        {
            //Riconvertiamo una stringa JSON in un oggetto
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}