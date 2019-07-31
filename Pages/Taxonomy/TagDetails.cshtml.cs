using System;
using System.Threading.Tasks;
using ButterCMS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SWMNU_NET.Configuration;
using SWMNU_NET.Models;

namespace SWMNU_NET.Pages.Taxonomy
{
    public class TagDetailsModel : PageModel
    {
        private readonly IHostingEnvironment _env;
        private readonly UrlOptions _urlOptions;
        private readonly ButterCmsOptions _siteOptions;
        private readonly ButterCMSClient _client;

        public TagDetailsModel(IHostingEnvironment env,
            IOptions<UrlOptions> urlOptions,
            IOptions<ButterCmsOptions> siteOptions,
            ButterCMSClient client)
        {
            _env = env;
            _urlOptions = urlOptions.Value;
            _siteOptions = siteOptions.Value;
            _client = client;
        }

        public BlogListViewModel Blogs { get; set; }
        public TagViewModel Tag { get; set; }

        public async Task<IActionResult> OnGet(string slug, int page = 1)
        {
            var postsPerPage = 10;

            var tagResponse = (await _client.RetrieveTagAsync(slug));
            var postsResponse = (await _client.ListPostsAsync(page, postsPerPage, true, tagSlug: slug));

            Blogs = new BlogListViewModel
            {
                Posts = postsResponse.Data,
                Count = postsResponse.Meta.Count,
                NextPage = postsResponse.Meta.NextPage,
                CurrentPage = page,
                PreviousPage = postsResponse.Meta.PreviousPage,
                TotalPages = Convert.ToInt32(Math.Floor(decimal.Divide(postsResponse.Meta.Count, postsPerPage)))
            };

            Tag = new TagViewModel
            {
                Tag = tagResponse,
                Blogs = Blogs                
            };

            return Page();
        }
    }
}