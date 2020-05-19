using System;
using System.Collections.Generic;


namespace Data.src.dao.interfaces
{

    public interface IUserDao
    {

        List<User> findAllUsers();


        User findUserById(int userId);


        User findUserByEmail(String userEmail);


		User findUserByReview(int reviewId);


		User createUser(User user);


        User updateUser(User user);


        Boolean activateOrDeactivateUser(User user, bool isUserSupposedToBeActive);


		User authenticate(String userEmail, String userPassword);

    }

}
