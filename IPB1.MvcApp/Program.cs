using IPB1.MvcApp.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp_.txt", rollingInterval: RollingInterval.Hour)
            .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("LogDbConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Tbl_LogEvent",
                        AutoCreateSqlTable = true,
                        AutoCreateSqlDatabase = true
                    })
            .CreateLogger();

    builder.Services.AddSerilog();

    // Add services to the container.
    builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

    builder.Services.AddHttpClient();

    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}