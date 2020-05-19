using Data.src.dao.interfaces;
using System;
using System.Collections.Generic;


namespace Data.src.facade
{

    public interface IServiceFacade
    {

		IGenreDao getGenreDao();


		IMovieDao getMovieDao();


		IReviewDao getReviewDao();


		IUserDao getUserDao();


		#region Genres

		List<Genre> findAllGenres();


		Genre findGenreById(int genreId);


		Genre findGenreByLabel(string genreLabel);


		List<Genre> findGenresByMovie(int movieId);


		Genre createGenre(Genre genre);


		Genre updateGenre(Genre genreToUpdate);


		bool deleteGenre(Genre genreToDelete);

		#endregion Genres

		#region Movies

		List<Movie> findAllMovies();


		Movie findMovieById(int movieId);


		Movie findMovieByTitle(string movieTitle);


		List<Movie> findMoviesByGenre(string genreLabel);


		Movie createMovie(Movie movieToCreate);


		Movie updateMovie(Movie movieToUpdate);


		bool deleteMovie(Movie movieToDelete);

		#endregion Movies

		#region Reviews

		List<Review> findAllReviews();


		Review findReviewById(int reviewId);


		List<Review> findReviewsByMovie(int movieId);


		decimal findAverageScoreByMovie(int movieId);


		Review createReview(Review reviewToCreate);


		Review updateReview(Review reviewToUpdate);


		bool deleteReview(Review reviewToDelete);


		bool userHasReviewedMovie(User user, Movie movie);

		#endregion Reviews

		#region Users

		List<User> findAllUsers();


		User findUserById(int userId);


		User findUserByEmail(string userEmail);


		User findUserByReview(int reviewId);


		User createUser(User userToCreate);


		User updateUser(User userToUpdate);


		bool activateOrDeactivateUser(User user, bool isUserSupposedToBeActive);


		User authenticate(String userEmail, String userPassword);

		#endregion Users
	}

}
