using System;
using System.Collections.Generic;


namespace Data.src.dao.interfaces
{

    public interface IReviewDao
    {

        List<Review> findAllReviews();


        Review findReviewById(int reviewId);


		List<Review> findReviewsByMovie(int movieId);


		decimal findAverageScoreByMovie(int movieId);


		Review createReview(Review review);


        Review updateReview(Review review);


        Boolean deleteReview(Review review);


		bool userHasReviewedMovie(User user, Movie movie);

	}

}
