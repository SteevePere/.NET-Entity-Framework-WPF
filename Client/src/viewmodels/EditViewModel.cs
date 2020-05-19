using Client.src.helpers;
using Data;
using Data.src.facade;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace Client.src.viewmodels
{

    public class EditViewModel : ViewModelBase
    {
		#region properties

		private IServiceFacade _service = null;

		public Movie _movieToEdit;

		private string _title;
		private string _synopsis;
		private string _selectedGenre;
		private string _message;
		private string _messageColor;

		private Visibility _successMessageVisibility;
		private Visibility _errorMessageVisibility;

		public ObservableCollection<Object> AllGenres { get; set; } = new ObservableCollection<Object>();

		public RelayCommand showHomeCommand { get; }
		public RelayCommand editMovieCommand { get; }
		public RelayCommand SignOutCommand { get; }
		public RelayCommand ShowProfileCommand { get; }

		public Movie MovieToEdit
		{
			get { return _movieToEdit; }
			set
			{
				_movieToEdit = value;
				Title = _movieToEdit.MOV_TITLE;
				Synopsis = _movieToEdit.MOV_SYNOPSIS;
				SelectedGenre = _service.findGenresByMovie(_movieToEdit.MOV_ID).FirstOrDefault().GNR_LABEL;
				RaisePropertyChanged();
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}

			set
			{
				_title = value;
				Message = "";
				ErrorMessageVisibility = Visibility.Hidden;

				RaisePropertyChanged();
			}
		}

		public string Synopsis
		{
			get
			{
				return _synopsis;
			}

			set
			{
				_synopsis = value;
				RaisePropertyChanged();
			}
		}

		public string SelectedGenre
		{
			get
			{
				return _selectedGenre;
			}

			set
			{
				_selectedGenre = value;
				RaisePropertyChanged();
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}

			set
			{
				_message = value;
				RaisePropertyChanged();
			}
		}

		public string MessageColor
		{
			get
			{
				return _messageColor;
			}

			set
			{
				_messageColor = value;
				RaisePropertyChanged();
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

		public Visibility ErrorMessageVisibility
		{
			get { return _errorMessageVisibility; }
			set
			{
				_errorMessageVisibility = value;
				RaisePropertyChanged();
			}
		}

		#endregion properties

		#region constructor

		public EditViewModel()
		{
			SuccessMessageVisibility = Visibility.Hidden;
			ErrorMessageVisibility = Visibility.Hidden;

			_service = ServiceFacade.getInstance;

			showHomeCommand = new RelayCommand(handleShowHome, true);
			editMovieCommand = new RelayCommand(handleSubmitAsync, canHandleSubmit);
			SignOutCommand = new RelayCommand(handleSignOut, true);
			ShowProfileCommand = new RelayCommand(handleShowProfile, true);

			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);

			getGenres();
		}

		#endregion constructor

		#region methods

		private void NotificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "ShowEditView")
			{
				MovieToEdit = (Movie)message.Sender;
			}
		}


		public void getGenres()
		{
			List<Genre> allGenres = _service.findAllGenres();

			AllGenres.Add("");

			foreach (var genre in allGenres)
			{
				var comboBoxGenre = genre.GNR_LABEL;

				AllGenres.Add(comboBoxGenre);
			}
		}


		async void handleSubmitAsync()
		{
			Genre movieGenre = _service.findGenreByLabel(_selectedGenre);
			List<Genre> movieGenres = new List<Genre>
			{
				movieGenre
			};

			Movie movieToEdit = _service.findMovieById(MovieToEdit.MOV_ID);

			movieToEdit.MOV_TITLE = Title;
			movieToEdit.MOV_SYNOPSIS = Synopsis;
			movieToEdit.Genres = movieGenres;

			Movie movieEdited = _service.updateMovie(movieToEdit);

			if (movieEdited == null)
			{
				ErrorMessageVisibility = Visibility.Visible;
				Message = "Could not update Movie. Please make sure that the Title is unique.";
				MessageColor = "#ee2a7b";
			}

			else
			{
				SuccessMessageVisibility = Visibility.Visible;
				Message = "Movie has been updated!";
				MessageColor = "MediumSeaGreen";

				await Task.Delay(2000);

				SuccessMessageVisibility = Visibility.Hidden;
				Message = "";
			}
		}


		bool canHandleSubmit()
		{
			return _title != null && _title.Length != 0 && _synopsis != null && _synopsis.Length != 0 && _selectedGenre != null && _selectedGenre.Length != 0;
		}


		void handleShowHome()
		{
			ShowHome();
		}


		private void ShowHome()
		{
			Messenger.Default.Send(new NotificationMessage("ReloadMovies"));
			Messenger.Default.Send(new NotificationMessage("ShowHomeView"));
		}


		private void handleSignOut()
		{
			Messenger.Default.Send(new NotificationMessage("ShowLoginView"));
			SessionManager.CurrentUser = null;
		}


		private void handleShowProfile()
		{
			Messenger.Default.Send(new NotificationMessage("ShowProfileView"));
		}

		#endregion methods
	}

}
