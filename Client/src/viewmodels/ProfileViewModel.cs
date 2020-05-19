using Client.src.helpers;
using Data;
using Data.src.facade;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Windows;


namespace Client.src.viewmodels
{

	public class ProfileViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		private string _firstName;
		private string _lastName;
		private string _email;
		private string _newPassword;
		private string _newPasswordConfirmation;
		private string _currentPassword;
		private string _editProfileButtonContent;
		private string _firstNameErrorMessage;
		private string _lastNameErrorMessage;
		private string _emailErrorMessage;
		private string _newPasswordErrorMessage;
		private string _currentPasswordErrorMessage;

		private bool _userIsEditingProfile;
		private bool _valuesHaveChanged;
		private bool _emailIsUnique;

		private Visibility _successMessageVisibility;

		public RelayCommand ShowHomeCommand { get; }
		public RelayCommand SignOutCommand { get; }
		public RelayCommand EditProfileCommand { get; }

		public string FirstName
		{
			get { return _firstName; }
			set
			{
				_firstName = value;
				_valuesHaveChanged = true;

				RaisePropertyChanged();

				if (_firstName == "") { FirstNameErrorMessage = "First Name is required"; }
				else { FirstNameErrorMessage = ""; }
			}
		}

		public string LastName
		{
			get { return _lastName; }
			set
			{
				_lastName = value;
				_valuesHaveChanged = true;

				RaisePropertyChanged();

				if (_lastName == "") { LastNameErrorMessage = "Last Name is required"; }
				else { LastNameErrorMessage = ""; }
			}
		}

		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
				_valuesHaveChanged = true;

				RaisePropertyChanged();

