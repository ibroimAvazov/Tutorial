using VkNet;
using VkNet.Model;

namespace MessengerVK
{
    public class Admin
    {
        private  User userSingelton;
        private  VkApi apiSingelton;
        protected  static  Admin AdminSingelton { get; set; }

        public  VkApi ApiSingelton
        {
            get
            {
                return apiSingelton;
            }

            set
            {
                apiSingelton = value;
            }
        }

        public User UserSingelton
        {
            get
            {
                return userSingelton;
            }

            set
            {
                userSingelton = value;
            }
        }

        protected Admin()
        {
            userSingelton=new User();
            apiSingelton=new VkApi();
        }

        public static Admin GetInstance()
        {
            if (AdminSingelton == null)
            {
                
                AdminSingelton=new Admin();
            }
            return AdminSingelton;
        }

    }
}