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

	public class ViewViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		public Movie _movieToView;
		public string _movieGenre;
		private decimal _averageScore;

		public ObservableCollection<Object> ReviewGrid { get; set; } = new ObservableCollection<Object>();

		public RelayCommand showHomeCommand { get; }
		public RelayCommand SignOutCommand { get; }
		public RelayCommand showAddReviewCommand { get; }
		public RelayCommand ShowProfileCommand { get; }

		public Movie MovieToView
		{
			get { return _movieToView; }
			set
			{
				_movieToView = value;
				getAverageScore(_movieToView);
				getMovieReviews(_movieToView);
				getMovieGenre(_movieToView);
				RaisePropertyChanged();
			}
		}

		public string MovieGenre
		{
			get { return _movieGenre; }
			set
			{
				_movieGenre = value;
				RaisePropertyChanged();
			}
		}

		public decimal AverageScore
		{
			get { return _averageScore;  }
			set
			{
				_averageScore = value;
				RaisePropertyChanged();
			}
		}

		#endregion properties

		#region constructor

		public ViewViewModel()
		{
			_service = ServiceFacade.getInstance;

			showHomeCommand = new RelayCommand(handleShowHome, true);
			SignOutCommand = new RelayCommand(handleSignOut, true);
			showAddReviewCommand = new RelayCommand(handleSubmit, canHandleSubmit);
			ShowProfileCommand = new RelayCommand(handleShowProfile, true);

			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
		}

		#endregion constructor

		#region methods

		private void NotificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "ShowViewView")
			{
				MovieToView = (Movie)message.Sender;
			}

			else if (message.Notification == "RefreshReviews")
			{
				getMovieReviews(_movieToView);
				getAverageScore(_movieToView);
			}
		}


		private void getAverageScore(Movie movie)
		{
			if (movie is object)
			{
				AverageScore = Math.Round(_service.findAverageScoreByMovie(movie.MOV_ID), 1);
			}
		}


		private void getMovieGenre(Movie movie)
		{
			if (movie is object)
			{
				MovieGenre = _service.findGenresByMovie(movie.MOV_ID).FirstOrDefault<Genre>().GNR_LABEL;
			}
		}


		private void getMovieReviews(Movie movie)
		{
			if (movie is object)
			{
				ReviewGrid.Clear();

				List<Review> movieReviews = _service.findReviewsByMovie(movie.MOV_ID);

				foreach (var review in movieReviews)
				{
					User reviewUser = _service.findUserByReview(review.REV_ID);

					var gridReview = new
					{
						USR_NAME = reviewUser.USR_FIRST_NAME + " " + reviewUser.USR_LAST_NAME,
						REV_RATING = review.REV_RATING,
						REV_CONTENT = review.REV_CONTENT
					};

					ReviewGrid.Add(gridReview);
				}
			}
		}


		void handleSubmit()
		{
			ShowAddReview();
		}


		bool canHandleSubmit()
		{
			return !_service.userHasReviewedMovie(SessionManager.CurrentUser, MovieToView);
		}


		private void ShowAddReview()
		{
			Messenger.Default.Send(new NotificationMessage(MovieToView, "ShowReviewView"));
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
