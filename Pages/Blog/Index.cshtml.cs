using System;
using System.Threading.Tasks;
using ButterCMS;
using ButterCMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SWMNU_NET.Configuration;
using SWMNU_NET.Models;

namespace SWMNU_NET.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IHostingEnvironment _env;
        private readonly UrlOptions _urlOptions;
        private readonly ButterCmsOptions _siteOptions;
        private readonly ButterCMSClient _client;
        private IMemoryCache _cache;

        public IndexModel(IHostingEnvironment env,
                          IOptions<UrlOptions> urlOptions,
                          IOptions<ButterCmsOptions> siteOptions,
                          ButterCMSClient client)
        {
            _env = env;
            _urlOptions = urlOptions.Value;
            _siteOptions = siteOptions.Value;
            _client = client;
        }
        public BlogListViewModel BlogListViewModel { get; set; }

        
        public async Task<IActionResult> OnGet(int page = 1)
        {
            var postsPerPage = 10;

            var response = await _client.ListPostsAsync(page, postsPerPage, false, _siteOptions.PrimaryAuthorSlug);

            BlogListViewModel = new BlogListViewModel
            {
                Posts = response.Data,
                Count = response.Meta.Count,
                NextPage = response.Meta.NextPage,
                CurrentPage = page,
                PreviousPage = response.Meta.PreviousPage,
                TotalPages = Convert.ToInt32(Math.Floor(decimal.Divide(response.Meta.Count, postsPerPage)))
            };

            return Page();
        }
    }
}