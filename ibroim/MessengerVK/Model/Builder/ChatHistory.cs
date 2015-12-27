using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerVK.Builder
{
    class ChatHistory
    {
        private List<Message> chatHistoryList;
        private static ChatHistory chatHistorySingelton;

        protected ChatHistory()
        {
            chatHistoryList=new List<Message>();
        }

        public List<Message> ChatHistoryList
        {
            get
            {
                return chatHistoryList;
            }

            set
            {
                chatHistoryList = value;
            }
        }

     
        public static ChatHistory GetInstance(long id)
        {
                chatHistorySingelton=new ChatHistory();
                chatHistorySingelton.ChatHistoryList = MethodMessage.ConvertVkNetMessageToMyMessageTypeAndGetResult(id);
            return chatHistorySingelton;
        }
    }
}
