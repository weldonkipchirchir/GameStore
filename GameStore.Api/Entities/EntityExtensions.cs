using GameStore.Api.Dtos;

namespace GameStore.Api.Entities
{
    public static class EntityExtensions
    {
        public static GameDtoV1 AsDtoV1(this Game game) => new(game.Id, game.Name, game.Genre, game.Price, game.ReleaseDate, game.ImageUri);

        public static GameDtoV2 AsDtoV2(this Game game) => new(game.Id, game.Name, game.Genre, game.Price, game.Price - (game.Price * .3m), game.ReleaseDate, game.ImageUri);
    }
}