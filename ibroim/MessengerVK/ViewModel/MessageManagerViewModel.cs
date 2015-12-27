using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using MessengerVK.Builder;
using MessengerVK.Helpers.Control;
using Message = MessengerVK.Builder.Message;

namespace MessengerVK.ViewModel
{
    public class MessageManagerViewModel :DependencyObject, INotifyPropertyChanged
    {
        int indexSelectedFriend;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _myHtml;
        ICommand sendMessage;
        ICommand writeMessage;
        ICommand refresh;
        ICommand saveToWordFile;
        ICommand close;
        Saver saver;
        ChatHistorySaver chatHistorySaver;
        public  readonly DependencyProperty CloseWindowFlagProperty;
        public MessageManagerViewModel()
        {
            CloseWindowFlagProperty =DependencyProperty.Register("CloseWindowFlag", typeof(bool?), typeof(MessageManagerViewModel), new UIPropertyMetadata(null));
            FriendModel.FriendListSingelton.UpdateFriendList();
            FriendModel.FriendListSingelton.TimerUpdateFriendList();
            FriendsList = FriendModel.FriendListSingelton.GetInstance().FriendsList;
            saver = new Saver();
            chatHistorySaver = new ChatHistorySaver();
            
        }
        //property
        public int IndexSelectedFriend
        {
            get { return indexSelectedFriend; }
            set
            {
                indexSelectedFriend = value;
                OnPropertyChanged("IndexSelectedFriend");
            }
        }

        public ICommand Refresh
        {
            get
            {
                return refresh = new RelayCommand((o) =>
                {
                    WriteMessage.Execute(o);
                });
            }
        }
        public List<Friend> FriendsList
        {
            get
            {
                
                return FriendModel.FriendListSingelton.GetInstance().FriendsList;
            }

            set
            {
                FriendModel.FriendListSingelton.GetInstance().FriendsList = value;
                OnPropertyChanged("FriendList");
            }
        }

        public List<Message> MessageList
        {
            get {
                
                return 
                    
                    ChatHistory.GetInstance(FriendsList[IndexSelectedFriend].Id).ChatHistoryList;
            }
            set
            {
                ChatHistory.GetInstance(FriendsList[IndexSelectedFriend].Id).ChatHistoryList = value;
                OnPropertyChanged("MessageList");
            }
        }
        public string MyHtml
        {
            get { return _myHtml; }
            set
            {
                if (_myHtml != value)
                {
                    _myHtml = value;
                    OnPropertyChanged("MyHtml");
                }
            }
        }

      
        public bool? CloseWindowFlag
        {
            get { return (bool?)GetValue(CloseWindowFlagProperty); }
            set { SetValue(CloseWindowFlagProperty, value); }
        }
        //commands
        public ICommand WriteMessage
        {
            get
            {
                return writeMessage = new RelayCommand((o =>
                {

                    WriteMessageMethod();

                }));
            }
        }

        public ICommand Close
        {
            get
            {
                return close = new RelayCommand((o =>
                {
                    CloseWindowFlag = true;

                }));
            }
        }
        public ICommand SaveToWordFile
        {
            get
            {
                return saveToWordFile = new RelayCommand((o =>
                {
                   
                    saver.SetCommand(new ChatHistoryOnCommand(chatHistorySaver));
                    saver.PressSave(Admin.GetInstance().UserSingelton.FirstName,
                        Admin.GetInstance().UserSingelton.LastName, FriendsList[IndexSelectedFriend].Name,
                        FriendsList[IndexSelectedFriend].LastName,
                    MessageList.Count,MessageList,IndexSelectedFriend);
                    
                }));
            }
        }

        ICommand deleteWordFile;
        public ICommand DeleteWordFile
        {
            get
            {
                return deleteWordFile = new RelayCommand((o =>
                {
                    saver.SetCommand(new ChatHistoryOnCommand(chatHistorySaver));
                    saver.PressDelete(FriendsList[IndexSelectedFriend].WordDocumentPath);
                }));
            }
        }

        public ICommand SendMessage
        {
            get
            {
                return sendMessage=new RelayCommand((o =>
                {
                    TextBoxControl textBoxControl = (TextBoxControl) o;
                    Admin.GetInstance().ApiSingelton.Messages.Send(FriendsList[indexSelectedFriend].Id, false, textBoxControl.GetPlainText());
                    WriteMessage.Execute(o);
                   textBoxControl.Clear();
               } ));
            }
            
        }

        private string document;
        public string Document
        {
            get { return document; }
            set
            {
                document = value;
                OnPropertyChanged("Document");
            }
        }

        //methods
        private void WriteMessageMethod()
        {
            MessageList = new List<Message>();
            MessageList = ChatHistory.GetInstance(FriendsList[IndexSelectedFriend].Id).ChatHistoryList;
            Director director = new Director();
            Builder.Builder builder2 = new ChatWithEmoji();
            Builder.Builder builder = new ChatWithOutEmoji();
            String SelectedFriendName = FriendsList[IndexSelectedFriend].Name;
            string SelectedFriendLastName = FriendsList[IndexSelectedFriend].LastName;
            string path = "";
            _myHtml = String.Empty;
            MyHtml = MessageList.Count >= 0 ? IfHaveAnyMessageWithSelectedFriend(path, builder, builder2,SelectedFriendName,SelectedFriendLastName) : string.Empty;

        }

        private string IfHaveAnyMessageWithSelectedFriend(string path, Builder.Builder builder, Builder.Builder builder2,string Name,string LastName)
        {
            foreach (var vm in MessageList)
            {
                path += vm.HasEmoji.Value
                    ?(vm.Body.Length > 0 ? builder2.BuildChat(vm, IndexSelectedFriend,Name,LastName) : "")
                    :( vm.Body.Length > 0 ? builder.BuildChat(vm, IndexSelectedFriend,Name,LastName) : "");
            }
            string result = "<html><head><meta charset=" + "utf-8" +
                            "/><style>table{ width: 100%;}</style></head><body><div>" + path +
                            "</div></body></html>";
            return result;
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


