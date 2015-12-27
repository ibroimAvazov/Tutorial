using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace MessengerVK.FriendModel
{
    public class FriendListSingelton
    {
        private List<Friend> friendsList;
        protected static FriendListSingelton friendListSingelton { get; set; }

        protected FriendListSingelton()
        {
            friendsList=new List<Friend>();
        }
        public static FriendListSingelton GetInstance()
        {
            return friendListSingelton ?? (friendListSingelton = new FriendListSingelton());
        }

        public  List<Friend> FriendsList
        {
            get
            {
                return friendsList;
            }

            set
            {
                friendsList = value;

            }
        }
     
       public static void TimerUpdateFriendList()
        {
          
                Timer timerUpadate = new Timer();
                timerUpadate.Interval = 30000;
                timerUpadate.Elapsed += UpdateFriendListTimer;
                timerUpadate.AutoReset = true;
               timerUpadate.Start();
        }
       
        public  static void UpdateFriendList()
        {
            FriendProfileFields.GetFriendProfileFields();
        }
        private  static async void UpdateFriendListTimer(object sender, ElapsedEventArgs e)
        {
           await FriendListUpdateTimerAsync();
        }

        private static Task<List<Friend>> FriendListUpdateTimerAsync()
        {
            FriendProfileFields.GetFriendProfileFieldsTimerForAsyncMethod();
            return Task.Run(() =>
            {
               return GetInstance().FriendsList;
            });
        }
    }
}
