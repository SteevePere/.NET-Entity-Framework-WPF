using Data.src.dao.interfaces;
using System;
using System.Collections.Generic;


namespace Data.src.dao
{

	class AbstractDao<T> : IDao<T>
	{

		List<T> IDao<T>.findAll()
		{
			return null;
		}


		T IDao<T>.findById(int id)
		{
			return default(T);
		}


		List<T> IDao<T>.findByCriteria(string criteria, object valueCriteria)
		{
			return null;
		}


		T IDao<T>.create(T t)
		{
			return default(T);
		}


		T IDao<T>.update(T t)
		{
			return default(T);
		}


		Boolean IDao<T>.delete(T t)
		{
			return false;
		}

	}

}
