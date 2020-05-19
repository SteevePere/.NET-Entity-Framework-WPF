using Data.src.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Data.src.dao.implementation
{

	class UserDao : IUserDao
	{

		// Returns List of User objects or null
		public List<User> findAllUsers()
		{
			using (var context = new MyMDB())
			{
				List<User> users = context.Users.ToList();

				return users;
			}
		}


		// Returns User object or null
		public User findUserById(int userId)
		{
			using (var context = new MyMDB())
			{
				User userFound = context.Users.Where(user => user.USR_ID == userId).FirstOrDefault<User>();

				return userFound;
			}
		}


		// Returns User object or null
		public User findUserByEmail(string userEmail)
		{
			using (var context = new MyMDB())
			{
				User userFound = context.Users.Where(user => user.USR_EMAIL == userEmail).FirstOrDefault<User>();

				return userFound;
			}
		}


		// Returns User object or null
		public User findUserByReview(int reviewId)
		{
			using (var context = new MyMDB())
			{
				Review reviewFound = context.Reviews.Where(review => review.REV_ID == reviewId).Include(dbReview => dbReview.User).FirstOrDefault<Review>();

				return reviewFound.User;
			}
		}


		// Returns User object or null
		public User createUser(User user)
		{
			using (var context = new MyMDB())
			{
				if (isUserValid(user)) 
				{
					user.USR_ACTIVE = true;
					user.USR_CREATION_DATETIME = DateTime.Now;
					user.USR_EDIT_DATETIME = DateTime.Now;
					user.USR_LAST_CONNECTION_DATETIME = null;

					context.Users.Add(user);
					context.SaveChanges();
				}

				else user = null;
			}

			return user;
		}


		// Returns User object or null
		public User updateUser(User updatedUser)
		{
			using (var context = new MyMDB())
			{
				var userToUpdate = context.Users.SingleOrDefault(user => user.USR_ID == updatedUser.USR_ID);

				if (userToUpdate != null && isUserValid(updatedUser))
				{
					userToUpdate.USR_EMAIL = updatedUser.USR_EMAIL;
					userToUpdate.USR_PASSWORD = updatedUser.USR_PASSWORD;
					userToUpdate.USR_FIRST_NAME = updatedUser.USR_FIRST_NAME;
					userToUpdate.USR_LAST_NAME = updatedUser.USR_LAST_NAME;
					userToUpdate.USR_GENDER = updatedUser.USR_GENDER;
					userToUpdate.USR_LAST_CONNECTION_DATETIME = updatedUser.USR_LAST_CONNECTION_DATETIME;
					userToUpdate.USR_EDIT_DATETIME = DateTime.Now;

					context.SaveChanges();
				}

				else updatedUser = null;
			}

			return updatedUser;
		}


		// Returns bool (true if user has been activated or deactivated)
		public bool activateOrDeactivateUser(User user, bool isUserSupposedToBeActive)
		{
			bool activatedOrDeactivated = false;

			if (user is object)
			{
				using (var context = new MyMDB())
				{
					var userToDelete = context.Users.SingleOrDefault(dbUser => dbUser.USR_ID == user.USR_ID);

					if (userToDelete is object)
					{
						userToDelete.USR_ACTIVE = isUserSupposedToBeActive;
						userToDelete.USR_EDIT_DATETIME = DateTime.Now;
						context.SaveChanges();

						activatedOrDeactivated = true;
					}
				}
			}

			return activatedOrDeactivated;
		}


		// Returns User object or null
		public User authenticate(String userEmail, String userPassword)
		{
			using (var context = new MyMDB())
			{
				User userFound = context.Users.Where(user => user.USR_EMAIL == userEmail && user.USR_PASSWORD == userPassword).FirstOrDefault<User>();

				return userFound;
			}
		}


		// --- Helper methods --- //


		// Returns bool (true if user is valid)
		private bool isUserValid(User user)
		{
			bool isValidUser = false;

			if (user.USR_EMAIL != null
				&& (findUserByEmail(user.USR_EMAIL) is null || findUserByEmail(user.USR_EMAIL).USR_ID == user.USR_ID) // ensures email unicity
				&& user.USR_FIRST_NAME != null
				&& user.USR_LAST_NAME != null
				&& user.USR_PASSWORD != null
				&& Enum.IsDefined(typeof(USR_GENDER), user.USR_GENDER))
			{
				isValidUser = true;
			}

			return isValidUser;
		}

	}

}
