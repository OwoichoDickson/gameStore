using gameStore.Api.Data;
using gameStore.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var ConnString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(ConnString);

var app = builder.Build();

app.MapGameEndpoints();
app.MigrateDb();

app.Run();
