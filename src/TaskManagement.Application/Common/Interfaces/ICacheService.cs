using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string cacheKey, T value);

        Task RemoveAsync(string key);
    }
}
