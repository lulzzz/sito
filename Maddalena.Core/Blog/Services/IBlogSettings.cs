using System;
using System.Collections.Generic;
using System.Text;

namespace Maddalena.Core.Blog.Services
{
    public interface IBlogSettings
    {
        int CommentsCloseAfterDays { get; set; }
        int PostsPerPage { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Owner { get; set; }
    }
}
