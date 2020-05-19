using Client.src.helpers;
using Data;
using Data.src.facade;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;


namespace Client.src.viewmodels
{

	public class RegisterViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		private string _email;
		private string _password;
		private string _passwordConfirmation;
		private string _firstName;
		private string _lastName;
		private string _gender;
		private string _errorMessage;

		private bool _emailIsUnique { get; set; }

		public RelayCommand cancelCommand { get; }
		public RelayCommand registerCommand { get; }

		public string Email
		{
			get
			{
				return _email;
			}

			set
			{
				_email = value;
				RaisePropertyChanged();

				if (_email != "") { checkIfEmailIsUnique(); }
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}

			set
			{
				_password = value;
				RaisePropertyChanged();

				checkIfPasswordsMatch();
			}
		}

		public string PasswordConfirmation
		{
			get
			{
				return _passwordConfirmation;
			}

			set
			{
				_passwordConfirmation = value;
				RaisePropertyChanged();

				checkIfPasswordsMatch();
			}
		}

		public string FirstName
		{
			get
			{
				return _firstName;
			}

			set
			{
				_firstName = value;
				RaisePropertyChanged();
			}
		}

		public string LastName
		{
			get
			{
				return _lastName;
			}

			set
			{
				_lastName = value;
				RaisePropertyChanged();
			}
		}

		public string Gender
		{
			get
			{
				return _gender;
			}

			set
			{
				_gender = value;
				RaisePropertyChanged();
			}
		}

		public string ErrorMessage
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

		public RegisterViewModel()
		{
			_service = ServiceFacade.getInstance;

			registerCommand = new RelayCommand(handleRegisterSubmit, canHandleSubmit);
			cancelCommand = new RelayCommand(handleCancelSubmit, true);

			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);

			_emailIsUnique = true;
		}

		#endregion constructor

		#region methods

		private void NotificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "ShowRegisterView")
			{
				FirstName = "";
				LastName = "";
				Email = "";
				Password = "";
				PasswordConfirmation = "";
			}
		}


		void handleRegisterSubmit()
		{
			createUser();
		}


		void handleCancelSubmit()
		{
			Messenger.Default.Send(new NotificationMessage("ShowLoginView"));
		}


		bool canHandleSubmit()
		{
			return
				_emailIsUnique &&
				FirstName != null && FirstName != "" &&
				LastName != null && LastName != "" &&
				Email != null && Email != "" &&
				Password != null && Password != "" &&
				PasswordConfirmation != null && PasswordConfirmation != "" &&
				PasswordConfirmation == Password;
		}


		private void createUser()
		{
			DateTime now = DateTime.Now;

			User userToCreate = new User()
			{
				USR_FIRST_NAME = FirstName,
				USR_LAST_NAME = LastName,
				USR_EMAIL = Email,
				USR_PASSWORD = Hasher.getHash(Password),
				USR_GENDER = USR_GENDER.M,
				USR_ACTIVE = true,
				USR_CREATION_DATETIME = now,
				USR_EDIT_DATETIME = now 
			};

			User createdUser = _service.createUser(userToCreate);

			if (createdUser == null)
			{
				ErrorMessage = "Could not create Account";
			}

			else
			{
				Messenger.Default.Send(new NotificationMessage("ShowRegisterConfirmationView"));
			}
		}


		private void checkIfPasswordsMatch()
		{
			if (PasswordConfirmation == Password)
			{
				ErrorMessage = "";
			}

			else
			{
				ErrorMessage = "Passwords don't match";
			}
		}


		private void checkIfEmailIsUnique()
		{
			if (_service.findUserByEmail(Email) != null)
			{
				_emailIsUnique = false;
				ErrorMessage = "This e-mail already exists";
			}

			else
			{
				_emailIsUnique = true;
				ErrorMessage = "";
			}
		}

		#endregion methods
	}

}
