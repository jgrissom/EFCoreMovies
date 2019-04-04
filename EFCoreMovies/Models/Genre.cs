using System.Collections.Generic;

namespace EFCoreMovies.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreText { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
