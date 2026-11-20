using WebRegiones.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IServicioRegiones, ServicioRegiones>(cliente =>
{
    cliente.BaseAddress = new Uri(builder.Configuration["ApiRegiones:Url"] ?? "https://localhost:7001/");
    cliente.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["ApiRegiones:ApiKey"] ?? "");
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
