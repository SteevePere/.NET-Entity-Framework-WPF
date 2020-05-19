using System;
using System.Collections.Generic;


namespace Data.src.dao.interfaces
{

    public interface IMovieDao
    {

        List<Movie> findAllMovies();


        Movie findMovieById(int movieId);


        Movie findMovieByTitle(String movieTitle);


		List<Movie> findMoviesByGenre(String genreLabel);


		Movie createMovie(Movie movie);


        Movie updateMovie(Movie movie);


        Boolean deleteMovie(Movie movie);

    }

}
