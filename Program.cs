using EmergencySituations.DataBase;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.Converters.Add(new JsonDate());
});

MyDataBase.Setup(builder.Configuration.GetConnectionString("DataBaseFile"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();

