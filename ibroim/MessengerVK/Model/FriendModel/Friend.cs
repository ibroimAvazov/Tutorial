using System;

namespace MessengerVK
{
    public class Friend
    {
        private Int64 id;
       private string name;
        private string lastName;
       private string avatar100px;
        private string onlineIcon;
        private bool online;
       private string country;
       private string city;
        private string birthDay;
        private string mobilePhone;
        private string wordDocumentPath;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public string Avatar
        {
            get
            {
                return avatar100px;
            }

            set
            {
                avatar100px = value;
            }
        }

        public bool Online
        {
            get
            {
                return online;
            }

            set
            {
                online = value;
            }
        }

        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
            }
        }

        public string BirthDay
        {
            get
            {
                return birthDay;
            }

            set
            {
                birthDay = value;
            }
        }

        public string MobilePhone
        {
            get
            {
                return mobilePhone;
            }

            set
            {
                mobilePhone = value;
            }
        }

        public string OnlineIcon
        {
            get
            {
                return onlineIcon;
            }

            set
            {
                onlineIcon = value;
            }
        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
            }
        }

        public string WordDocumentPath
        {
            get
            {
                return wordDocumentPath;
            }

            set
            {
                wordDocumentPath = value;
            }
        }
    }
}
