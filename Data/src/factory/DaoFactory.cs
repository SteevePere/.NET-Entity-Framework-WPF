using Data.src.dao.implementation;
using Data.src.dao.interfaces;


namespace Data.src.factory
{

	class DaoFactory : AbstractDaoFactory
	{

		internal override IGenreDao getGenreDao()
		{
			return new GenreDao();
		}


		internal override IMovieDao getMovieDao()
		{
			return new MovieDao();
		}


		internal override IReviewDao getReviewDao()
		{
			return new ReviewDao();
		}


		internal override IUserDao getUserDao()
		{
			return new UserDao();
		}

	}

}
