using Application.Common.Abstractions;
using Application.Core.Contracts;
using Application.Core.Interfaces.Core;
using Framework.Core.Helpers.Cache;

namespace Application.Core.Services.Core
{
    public class LocalizeServices : ILocalizeServices
    {
        private readonly ICachingService cachingService;
        private readonly IResourceServices resourceService;

        public LocalizeServices(ICachingService _cachingService, IResourceServices _resourceService)
        {
            cachingService = _cachingService;
            resourceService = _resourceService;
        }

        public string Get(string module, string screen, string key)
        {
            // Gen key
            var lang = "ja";
            string cacheKey = $"resource_{lang}_{module}_{screen}";

            // Get from cache
            var data = cachingService.Get(cacheKey, () =>
            {
                var items = resourceService.GetByScreen(lang, module, screen);
                return items.Count() > 0 ? items : null;
            });

            if (data != null && data.Count > 0)
            {
                var dataItem = data.Where(x => x.key == key).FirstOrDefault();
                if (dataItem != null)
                    return dataItem.text;
                else
                    return RegisterNewResource(module, screen, key, lang, cacheKey).Result;
            }
            else
                return RegisterNewResource(module, screen, key, lang, cacheKey).Result;
        }

        public async Task<string> GetAsync(string module, string screen, string key)
        {
            // Gen key
            var lang = "ja";
            string cacheKey = $"resource_{lang}_{module}_{screen}";

            // Get from cache
            var data = cachingService.Get(cacheKey, () =>
            {
                var items = resourceService.GetByScreen(lang, module, screen);
                return items.Count() > 0 ? items : null;
            });

            if (data != null && data.Count > 0)
            {
                var dataItem = data.Where(x => x.key == key).FirstOrDefault();
                if (dataItem != null)
                    return dataItem.text;
                else
                    return await RegisterNewResource(module, screen, key, lang, cacheKey);
            }
            else
                return await RegisterNewResource(module, screen, key, lang, cacheKey);
        }

        private async Task<string> RegisterNewResource(string module, string screen, string key, string lang, string cacheKey)
        {
            await resourceService.Create(new ResourceRequest()
            {
                lang = lang,
                module = module,
                screen = screen,
                key = key,
                text = key
            });
            cachingService.ClearAsync(cacheKey);
            return key;
        }
    }
}
