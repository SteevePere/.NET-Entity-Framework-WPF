using System;
using System.Collections.Generic;


namespace Data.src.dao.interfaces
{

    public interface IGenreDao
    {

        List<Genre> findAllGenres();


        Genre findGenreById(int genreId);


        Genre findGenreByLabel(String genreLabel);


		List<Genre> findGenresByMovie(int movieId);


		Genre createGenre(Genre genre);


        Genre updateGenre(Genre genre);


        Boolean deleteGenre(Genre genre);

    }

}
