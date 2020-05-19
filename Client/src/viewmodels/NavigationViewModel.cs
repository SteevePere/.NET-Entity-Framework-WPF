using Client.src.views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;


namespace Client.src.viewmodels
{

	class NavigationViewModel : ViewModelBase
	{
		#region properties

		private object _currentView;
		private object _loginView;
		private object _registerView;
		private object _registerConfirmationView;
		private object _homeView;
		private object _addView;
		private object _viewView;
		private object _reviewView;
		private object _editView;
		private object _profileView;

		private string _windowState;

		public object CurrentView
		{
			get { return _currentView; }
			set
			{
				_currentView = value;
				RaisePropertyChanged("CurrentView");
			}
		}

		public string WindowState
		{
			get { return _windowState; }
			set
			{
				_windowState = value;
				RaisePropertyChanged();
			}
		}

		#endregion properties

		#region constructor

		public NavigationViewModel()
		{
			_loginView = new LoginView();
			_registerView = new RegisterView();
			_registerConfirmationView = new RegisterConfirmationView();
			_homeView = new HomeView();
			_addView = new AddView();
			_viewView = new ViewView();
			_reviewView = new ReviewView();
			_editView = new EditView();
			_profileView = new ProfileViewModel();

			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);

			WindowState = "Normal";
			CurrentView = _loginView;
		}

		#endregion constructor

		#region methods

		private void NotificationMessageReceived(NotificationMessage message)
		{
			switch (message.Notification)
			{
				case "ShowRegisterView":
					CurrentView = _registerView;
					break;

				case "ShowRegisterConfirmationView":
					CurrentView = _registerConfirmationView;
					break;

				case "ShowHomeView":
					WindowState = "Maximized";
					CurrentView = _homeView;
					break;

				case "ShowLoginView":
					WindowState = "Normal";
					CurrentView = _loginView;
					break;

				case "ShowAddView":
					CurrentView = _addView;
					break;

				case "ShowViewView":
					CurrentView = _viewView;
					break;

				case "ShowReviewView":
					CurrentView = _reviewView;
					break;

				case "ShowEditView":
					CurrentView = _editView;
					break;

				case "ShowProfileView":
					CurrentView = _profileView;
					break;
			}
		}

		#endregion methods
	}

}
