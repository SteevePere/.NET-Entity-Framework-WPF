using System;
using System.Collections.Generic;
using Data.src.dao.interfaces;
using Data.src.factory;


namespace Data.src.facade
{

	public class ServiceFacade : IServiceFacade
	{

		private static ServiceFacade _instance;
		private static readonly object _padlock = new object();
		private AbstractDaoFactory.DaoFactoryType _DEFAULT_IMPLEMENTATION = AbstractDaoFactory.DaoFactoryType.MSSQL_DAO_FACTORY;

		private IGenreDao genreDao = null;
		private IMovieDao movieDao = null;
		private IReviewDao reviewDao = null;
		private IUserDao userDao = null;
		

		private ServiceFacade()
		{
			genreDao = AbstractDaoFactory.getFactory(_DEFAULT_IMPLEMENTATION).getGenreDao();
			movieDao = AbstractDaoFactory.getFactory(_DEFAULT_IMPLEMENTATION).getMovieDao();
			reviewDao = AbstractDaoFactory.getFactory(_DEFAULT_IMPLEMENTATION).getReviewDao();
			userDao = AbstractDaoFactory.getFactory(_DEFAULT_IMPLEMENTATION).getUserDao();
		}


		public static ServiceFacade getInstance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
					{
						_instance = new ServiceFacade();
					}

					return _instance;
				}
			}
		}


		public IGenreDao getGenreDao()
		{
			return genreDao;
		}


		public IMovieDao getMovieDao()
		{
			return movieDao;
		}


		public IReviewDao getReviewDao()
		{
			return reviewDao;
		}


		public IUserDao getUserDao()
		{
			return userDao;
		}

		#region Genres

		public List<Genre> findAllGenres()
		{
			return genreDao.findAllGenres();
		}


		public Genre findGenreById(int genreId)
		{
			return genreDao.findGenreById(genreId);
		}


		public Genre findGenreByLabel(string genreLabel)
		{
			return genreDao.findGenreByLabel(genreLabel);
		}


		public List<Genre> findGenresByMovie(int movieId)
		{
			return genreDao.findGenresByMovie(movieId);
		}


		public Genre createGenre(Genre genre)
		{
			return genreDao.createGenre(genre);
		}


		public Genre updateGenre(Genre genreToUpdate)
		{
			return genreDao.updateGenre(genreToUpdate);
		}


		public bool deleteGenre(Genre genreToDelete)
		{
			return genreDao.deleteGenre(genreToDelete);
		}

		#endregion Genres

		#region Movies

		public List<Movie> findAllMovies()
		{
			return movieDao.findAllMovies();
		}


		public Movie findMovieById(int movieId)
		{
			return movieDao.findMovieById(movieId);
		}


		public Movie findMovieByTitle(string movieTitle)
		{
			return movieDao.findMovieByTitle(movieTitle);
		}


		public List<Movie> findMoviesByGenre(string genreLabel)
		{
			return movieDao.findMoviesByGenre(genreLabel);
		}


		public Movie createMovie(Movie movieToCreate)
		{
			return movieDao.createMovie(movieToCreate);
		}


		public Movie updateMovie(Movie movieToUpdate)
		{
			return movieDao.updateMovie(movieToUpdate);
		}


		public bool deleteMovie(Movie movieToDelete)
		{
			return movieDao.deleteMovie(movieToDelete);
		}

		#endregion Movies

		#region Reviews

		public List<Review> findAllReviews()
		{
			return reviewDao.findAllReviews();
		}


		public Review findReviewById(int reviewId)
		{
			return reviewDao.findReviewById(reviewId);
		}


		public List<Review> findReviewsByMovie(int movieId)
		{
			return reviewDao.findReviewsByMovie(movieId);
		}


		public decimal findAverageScoreByMovie(int movieId)
		{
			return reviewDao.findAverageScoreByMovie(movieId);
		}


		public Review createReview(Review reviewToCreate)
		{
			return reviewDao.createReview(reviewToCreate);
		}


		public Review updateReview(Review reviewToUpdate)
		{
			return reviewDao.updateReview(reviewToUpdate);
		}


		public bool deleteReview(Review reviewToDelete)
		{
			return reviewDao.deleteReview(reviewToDelete);
		}


		public bool userHasReviewedMovie(User user, Movie movie)
		{
			return reviewDao.userHasReviewedMovie(user, movie);
		}

		#endregion Reviews

		#region Users

		public List<User> findAllUsers()
		{
			return userDao.findAllUsers();
		}


		public User findUserById(int userId)
		{
			return userDao.findUserById(userId);
		}


		public User findUserByEmail(string userEmail)
		{
			return userDao.findUserByEmail(userEmail);
		}


		public User findUserByReview(int reviewId)
		{
			return userDao.findUserByReview(reviewId);
		}


		public User createUser(User userToCreate)
		{
			return userDao.createUser(userToCreate);
		}


		public User updateUser(User userToUpdate)
		{
			return userDao.updateUser(userToUpdate);
		}


		public bool activateOrDeactivateUser(User user, bool isUserSupposedToBeActive)
		{
			return userDao.activateOrDeactivateUser(user, isUserSupposedToBeActive);
		}


		public User authenticate(String userEmail, String userPassword)
		{
			return userDao.authenticate(userEmail, userPassword);
		}

		#endregion Users

	}

}
