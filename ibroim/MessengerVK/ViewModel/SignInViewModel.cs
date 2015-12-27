using MessengerVK.ViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VkNet;
using VkNet.Enums.Filters;


namespace MessengerVK
{
    
    public  class SignInViewModel :DependencyObject,INotifyPropertyChanged
    {
        private string _status;
        private const int appID = 5074413; // ID приложения
        private string login; // email или телефон
        private string password; // пароль для авторизации
        private static AuthInformation authInformation;
        private Visibility isVisible;
        private Settings scope; //Уровень доступа
        private ICommand buttonSign;
        ICommand close;
        public event PropertyChangedEventHandler PropertyChanged;

        public string status
        {
            get { return _status; }
            set
            {
                _status = value;
               OnPropertyChanged("status");
            }
        }

        public string Login
        {
            get { return login; }

            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get { return password; }

            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public Visibility IsVisible
        {
            get { return isVisible; }

            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        public static readonly DependencyProperty CloseWindowFlagProperty =
        DependencyProperty.Register("CloseWindowFlag", typeof(bool?), typeof(SignInViewModel), new UIPropertyMetadata(null));
        public bool? CloseWindowFlag
        {
            get { return (bool?)GetValue(CloseWindowFlagProperty); }
            set { SetValue(CloseWindowFlagProperty, value); }
        }
        //Methods
        public SignInViewModel()
        {
            scope = Settings.All;
            authInformation = new AuthInformation();
            MainWindow.messageManager = new MessageManager();
        }

        public void StartUpMessageManager()
        {
            MainWindow.messageManager.InitializeComponent();
            MainWindow.messageManager.ShowDialog();
       }
        private async Task Wait()
        {
            await Task.Run((() =>
            {
                try
                {
                    ApiAuthParams apiAuthParams=new ApiAuthParams();
                    apiAuthParams.ApplicationId = appID;
                    apiAuthParams.Login = Login;
                    apiAuthParams.Password = Password;
                    apiAuthParams.Settings = scope;
                    status = authInformation.PleaseWait;
                    Admin.GetInstance().ApiSingelton.Authorize(apiAuthParams);
                    Admin.GetInstance().UserSingelton =
                        Admin.GetInstance().ApiSingelton.Users.Get((long) Admin.GetInstance().ApiSingelton.UserId);

                }
                catch (Exception)
                {
                    status = authInformation.AuthFailed;
                }

            }));
        }

        private async void Authorization()
        {
            try
            {
                await Wait();
                if (Admin.GetInstance().ApiSingelton.AccessToken != null)
                {
                    IsVisible = Visibility.Collapsed;
                    StartUpMessageManager();
                }
            }
            catch
            {
                status = authInformation.AuthFailed;
            }
        }


        public ICommand ButtonSign
        {
            get
            {
                return buttonSign ?? (buttonSign = new RelayCommand((args) =>
                {
                    Password = ((PasswordBox) args).Password;
                    if ((Login!=null) && Password.Length>0)
                    {
                        Authorization();
                    }
                    else
                    {
                        status = authInformation.NullField;
                    }
                }));
            }
        }

        
        public ICommand Close
        {
            get { return close = new RelayCommand((args) => { CloseWindowFlag = true; }); }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
