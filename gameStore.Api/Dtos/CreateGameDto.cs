using System.ComponentModel.DataAnnotations;

namespace gameStore.Api.Dtos;

public record class CreateGameDto( 
   [Required][StringLength(50)]string Name, 
   int GenreId, 
   [Range(1,300)]decimal Price, 
    DateOnly ReleaseDate
);
 