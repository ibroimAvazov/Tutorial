using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VkNet.Enums;

namespace MessengerVK.Builder
{
   public class Message
   {
       long id;
       MessageType typeMessage;
       DateTime dateTime;
       string body;
       string authorAvatar ;
       bool? hasEmoji;
       public string AuthorAvatar
       {
           get { return authorAvatar; }
           set { authorAvatar = value; }
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

        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }

            set
            {
                dateTime = value;
            }
        }

        public string Body
        {
            get
            {
                return body;
            }

            set
            {
                body = value;
            }
        }

        public MessageType TypeMessage
        {
            get
            {
                return typeMessage;
            }

            set
            {
                typeMessage = value;
            }
        }

        public bool? HasEmoji
        {
            get
            {
                return hasEmoji;
            }

            set
            {
                hasEmoji = value;
            }
        }
    }
}
