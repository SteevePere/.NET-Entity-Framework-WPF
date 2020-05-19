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


namespace Client.src.viewmodels
{

	public class HomeViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		private string  _userName;
		private string _nameFilter = null;
		private string _genreFilter = null;

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = "Hello, " + value;
				RaisePropertyChanged();
			}
		}

		public string NameFilter
		{
			get { return _nameFilter; }
			set
			{
				_nameFilter = value;
				RaisePropertyChanged();

				getMovies(_genreFilter, _nameFilter);
			}
		}

		public string GenreFilter
		{
			get { return _genreFilter; }
			set
			{
				_genreFilter = value;
				RaisePropertyChanged();

				getMovies(_genreFilter, _nameFilter);
			}
		}

		public string SelectedGenre
		{
			get
			{
				return _genreFilter;
			}

			set
			{
				_genreFilter = value;
				RaisePropertyChanged();

				getMovies(_genreFilter, _nameFilter);
			}
		}

		public RelayCommand LoadedCommand
		{
			get;
			private set;
		}

		public RelayCommand ShowAddFormCommand { get; }
		public RelayCommand<int> ViewCommand { get; }
		public RelayCommand<int> EditCommand { get; }
		public RelayCommand<int> DeleteCommand { get; }
		public RelayCommand SignOutCommand { get; }
		public RelayCommand ShowProfileCommand { get; }

		public ObservableCollection<Object> MovieGrid { get; set; } = new ObservableCollection<Object>();
		public ObservableCollection<Object> AllGenres { get; set; } = new ObservableCollection<Object>();

		#endregion properties

		#region constructor

		public HomeViewModel()
		{
			_service = ServiceFacade.getInstance;

			getMovies();
			getGenres();

			LoadedCommand = new RelayCommand(() =>
			{
				UserName = SessionManager.CurrentUser.USR_FIRST_NAME;
			});

			ShowAddFormCommand = new RelayCommand(handleAddForm, canHandleAddForm);
			ViewCommand = new RelayCommand<int>(handleView, true);
			EditCommand = new RelayCommand<int>(handleEdit, true);
			DeleteCommand = new RelayCommand<int>(handleDelete, true);
			SignOutCommand = new RelayCommand(handleSignOut, true);
			ShowProfileCommand = new RelayCommand(handleShowProfile, true);

			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
		}

		#endregion constructor

		#region methods

		private void getMovies(string genreFilter = null, string nameFilter = null)
		{
			MovieGrid.Clear();

			List<Movie> allMovies = _service.findAllMovies();

			if (genreFilter != null && genreFilter != "")
			{
				allMovies = _service.findMoviesByGenre(genreFilter);
			}

			if (nameFilter != null && nameFilter != "")
			{
				allMovies = allMovies.Where(movie => movie.MOV_TITLE.ToLower().Contains(nameFilter.ToLower())).ToList();
			}

			foreach (var movie in allMovies)
			{
				List<Genre> movieGenres = _service.findGenresByMovie(movie.MOV_ID);

				decimal movieAverageScore = Math.Round(_service.findAverageScoreByMovie(movie.MOV_ID), 1);
				int movieTotalReviews = _service.findReviewsByMovie(movie.MOV_ID).Count;

				string movieGenresAsString = String.Concat(movieGenres.Select(genre => genre.GNR_LABEL + ", "));
				movieGenresAsString = movieGenresAsString.TrimEnd(' ');
				movieGenresAsString = movieGenresAsString.TrimEnd(',');

				var gridMovie = new {
					MOV_ID = movie.MOV_ID,
					MOV_TITLE = movie.MOV_TITLE,
					MOV_SYNOPSIS = movie.MOV_SYNOPSIS,
					MOV_GENRES = movieGenresAsString,
					MOV_RATING = movieAverageScore,
					MOV_REVIEWS = movieTotalReviews,
				};

				MovieGrid.Add(gridMovie);
			}
		}


		private void getGenres()
		{
			List<Genre> allGenres = _service.findAllGenres();

			AllGenres.Add("");

			foreach (var genre in allGenres)
			{
				var comboBoxGenre = genre.GNR_LABEL;

				AllGenres.Add(comboBoxGenre);
			}
		}


		private void handleAddForm()
		{
			showAddForm();
		}


		private bool canHandleAddForm()
		{
			return true;
		}


		private void showAddForm()
		{
			Messenger.Default.Send(new NotificationMessage("ShowAddView"));
		}


		private void handleDelete(int movieId)
		{
			deleteMovie(movieId);
		}


		private void deleteMovie(int movieId)
		{
			Movie movieToDelete = _service.findMovieById(movieId);
			bool movieHasBeenDeleted = _service.deleteMovie(movieToDelete);

			if (movieHasBeenDeleted)
			{
				getMovies();
			}
		}


		private void handleView(int movieId)
		{
			Movie movieToView = _service.findMovieById(movieId);

			if (movieToView != null)
			{
				Messenger.Default.Send(new NotificationMessage(movieToView, "ShowViewView"));
			}
		}


		private void handleEdit(int movieId)
		{
			Movie movieToEdit = _service.findMovieById(movieId);

			if (movieToEdit != null)
			{
				Messenger.Default.Send(new NotificationMessage(movieToEdit, "ShowEditView"));
			}
		}


		private void handleShowProfile()
		{
			Messenger.Default.Send(new NotificationMessage("ShowProfileView"));
		}


		private void handleSignOut()
		{
			Messenger.Default.Send(new NotificationMessage("ShowLoginView"));
			SessionManager.CurrentUser = null;
		}


		private void NotificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "ReloadMovies")
			{
				NameFilter = "";
				getMovies();
			}
		}

		#endregion methods
	}

}
