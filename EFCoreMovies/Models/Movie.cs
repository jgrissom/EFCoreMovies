using System.Collections.Generic;

namespace EFCoreMovies.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
