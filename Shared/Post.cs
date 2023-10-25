using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Post

    {

        public long postId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string user { get; set; }
        public List<Comment>? comments {get; set;} = new List<Comment>();
        public int vote { get; set; }
         

    }
}
