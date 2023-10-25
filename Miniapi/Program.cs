using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Data;
using Service;
using Shared;

namespace miniapi { 
public class Program {

        public static void Main(string[] args) {

            var builder = WebApplication.CreateBuilder(args);
            var AllowSomeStuff = "_AllowSomeStuff";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSomeStuff, builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<BoardContext>(options =>
            options.UseSqlite($"Data Source=bin/reddit.db"));

            builder.Services.AddScoped<DataService>();

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var Dataservice = scope.ServiceProvider.GetRequiredService<DataService>();
                Dataservice.SeedData();
            }

                app.UseHttpsRedirection();

            app.UseCors(AllowSomeStuff);

            app.Use(async (context, next) =>
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                await next(context);
            });

            app.MapGet("/api/boards/{id}", (int id, DataService service) =>
            {
                return service.GetBoard(id);
            });

            app.MapPost("/api/posts", (DataService service, NewPostData data) =>
            {
                return service.CreatePost(data.title, data.user, data.Content, data.BoardId, data.comments);
            });

            app.MapPut("/api/posts/{postId}/votes", (int postId, NewPostData data, DataService service) =>
            {
                return service.ChangeVote(postId, data.vote);
            });

            app.MapPost("api/posts/{postId}/comments", (int postId, NewCommentata data, DataService service) =>
            {
                string result = service.CreateComment(postId, data.Commentcontext, data.User);
                return new {message = result};  
            });
            
            app.Run();

        }
        record NewPostData(string title, string user, int vote, string Content, int BoardId, List<Comment> comments);
        record NewCommentata(int postId, string Commentcontext, string User);
}
}