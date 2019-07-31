using System.Collections.Generic;

namespace SWMNU_NET.Models
{
    public class AuthorListViewModel
    {
        public IEnumerable<ButterCMS.Models.Author> Authors { get; set; }
        public int Count { get; set; }
    }
}
