using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("config.json");
Library library = builder.Configuration.GetSection("Library").Get<Library>() ?? new Library();
var app = builder.Build();


Profile myProfile = new Profile();
myProfile.Name = "Max";
myProfile.Surname = "Yevstratiev";

library.Profiles.Add(myProfile);


app.MapGet("/", async context => context.Response.Redirect("/library"));

app.MapGet("/library", () => {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<h1>Welcome to the Library!</h1>");
    stringBuilder.Append("<h2>Explore:</h2>");
    stringBuilder.Append("<ul>");
    stringBuilder.Append($"<li><a href=\"/library/books\">Books</a></li>");
    stringBuilder.Append($"<li><a href=\"/library/profile\">Profile</a></li>");
    stringBuilder.Append("</ul>");
    return Results.Content(stringBuilder.ToString(), "text/html");
});
app.MapGet("/library/books", () => { 
    StringBuilder stringBuilder = new StringBuilder();
    foreach(var book in library.Books){
        stringBuilder.Append($"Title:{book.Title}\nAuthor:{book.Author}\n");
    }
    return stringBuilder.ToString();
});
app.MapGet("/library/profile/{id?}", (int? id) =>
{
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<h1>Profiles</h1>");
    stringBuilder.Append("<ul>");

    if (id is null)
    {
        for (int i = 0; i < library.Profiles.Count; i++)
        {
            var profile = library.Profiles[i];
            stringBuilder.Append($"<li><a href=\"/library/profile/{i}\">{profile.Name} {profile.Surname}</a></li>");
        }
    }
    else if (id >= 0 && id < library.Profiles.Count)
    {
        var profile = library.Profiles[(int)id];
        stringBuilder.Append($"<li>Profile Name: {profile.Name}</li>");
        stringBuilder.Append($"<li>Profile Surname: {profile.Surname}</li>");
    }
    else
    {
        stringBuilder.Append("<li>Profile not found</li>");
    }

    stringBuilder.Append("</ul>");
    return Results.Content(stringBuilder.ToString(), "text/html");
});
app.Run();
