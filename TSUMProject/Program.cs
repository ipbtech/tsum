var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Home/Error");

app.UseHsts();
app.UseHttpsRedirection();

app.UseRouting();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}")
    .WithStaticAssets();

app.Run();
