using Data.src.dao.interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Data.src.dao.implementation
{

	class MovieDao : IMovieDao
	{

		// Returns List of Movie objects or null
		public List<Movie> findAllMovies()
		{
			using (var context = new MyMDB())
			{
				List<Movie> movies = context.Movies.ToList();

				return movies;
			}
		}


		// Returns a Movie object or null
		public Movie findMovieById(int movieId)
		{
			using (var context = new MyMDB())
			{
				Movie movieFound = context.Movies.Where(movie => movie.MOV_ID == movieId).FirstOrDefault<Movie>();

				return movieFound;
			}
		}


		// Returns a Movie object or null
		public Movie findMovieByTitle(string movieTitle)
		{
			using (var context = new MyMDB())
			{
				Movie movieFound = context.Movies.Where(movie => movie.MOV_TITLE == movieTitle).FirstOrDefault<Movie>();

				return movieFound;
			}
		}


		// Returns List of Movie objects or null
		public List<Movie> findMoviesByGenre(string genreLabel)
		{
			using (var context = new MyMDB())
			{
				List<Movie> movies = context.Movies.Where(movie => movie.Genres.Any(genre => genre.GNR_LABEL == genreLabel)).ToList();

				return movies;
			}
		}


		// Returns a Movie object or null
		public Movie createMovie(Movie movie)
		{
			using (var context = new MyMDB())
			{
				List<int> genreIds = movie.Genres.Select(genre => genre.GNR_ID).ToList();
				List<Genre> movieGenres = context.Genres.Where(genre => genreIds.Contains(genre.GNR_ID)).ToList(); // need to track Genres coming from other DAO
				movie.Genres = movieGenres;

				if (isMovieValid(movie))
				{
					context.Movies.Add(movie);
					context.SaveChanges();
				}

				else movie = null;
			}

			return movie;
		}


		// Returns a Movie object or null
		public Movie updateMovie(Movie updatedMovie)
		{
			using (var context = new MyMDB())
			{
				Movie movieToUpdate = context.Movies
					.Where(movie => movie.MOV_ID == updatedMovie.MOV_ID)
					.Include(dbMovie => dbMovie.Genres).FirstOrDefault<Movie>();

				if (movieToUpdate != null && isMovieValid(updatedMovie))
				{
					List<int> updatedMovieGenreIds = updatedMovie.Genres.Select(genre => genre.GNR_ID).ToList();
					List<Genre> updatedMovieGenres = context.Genres.Where(genre => updatedMovieGenreIds.Contains(genre.GNR_ID)).ToList();

					movieToUpdate.MOV_TITLE = updatedMovie.MOV_TITLE;
					movieToUpdate.MOV_SYNOPSIS = updatedMovie.MOV_SYNOPSIS;
					movieToUpdate.Genres = updatedMovieGenres;

					context.SaveChanges();
				}

				else updatedMovie = null;
			}

			return updatedMovie;
		}


		// Returns bool (true if deleted)
		public bool deleteMovie(Movie movie)
		{
			bool deleted = false;

			if (movie is object)
			{
				using (var context = new MyMDB())
				{
					Movie movieToDelete = context.Movies // we need to be deleting movie +
						.Include(dbMovie => dbMovie.Reviews) // associated reviews +
						.Include(dbMovie => dbMovie.Genres) // MovieGenre relations (from join table)
						.First(dbMovie => dbMovie.MOV_ID == movie.MOV_ID); // getting actual movie

					foreach (Review review in movieToDelete.Reviews.ToList())
					{
						context.Reviews.Remove(review);
					}

					if (movieToDelete is object)
					{
						context.Entry(movieToDelete).State = EntityState.Deleted;
						context.SaveChanges();

						deleted = true;
					}
				}
			}

			return deleted;
		}


		// --- Helper methods --- //


		// Returns bool (true if movie is valid)
		private bool isMovieValid(Movie movie)
		{
			bool isValidMovie = false;

			if (movie.MOV_TITLE != null 
				&& (findMovieByTitle(movie.MOV_TITLE) == null || (movie.MOV_ID != 0 && findMovieByTitle(movie.MOV_TITLE).MOV_ID == movie.MOV_ID))) // ensures title unicity
			{
				isValidMovie = true;
			}

			return isValidMovie;
		}

	}

}
