using ButterCMS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SWMNU_NET.Configuration;
using SWMNU_NET.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SWMNU_NET.Services
{
    public class TaxonomyService
    {
        private readonly IHostingEnvironment _env;
        private readonly UrlOptions _urlOptions;
        private readonly ButterCmsOptions _siteOptions;
        private readonly ButterCMSClient _client;

        public TaxonomyService(IHostingEnvironment env,
            IOptions<UrlOptions> urlOptions,
            IOptions<ButterCmsOptions> siteOptions,
            ButterCMSClient client)
        {
            _env = env;
            _urlOptions = urlOptions.Value;
            _siteOptions = siteOptions.Value;
            _client = client;
        }

        public CategoriesListViewModel ListAllCategories()
        {
            var response = _client.ListCategories().ToList();

            var model = new CategoriesListViewModel
            {
                Categories = response,
                Count = response.Count
            };

            return model;
        }

        public TagsListViewModel ListAllTags()
        {
            var response = _client.ListTags().ToList();

            var model = new TagsListViewModel
            {
                Tags = response,
                Count = response.Count
            };

            return model;
        }
    }
}
