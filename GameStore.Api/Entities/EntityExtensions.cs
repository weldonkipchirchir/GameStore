using GameStore.Api.Dtos;

namespace GameStore.Api.Entities
{
    public static class EntityExtensions
    {
        public static GameDto AsDto(this Game game) => new(game.Id, game.Name, game.Genre, game.Price, game.ReleaseDate, game.ImageUrl);
    }
}