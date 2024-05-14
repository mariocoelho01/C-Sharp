using Blog.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => 
{
    options.SuppressModelStateInvalidFilter= true;
});//Add MVC, disable automatic authentication model status
builder.Services.AddDbContext<BlogDataContext>();//Add DataContext

var app = builder.Build();
app.MapControllers();//start at controllers


app.Run();