				if (_email != "") { checkIfEmailIsUnique(); }
				else { EmailErrorMessage = "Email is required";  }
			}
		}

		public string NewPassword
		{
			get { return _newPassword; }
			set
			{
				_newPassword = value;
				_valuesHaveChanged = true;

				RaisePropertyChanged();
				checkIfPasswordsMatch();
				checkIfCurrentPasswordIsCorrect();
			}
		}

		public string NewPasswordConfirmation
		{
			get { return _newPasswordConfirmation; }
			set
			{
				_newPasswordConfirmation = value;
				_valuesHaveChanged = true;

				RaisePropertyChanged();
				checkIfPasswordsMatch();
			}
		}

		public string CurrentPassword
		{
			get { return _currentPassword; }
			set
			{
				_currentPassword = value;
				_valuesHaveChanged = true;

				RaisePropertyChanged();
				checkIfCurrentPasswordIsCorrect();
			}
		}

		public string EditProfileButtonContent
		{
			get { return _editProfileButtonContent; }
			set
			{
				_editProfileButtonContent = value;
				RaisePropertyChanged();
			}
		}

		public string FirstNameErrorMessage
		{
			get { return _firstNameErrorMessage; }
			set
			{
				_firstNameErrorMessage = value;
				RaisePropertyChanged();
			}
		}

		public string LastNameErrorMessage
		{
			get { return _lastNameErrorMessage; }
			set
			{
				_lastNameErrorMessage = value;
				RaisePropertyChanged();
			}
		}

		public string EmailErrorMessage
		{
			get { return _emailErrorMessage; }
			set
			{
				_emailErrorMessage = value;
				RaisePropertyChanged();
			}
		}

		public string NewPasswordErrorMessage
		{
			get { return _newPasswordErrorMessage; }
			set
			{
				_newPasswordErrorMessage = value;
				RaisePropertyChanged();
			}
		}

		public string CurrentPasswordErrorMessage
		{
			get { return _currentPasswordErrorMessage; }
			set
			{
				_currentPasswordErrorMessage = value;
				RaisePropertyChanged();
			}
		}

		public bool UserIsEditingProfile
		{
			get { return _userIsEditingProfile; }
			set
			{
				_userIsEditingProfile = value;
				RaisePropertyChanged();

				EditProfileButtonContent = _userIsEditingProfile ? "Save Changes" : "Edit Profile";
			}
		}

		public Visibility SuccessMessageVisibility
		{
			get { return _successMessageVisibility; }
			set
			{
				_successMessageVisibility = value;
				RaisePropertyChanged();
			}
		}

		#endregion properties

		#region constructor

		public ProfileViewModel()
		{
			_valuesHaveChanged = false;
			_emailIsUnique = true;
			UserIsEditingProfile = false;
			SuccessMessageVisibility = Visibility.Hidden;

			_service = ServiceFacade.getInstance;

			ShowHomeCommand = new RelayCommand(handleShowHome, true);
			SignOutCommand = new RelayCommand(handleSignOut, true);
			EditProfileCommand = new RelayCommand(handleEditProfileAsync, canEditProfile);

			Messenger.Default.Register<NotificationMessage>(this, notificationMessageReceived);
		}

		#endregion constructor

		#region methods

		private void notificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "UserIsLogged")
			{
				FirstName = SessionManager.CurrentUser.USR_FIRST_NAME;
				LastName = SessionManager.CurrentUser.USR_LAST_NAME;
				Email = SessionManager.CurrentUser.USR_EMAIL;

				UserIsEditingProfile = false;
				_valuesHaveChanged = false;
			}
		}


		private void handleShowHome()
		{
			resetForm();
			showHome();
		}


		private void handleSignOut()
		{
			Messenger.Default.Send(new NotificationMessage("ShowLoginView"));
			SessionManager.CurrentUser = null;
		}


		private void resetForm()
		{
			FirstName = SessionManager.CurrentUser.USR_FIRST_NAME;
			LastName = SessionManager.CurrentUser.USR_LAST_NAME;
			Email = SessionManager.CurrentUser.USR_EMAIL;

			NewPassword = "";
			NewPasswordConfirmation = "";
			CurrentPassword = "";

			UserIsEditingProfile = false;
			_valuesHaveChanged = false;
		}


		private void showHome()
		{
			Messenger.Default.Send(new NotificationMessage("ShowHomeView"));
		}


		private async void handleEditProfileAsync()
		{
			if (!UserIsEditingProfile) // first click, user wants to edit
			{
				UserIsEditingProfile = true;
			}

			else // second click, user is editing profile and wants to save
			{
				if (_valuesHaveChanged)
				{
					User userToEdit = new User()
					{
						USR_ID = SessionManager.CurrentUser.USR_ID,
						USR_FIRST_NAME = FirstName,
						USR_LAST_NAME = LastName,
						USR_EMAIL = Email,
						USR_PASSWORD = NewPassword == "" || NewPassword is null ? 
							SessionManager.CurrentUser.USR_PASSWORD : Hasher.getHash(NewPassword),
						USR_GENDER = SessionManager.CurrentUser.USR_GENDER
					};

					User updatedUser = _service.updateUser(userToEdit);

					if (updatedUser != null) // success
					{
						NewPassword = "";
						NewPasswordConfirmation = "";
						CurrentPassword = "";

						_valuesHaveChanged = false;
						UserIsEditingProfile = false;

						SessionManager.CurrentUser.USR_FIRST_NAME = userToEdit.USR_FIRST_NAME;
						SessionManager.CurrentUser.USR_LAST_NAME = userToEdit.USR_LAST_NAME;
						SessionManager.CurrentUser.USR_EMAIL = userToEdit.USR_EMAIL;
						SessionManager.CurrentUser.USR_PASSWORD = userToEdit.USR_PASSWORD;

						SuccessMessageVisibility = Visibility.Visible;
						await Task.Delay(2000);
						SuccessMessageVisibility = Visibility.Hidden;
					}
				}

				else
				{
					UserIsEditingProfile = false;
				}
			}
		}


		private bool canEditProfile()
		{
			bool userCanEditProfile = false;

			if (!UserIsEditingProfile)
			{
				userCanEditProfile = true;
			}

			else
			{
				userCanEditProfile =
					FirstName != null && FirstName != "" &&
					LastName != null && LastName != "" &&
					Email != null && Email != "" &&
					(NewPassword == null || 
					NewPassword == "" || 
					NewPassword == NewPasswordConfirmation && Hasher.getHash(CurrentPassword) == SessionManager.CurrentUser.USR_PASSWORD) &&
					_emailIsUnique;
			}

			return userCanEditProfile;
		}


		private void checkIfEmailIsUnique()
		{
			User userWithSameEmail = _service.findUserByEmail(Email);

			if (userWithSameEmail != null && userWithSameEmail.USR_ID != SessionManager.CurrentUser.USR_ID)
			{
				_emailIsUnique = false;
				EmailErrorMessage = "This e-mail already exists";
			}

			else
			{
				_emailIsUnique = true;
				EmailErrorMessage = "";
			}
		}


		private void checkIfPasswordsMatch()
		{
			if (NewPasswordConfirmation == NewPassword)
			{
				NewPasswordErrorMessage = "";
			}

			else
			{
				NewPasswordErrorMessage = "Passwords don't match";
			}
		}


		private void checkIfCurrentPasswordIsCorrect()
		{
			if (NewPassword != "" && CurrentPassword == "")
			{
				CurrentPasswordErrorMessage = "Current Password is required";
			}

			else if (Hasher.getHash(CurrentPassword) != SessionManager.CurrentUser.USR_PASSWORD && NewPassword != "")
			{
				CurrentPasswordErrorMessage = "Password is incorrect";
			}

			else
			{
				CurrentPasswordErrorMessage = "";
			}
		}

		#endregion methods
	}

}
