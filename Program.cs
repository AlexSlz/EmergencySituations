using EmergencySituations.DataBase;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

MyDataBase.Setup(builder.Configuration.GetConnectionString("DataBaseFile"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();
