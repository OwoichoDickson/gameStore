using gameStore.Api.Data;
using gameStore.Api.Dtos;
using gameStore.Api.Entities;

namespace gameStore.Api.EndPoints;

public static class GameEndPoints
{
    const string GetEndPointName = "GetName";

    private static readonly List<GameDto> games = [
    new(
    1,
    "Fantastic 4",
    "Action",
    19.9M,
    new DateOnly(2004,4,15)),
new(
    2,
    "FIFA 4",
    "Sport",
    57.9M,
    new DateOnly(2045,6,11)),
new(
    3,
    "Sonic ",
    "RolePlay",
    49.9M,
    new DateOnly(1994,8,12)),
];

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        //GET Games
        group.MapGet("/", () => games);

        //GET Game
        group.MapGet("/{id}", (int id) =>

        {
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetEndPointName);

        //POST GAME

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>

        {
            

           Game game =new(){

            Name = newGame.Name,
            Genre = dbContext.Genres.Find(newGame.GenreId),
            GenreId = newGame.GenreId,
            Price = newGame.Price,
            ReleaseDate = newGame.ReleaseDate,

           };
           GameDto gameDto = new(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate

           );
        

         dbContext.Games.Add(game);
         dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetEndPointName, new { id = game.Id, gameDto });
        });

        //PUT GAMES

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {

            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate


            );
            return Results.NoContent();




        });


        // DELETE 

        group.MapDelete("/{id}", (int id) =>
        {

            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
        return group;
    }

}
