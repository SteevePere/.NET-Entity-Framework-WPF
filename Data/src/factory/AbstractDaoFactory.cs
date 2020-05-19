using Data.src.dao.interfaces;


namespace Data.src.factory
{

    public abstract class AbstractDaoFactory
    {

		internal abstract IGenreDao getGenreDao();
		internal abstract IMovieDao getMovieDao();
		internal abstract IReviewDao getReviewDao();
		internal abstract IUserDao getUserDao();


		public enum DaoFactoryType
		{
			MSSQL_DAO_FACTORY
		}


		public static AbstractDaoFactory getFactory(DaoFactoryType daoFactoryType)
		{

			AbstractDaoFactory factory = null;

			switch (daoFactoryType)
			{

				case DaoFactoryType.MSSQL_DAO_FACTORY:

					factory = new DaoFactory();
					break;
			}

			return factory;
		}

	}

}
