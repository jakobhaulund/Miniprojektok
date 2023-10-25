using System;
namespace Shared
{
	public class Board
	{
		public Board()
		{

			Post = new List<Post>();
			
		}

		public List<Post> Post { get; set; }
		public long BoardId { get; set; }
	}

}

