using Data;


namespace Client.src.helpers
{

	public static class SessionManager
	{

		private static User _currentUser;


		static SessionManager()
		{
			_currentUser = null;
		}


		public static User CurrentUser
		{
			get
			{
				return (_currentUser);
			}
			set
			{
				_currentUser = value;
			}
		}

	}

}
