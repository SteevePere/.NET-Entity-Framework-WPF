using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;


namespace Client.src.viewmodels
{

	public class RegisterConfirmationViewModel : ViewModelBase
	{
		#region properties

		public RelayCommand goToLoginCommand { get; }

		#endregion properties

		#region constructor

		public RegisterConfirmationViewModel()
		{
			goToLoginCommand = new RelayCommand(handleSubmit, true);
		}

		#endregion constructor

		#region methods

		private void handleSubmit()
		{
			goToLogin();
		}


		private void goToLogin()
		{
			Messenger.Default.Send(new NotificationMessage("ShowLoginView"));
		}

		#endregion methods
	}

}
