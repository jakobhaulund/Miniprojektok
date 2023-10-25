using System;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using Data;
using Shared;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Service
{
    public class DataService
    {
        private BoardContext db { get; }

        public DataService(BoardContext db)
        {
            this.db = db;
        }

        public void SeedData()
        {
            try
            {
                Board board = db.Board.FirstOrDefault();

                if (board == null)
                {
                    board = new Board();

                    Post post = new Post
                    {
                        title = "Et opslag om fisk",
                        content = "En haj er en stor fisk ngl",
                        vote = 0,
                        user = "Furkan",
                        comments = new List<Comment>()
                    {
                        new Comment
                        {
                           commentContext = "fedt opslag ven",
                           user = "August"
                        },
                        new Comment
                        {
                            commentContext = "vil gerne høre mere pls",
                            user = "Rasmus"
                        }
                    }
                    };

                    board.Post.Add(post);

                    db.Board.Add(board);

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log undtagelsen eller udskriv en fejlmeddelelse
                Console.WriteLine("Fejl ved seeding af data: " + ex.Message);
            }


        }

        public Board GetBoard(int id)
        {
            return db.Board.Include(b => b.Post).ThenInclude(p => p.comments).FirstOrDefault(b => b.BoardId == id);
        }

        public string CreatePost(string title, string user, string content, int BoardId, List<Comment> comments)
        {
            Board board = db.Board.FirstOrDefault(b => b.BoardId == BoardId);
            if (board != null)
            {
                Post post = new Post { title = title, user = user, content = content, vote = 0, comments = new List<Comment>() };
                board.Post.Add(post);

                db.SaveChanges();

                return "Post oprettet";

            } else
            {
                return "Board kunne ikke findes :D";
            }


        }

        public string CreateComment(int postId, string commentContext, string user)
        {
            var post = db.Post.FirstOrDefault(p => p.postId == postId);
            if (post != null)
            {
                Comment comment = new Comment { commentContext = commentContext, user = user };
                post.comments.Add(comment);

                db.SaveChanges();

                return "kommentar oprettet  :D";


            }
            else
            {
                return "kunne ikke finde post :(";
            }


        }
        public string ChangeVote(int postId, int vote) 
        { 
            var post = db.Post.FirstOrDefault(p =>p.postId == postId); 
            if (post != null) 
            {
                post.vote += vote; 
                db.SaveChanges();

                return "Vote Ændret";

            }else
            {
                return "kunne finde opslaget :/"; 
            }
        }

    }
}
