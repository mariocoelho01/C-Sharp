using Blog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();//Add MVC
builder.Services.AddDbContext<BlogDataContext>();//Add DataContext

var app = builder.Build();
app.MapControllers();//start at controllers


app.Run();
