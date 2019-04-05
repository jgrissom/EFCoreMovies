using System;
using System.IO;
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

            StreamReader sr = new StreamReader("movies.csv");
            // if the text file is empty, do not seed
            if (!sr.EndOfStream)
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
                        Console.WriteLine($"{movie.Title} - {movieDetails[2].Replace('|', ',')}");
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
                        Console.WriteLine($"{movie.Title} - {line.Replace('|', ',')}");
                    }
                }
                // close file when done
                sr.Close();
            }
            else
            {
                logger.Info("Database NOT seeded");
            }
            logger.Info("Program ended");
        }
    }
}
