using Client.src.helpers;
using Data;
using Data.src.facade;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;


namespace Client.src.viewmodels
{

    public class AddViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		private string _title;
		private string _synopsis;
		private string _selectedGenre;
		private string _message;
		private string _messageColor;

		private Visibility _successMessageVisibility;
		private Visibility _errorMessageVisibility;

		public ObservableCollection<Object> AllGenres { get; set; } = new ObservableCollection<Object>();

		public RelayCommand showHomeCommand { get; }
		public RelayCommand addMovieCommand { get; }
		public RelayCommand SignOutCommand { get; }
		public RelayCommand ShowProfileCommand { get; }

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
			get{
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

		public AddViewModel()
		{
			SuccessMessageVisibility = Visibility.Hidden;
			ErrorMessageVisibility = Visibility.Hidden;

			_service = ServiceFacade.getInstance;

			addMovieCommand = new RelayCommand(handleSubmitAsync, canHandleSubmit);
			showHomeCommand = new RelayCommand(handleShowHome, true);
			SignOutCommand = new RelayCommand(handleSignOut, true);
			ShowProfileCommand = new RelayCommand(handleShowProfile, true);

			getGenres();
		}

		#endregion constructor

		#region methods

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

			Movie movieToCreate = new Movie
			{
				MOV_TITLE = _title,
				MOV_SYNOPSIS = _synopsis,
				Genres = movieGenres
			};

			Movie movieCreated = _service.createMovie(movieToCreate);

			if (movieCreated == null)
			{
				ErrorMessageVisibility = Visibility.Visible;
				Message = "Could not create Movie. Please make sure that the Title is unique.";
				MessageColor = "#ee2a7b";
			}

			else
			{
				SuccessMessageVisibility = Visibility.Visible;
				Message = "Movie has been saved!";
				MessageColor = "MediumSeaGreen";

				await Task.Delay(2000);

				SuccessMessageVisibility = Visibility.Hidden;
				resetFormValues();
			}
		}


		bool canHandleSubmit()
		{
			return _title != null && _title.Length != 0 && _synopsis != null && _synopsis.Length != 0 && _selectedGenre != null && _selectedGenre.Length != 0;
		}


		void handleShowHome()
		{
			showHome();
		}


		private void showHome()
		{
			Messenger.Default.Send(new NotificationMessage("ReloadMovies"));
			Messenger.Default.Send(new NotificationMessage("ShowHomeView"));

			resetFormValues();
		}


		private void handleSignOut()
		{
			Messenger.Default.Send(new NotificationMessage("ShowLoginView"));
			SessionManager.CurrentUser = null;

			resetFormValues();
		}


		private void handleShowProfile()
		{
			Messenger.Default.Send(new NotificationMessage("ShowProfileView"));

			resetFormValues();
		}


		private void resetFormValues()
		{
			Title = "";
			Synopsis = "";
			SelectedGenre = "";
		}

		#endregion methods
	}

}
