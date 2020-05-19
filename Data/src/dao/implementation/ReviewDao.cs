using Data.src.dao.interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Data.src.dao.implementation
{

	class ReviewDao : IReviewDao
	{
		
		// Returns List of Review objects or null
		public List<Review> findAllReviews()
		{
			using (var context = new MyMDB())
			{
				List<Review> reviews = context.Reviews.ToList();

				return reviews;
			}
		}


		// Returns a Review object or null
		public Review findReviewById(int reviewId)
		{
			using (var context = new MyMDB())
			{
				Review reviewFound = context.Reviews.Where(review => review.REV_ID == reviewId).FirstOrDefault<Review>();

				return reviewFound;
			}
		}


		// Returns List of Review objects or null
		public List<Review> findReviewsByMovie(int movieId)
		{
			using (var context = new MyMDB())
			{
				List<Review> reviews = context.Reviews.Where(review => review.Movie.MOV_ID == movieId).ToList();

				return reviews;
			}
		}


		// Returns decimal
		public decimal findAverageScoreByMovie(int movieId)
		{
			using (var context = new MyMDB())
			{
				int totalScore = 0;
				decimal averageScore = 0;

				List<Review> reviews = context.Reviews.Where(review => review.Movie.MOV_ID == movieId).ToList();

				if (reviews.Count > 0)
				{
					foreach (var review in reviews)
					{
						totalScore += review.REV_RATING;
					}

					averageScore = (decimal) totalScore / reviews.Count;
				}

				return averageScore;
			}
		}


		// Returns a Review object or null
		public Review createReview(Review review)
		{
			using (var context = new MyMDB())
			{
				if (!isReviewValid(review) || !_isReviewUniqueForMovieAndUser(review, context))
				{
					review = null;
				}

				else
				{
					Movie reviewMovie = context.Movies.SingleOrDefault(movie => movie.MOV_ID == review.Movie.MOV_ID);
					User reviewUser = context.Users.SingleOrDefault(user => user.USR_ID == review.User.USR_ID);

					review.Movie = reviewMovie;
					review.User = reviewUser;

					context.Reviews.Add(review);
					context.SaveChanges();
				}
			}

			return review;
		}


		// Returns a Review object or null
		public Review updateReview(Review updatedReview)
		{
			using (var context = new MyMDB())
			{
				Review reviewToUpdate = context.Reviews.SingleOrDefault(review => review.REV_ID == updatedReview.REV_ID);

				if (reviewToUpdate != null && updatedReview.REV_RATING > 0)
				{
					reviewToUpdate.REV_RATING = updatedReview.REV_RATING;
					reviewToUpdate.REV_CONTENT = updatedReview.REV_CONTENT;
					context.SaveChanges();
				}

				else updatedReview = null;
			}

			return updatedReview;
		}


		// Returns bool (true if deleted)
		public bool deleteReview(Review review)
		{
			bool deleted = false;

			if (review is object)
			{
				using (var context = new MyMDB())
				{
					Review reviewToDelete = context.Reviews.SingleOrDefault(dbReview => dbReview.REV_ID == review.REV_ID);

					if (reviewToDelete is object)
					{
						context.Entry(reviewToDelete).State = EntityState.Deleted;
						context.SaveChanges();

						deleted = true;
					}
				}
			}

			return deleted;
		}


		// Returns bool (true if user has already reviewed movie)
		public bool userHasReviewedMovie(User user, Movie movie)
		{
			bool userHasReviewedMovie = false;

			if (movie is object && user is object)
			{
				using (var context = new MyMDB())
				{
					if (context.Reviews.Where(
						dbReview => dbReview.Movie.MOV_ID == movie.MOV_ID &&
						dbReview.User.USR_ID == user.USR_ID
					).Any())
					{
						userHasReviewedMovie = true;
					}
				}
			}

			return userHasReviewedMovie;
		}

		// --- Helper methods --- //


		// Returns bool (true if review is valid)
		private bool isReviewValid(Review review)
		{
			bool isValidReview = false;

			if (review.REV_RATING > 0 && review.User is User && review.Movie is Movie)
			{
				isValidReview = true;
			}

			return isValidReview;
		}


		// Returns bool (true if review is unique for this user and movie)
		// Ensures that only one review per user per movie may be created
		private bool _isReviewUniqueForMovieAndUser(Review review, MyMDB context)
		{
			bool isUniqueForMovieAndUser = false;

			if (!context.Reviews.Where(
				dbReview => dbReview.Movie.MOV_ID == review.Movie.MOV_ID &&
				dbReview.User.USR_ID == review.User.USR_ID
				).Any())
			{
				isUniqueForMovieAndUser = true;
			}

			return isUniqueForMovieAndUser;
		}

	}

}
