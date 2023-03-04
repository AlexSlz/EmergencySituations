var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

List<User> users = new List<User>() { new User("alex", 999), new User("test", 5) };

app.MapGet("/api/getUsers", () =>
{
    return users;
});

app.MapPost("/api/addUser", (User user) =>
{
    users.Add(user);
});

app.MapDelete("/api/removeUser/{id}", (int id) =>
{
    Console.WriteLine(id);
    users.RemoveAt(id);
});

app.MapFallbackToFile("index.html"); ;

app.Run();

public class User
{
    public string Name { get; set; }
    public int Power { get; set; }

    public User(string name, int power)
    {
        Name = name;
        Power = power;
    }
}