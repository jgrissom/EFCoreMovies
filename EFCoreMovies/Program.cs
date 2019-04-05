using System;
using System.IO;
using System.Linq;
using EFCoreMovies.Models;
using NLog;

namespace EFCoreMovies
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            using (var context = new MovieContext())
            {
                StreamReader sr = new StreamReader("movies.csv");
                // if there are records in the database, or text file is empty, do not seed
                if (context.Movies.Count() == 0 && !sr.EndOfStream)
                {
                    // skip first line of data file
                    sr.ReadLine();
                    // read from the data file
                    while (!sr.EndOfStream)
                    {
                        // create instance of Movie class
                        Movie movie = new Movie();
                        string line = sr.ReadLine();
                        // first look for quote(") in string
                        // this indicates a comma(,) in movie title
                        int idx = line.IndexOf('"');
                        if (idx == -1)
                        {
                            // no quote = no comma in movie title
                            // movie details are separated with comma(,)
                            string[] movieDetails = line.Split(',');
                            movie.Title = movieDetails[1];
                            AddMovie(context, movie, movieDetails[2].Split('|'));
                        }
                        else
                        {
                            // quote = comma or quotes in movie title
                            // remove movieId and first comma from string
                            line = line.Substring(idx);
                            // find the last quote
                            idx = line.LastIndexOf('"');
                            // extract title (replace "" with ")
                            movie.Title = line.Substring(1, idx - 1).Replace("\"\"", "\"");
                            // remove title and next comma from the string
                            line = line.Substring(idx + 2);
                            AddMovie(context, movie, line.Split('|'));
                        }
                    }
                    // close file when done
                    sr.Close();
                    // commit updates to database
                    context.SaveChanges();
                }
                else
                {
                    logger.Info("Database NOT seeded");
                }
                // log results
                logger.Info($"{context.Movies.Count()} Movies");
                logger.Info($"{context.Genres.Count()} Genres");
                logger.Info($"{context.MovieGenres.Count()} MovieGenres");
            }

            logger.Info("Program ended");
        }

        private static void AddMovie(MovieContext context, Movie movie, String[] genreText)
        {
            // add movie to Movies table
            context.Movies.Add(movie);
            foreach (var gt in genreText)
            {
                if (gt.ToLower() != "(no genres listed)")
                {
                    MovieGenre mg = new MovieGenre() { Movie = movie };
                    // only add unique genres to Genres table
                    if (!context.Genres.Local.Any(g => g.GenreText == gt))
                    {
                        Genre genre = new Genre() { GenreText = gt };
                        context.Genres.Add(genre);
                        mg.Genre = genre;
                    }
                    else
                    {
                        Genre genre = context.Genres.Local.FirstOrDefault(g => g.GenreText == gt);
                        mg.Genre = genre;
                    }
                    // add all movie genres to MovieGenres table
                    context.MovieGenres.Add(mg);
                }
            }
        }
    }
}
