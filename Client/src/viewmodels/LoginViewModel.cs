using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Data;
using Data.src.facade;
using GalaSoft.MvvmLight.Messaging;
using Client.src.helpers;


namespace Client.src.viewmodels
{

	public class LoginViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		private string _login;
		private string _password;
		private string _errorMessage;

		public RelayCommand loginCommand { get; }
		public RelayCommand registerCommand { get; }

		public string login
		{
			get
			{
				return _login;
			}

			set
			{
				_login = value;
				errorMessage = "";
				RaisePropertyChanged();
			}
		}


		public string password
		{
			get
			{
				return _password;
			}

			set
			{
				_password = value;
				errorMessage = "";
				RaisePropertyChanged();
			}
		}


		public string errorMessage
		{
			get
			{
				return _errorMessage;
			}

			set
			{
				_errorMessage = value;
				RaisePropertyChanged();
			}
		}

		#endregion properties

		#region constructor

		public LoginViewModel()
		{
			_service = ServiceFacade.getInstance;

			loginCommand = new RelayCommand(handleSubmit, canHandleSubmit);
			registerCommand = new RelayCommand(handleRegisterSubmit, true);

			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
		}

		#endregion constructor

		#region methods

		void handleSubmit()
		{
			User authenticatedUser = _service.authenticate(_login, Hasher.getHash(_password));

			if (authenticatedUser != null)
			{
				SessionManager.CurrentUser = authenticatedUser;
				ShowHomeView();
				Messenger.Default.Send(new NotificationMessage("UserIsLogged"));
			}

			else
			{
				errorMessage = "Wrong Login or Password";
			}
		}


		bool canHandleSubmit()
		{
			return _login != null && _login.Length != 0 && _password != null && _password.Length != 0;
		}


		private void ShowHomeView()
		{
			Messenger.Default.Send(new NotificationMessage("ShowHomeView"));
		}


		void handleRegisterSubmit()
		{
			ShowRegisterView();
		}


		private void ShowRegisterView()
		{
			Messenger.Default.Send(new NotificationMessage("ShowRegisterView"));
		}


		private void NotificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "ShowLoginView")
			{
				login = "";
				password = "";
			}
		}

		#endregion methods
	}

}
