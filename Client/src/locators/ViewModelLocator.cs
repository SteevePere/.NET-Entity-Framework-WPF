using Client.src.viewmodels;


namespace Client.src.locators
{

	public class ViewModelLocator
	{
		public LoginViewModel loginViewModel { get; } = new LoginViewModel();
		public RegisterViewModel registerViewModel { get; } = new RegisterViewModel();
		public RegisterConfirmationViewModel registerConfirmationViewModel { get; } = new RegisterConfirmationViewModel();
		public HomeViewModel homeViewModel { get; } = new HomeViewModel();
		public ProfileViewModel profileViewModel { get; } = new ProfileViewModel();
		public AddViewModel addFormViewModel { get; } = new AddViewModel();
		public ViewViewModel viewFormViewModel { get; } = new ViewViewModel();
		public EditViewModel editFormViewModel { get; } = new EditViewModel();
		public ReviewViewModel addReviewFormViewModel { get; } = new ReviewViewModel();
	}
}
