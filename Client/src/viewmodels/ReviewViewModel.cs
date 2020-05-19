using Client.src.helpers;
using Data;
using Data.src.facade;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;


namespace Client.src.viewmodels
{

	public class ReviewViewModel : ViewModelBase
	{
		#region properties

		private IServiceFacade _service = null;

		public Movie _movieToReview;

		private string _reviewContent = null;
		private int _reviewRating = 0;

		public Movie MovieToReview
		{
			get { return _movieToReview; }
			set
			{
				_movieToReview = value;
				ReviewContent = "";
				ReviewRating = 0;
				RaisePropertyChanged();
			}
		}

		public string ReviewContent
		{
			get { return _reviewContent; }
			set
			{
				_reviewContent = value;
				RaisePropertyChanged();
			}
		}

		public int ReviewRating
		{
			get { return _reviewRating; }
			set
			{
				_reviewRating = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand cancelCommand { get; }
		public RelayCommand addReviewCommand { get; }

		#endregion properties

		#region constructor

		public ReviewViewModel()
		{
			_service = ServiceFacade.getInstance;

			cancelCommand = new RelayCommand(handleCancel, true);
			addReviewCommand = new RelayCommand(handleSubmit, canHandleSubmit);
			Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
		}

		#endregion constructor

		#region methods

		private void NotificationMessageReceived(NotificationMessage message)
		{
			if (message.Notification == "ShowReviewView")
			{
				MovieToReview = (Movie)message.Sender;
			}
		}


		void handleSubmit()
		{
			createReview();	
		}


		bool canHandleSubmit()
		{
			return _reviewContent != null && _reviewContent.Length != 0 && _reviewRating != 0;
		}


		void handleCancel()
		{
			Messenger.Default.Send(new NotificationMessage(MovieToReview, "ShowViewView"));
		}


		void createReview()
		{
			Review reviewToCreate = new Review
			{
				REV_CONTENT = ReviewContent,
				REV_RATING = ReviewRating,
				Movie = MovieToReview,
				User = SessionManager.CurrentUser
			};

			_service.createReview(reviewToCreate);

			Messenger.Default.Send(new NotificationMessage("RefreshReviews"));
			Messenger.Default.Send(new NotificationMessage(MovieToReview, "ShowViewView"));
		}

		#endregion methods
	}

}
