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
    public class PostDetailModel : PageModel
    {
        private readonly IHostingEnvironment _env;
        private readonly UrlOptions _urlOptions;
        private readonly ButterCmsOptions _siteOptions;
        private readonly ButterCMSClient _client;

        public PostDetailModel(IHostingEnvironment env,
                              IOptions<UrlOptions> urlOptions,
                              IOptions<ButterCmsOptions> siteOptions,
                              ButterCMSClient client)
        {
            _env = env;
            _urlOptions = urlOptions.Value;
            _siteOptions = siteOptions.Value;
            _client = client;
        }

        public Post Post { get; set; }
        public async Task<IActionResult> OnGet(string slug)
        {
            if (string.IsNullOrEmpty(slug) || string.IsNullOrWhiteSpace(slug))
                RedirectToPage("/");

            PostResponse post = await _client.RetrievePostAsync(slug);

            Post = post.Data;

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}