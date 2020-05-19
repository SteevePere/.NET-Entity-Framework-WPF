using Data.src.dao.interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Data.src.dao.implementation
{

	class GenreDao : IGenreDao
	{

		// Returns List of Genre objects or null
		public List<Genre> findAllGenres()
		{
			using (var context = new MyMDB())
			{
				List<Genre> genres = context.Genres.ToList();

				return genres;
			}
		}


		// Returns Genre object or null
		public Genre findGenreById(int genreId)
		{
			using (var context = new MyMDB())
			{
				Genre genreFound = context.Genres.Where(genre => genre.GNR_ID == genreId).FirstOrDefault<Genre>();

				return genreFound;
			}
		}


		// Returns Genre object or null
		public Genre findGenreByLabel(string genreLabel)
		{
			using (var context = new MyMDB())
			{
				Genre genreFound = context.Genres.Where(genre => genre.GNR_LABEL == genreLabel).FirstOrDefault<Genre>();

				return genreFound;
			}
		}


		// Returns List of Genre objects or null
		public List<Genre> findGenresByMovie(int movieId)
		{
			using (var context = new MyMDB())
			{
				List<Genre> genres = context.Genres.Where(genre => genre.Movies.Any(movie => movie.MOV_ID == movieId)).ToList();

				return genres;
			}
		}


		// Returns Genre object or null
		public Genre createGenre(Genre genre)
		{
			using (var context = new MyMDB())
			{
				if (isGenreValid(genre))
				{
					context.Genres.Add(genre);
					context.SaveChanges();
				}

				else genre = null;
			}

			return genre;
		}


		// Returns Genre object or null
		public Genre updateGenre(Genre updatedGenre)
		{
			using (var context = new MyMDB())
			{
				var genreToUpdate = context.Genres.SingleOrDefault(genre => genre.GNR_ID == updatedGenre.GNR_ID);

				if (genreToUpdate != null && isGenreValid(updatedGenre))
				{
					genreToUpdate.GNR_LABEL = updatedGenre.GNR_LABEL;
					context.SaveChanges();
				}

				else updatedGenre = null;
			}

			return updatedGenre;
		}


		// Returns bool (true if deleted)
		public bool deleteGenre(Genre genre)
		{
			bool deleted = false;

			if (genre is object)
			{
				Genre genreToDelete = findGenreById(genre.GNR_ID);

				using (var context = new MyMDB())
				{
					if (genreToDelete is object)
					{
						context.Entry(genreToDelete).State = EntityState.Deleted;
						context.SaveChanges();

						deleted = true;
					}
				}
			}

			return deleted;
		}


		// --- Helper methods --- //


		// Returns bool (true if genre is valid)
		private bool isGenreValid(Genre genre)
		{
			bool isValidGenre = false;

			if (genre.GNR_LABEL != null && findGenreByLabel(genre.GNR_LABEL) == null) // findGenreByLabel part ensures label unicity
			{
				isValidGenre = true;
			}

			return isValidGenre;
		}

	}

}
