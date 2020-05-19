using System;
using System.Collections.Generic;


namespace Data.src.dao.interfaces
{

    interface IDao<T>
    {

        List<T> findAll();


        T findById(int id);


        List<T> findByCriteria(String criteria, Object valueCriteria);


        T create(T t);


        T update(T t);


        Boolean delete(T t);

    }

}
