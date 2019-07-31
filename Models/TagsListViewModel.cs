using ButterCMS.Models;
using System.Collections.Generic;

namespace SWMNU_NET.Models
{
    public class TagsListViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }
        public int Count { get; set; }
    }
}
