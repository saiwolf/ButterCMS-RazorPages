using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWMNU_NET.Configuration
{
    public class ButterCmsOptions
    {
        public string ApiKey { get; set; }
        public int BlogPostsPerPage { get; set; }
        public string PrimaryAuthorSlug { get; set; }
    }
}
