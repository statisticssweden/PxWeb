using System;

namespace PXWeb.API.Services
{
    public interface ICacheService
    {
        void ClearCache(Type type);
    }
}