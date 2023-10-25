using System;
using Microsoft.EntityFrameworkCore;
using Shared;


namespace Data
{
    public class BoardContext : DbContext
    {
        public DbSet<Board> Board => Set<Board>();

        public DbSet<Post> Post => Set<Post>();
        
        public DbSet<Comment> comments => Set<Comment>();

        public BoardContext(DbContextOptions<BoardContext> options)
            : base(options)
        {
            // Den her er tom. Men ": base(options)" sikre at constructor
            // på DbContext super-klassen bliver kaldt.
        }
    }
}

