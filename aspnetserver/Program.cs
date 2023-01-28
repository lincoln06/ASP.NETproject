using aspnetserver.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options=>
{
    options.AddPolicy("CORDPolicy",
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins();
        });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1",new OpenApiInfo { Title="ASP.Net React Tutorial", Version="v1" });
});


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.DocumentTitle= "I tak jeste� g�upi";
    swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API jest bardzo proste");
    swaggerUIOptions.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId) =>
{
    Post postToReturn=await PostsRepository.GetPostByIdAsync(postId);
    if(postToReturn!=null)
    {
        return Results.Ok(postToReturn);
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Post Endpoints");

app.MapPost("/create-post", async (Post postToCreate) =>
{
    bool createSuccessfull = await PostsRepository.CreatePostAsync(postToCreate);
    if (createSuccessfull)
    {
        return Results.Ok("Utworzono pomyslnie");
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Post Endpoints");

app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool createSuccessfull = await PostsRepository.UpdatePostAsync(postToUpdate);
    if (createSuccessfull)
    {
        return Results.Ok("Zaktualizowano pomyslnie");
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Post Endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>
{
    bool deleteSuccessful = await PostsRepository.DeletePostAsync(postId);
    if (deleteSuccessful)
    {
        return Results.Ok("Usuni�to pomyslnie");
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Post Endpoints");
app.Run();
