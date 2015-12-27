using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace MessengerVK.Builder
{
    class MethodMessage
    {
        public static List<Message> ConvertVkNetMessageToMyMessageTypeAndGetResult(long id)
        {
           
            HistoryGetParams historyGetParams=new HistoryGetParams();
            historyGetParams.Count = 20;
            historyGetParams.UserID = id;
            //int countReceiveMessages = 100;
            //ReadOnlyCollection<VkNet.Model.Message> messageCollectionWithFriendsReadOnly =
            //    Admin.GetInstance().ApiSingelton.Messages.GetHistory(id: id, false, out countReceiveMessages, null, 50);

            //List<Message> messageList = new List<Message>();
            //messageList = messageCollectionWithFriendsReadOnly.Select(t => new Message() { Id = t.UserId.Value, Body = t.Body, DateTime = t.Date.Value, HasEmoji = t.ContainsEmojiSmiles, TypeMessage = t.Type.Value }).ToList();
            MessagesGetObject messageCollectionWithFriendsReadOnly =
                Admin.GetInstance().ApiSingelton.Messages.GetHistory(historyGetParams);
            List<Message> messageList=new List<Message>();
            messageList = messageCollectionWithFriendsReadOnly.Messages.Select(t => new Message() { Id = t.UserId.Value, Body = t.Body, DateTime = t.Date.Value, HasEmoji = t.ContainsEmojiSmiles, TypeMessage = t.Type.Value }).ToList();

            for (int i = 0; i < messageList.Count; i++)
            {
                messageList[i] = CheckMessageHasEmojiWithMySysmbol(messageList[i]);
            }
            return messageList;
        }

        public static Message CheckMessageHasEmojiWithMySysmbol(Message message)
        {
            Regex regex = new Regex(":[)]|:[(]");
          MatchCollection matchCollection = regex.Matches(message.Body);
            message.HasEmoji = matchCollection.Count != 0 ? true : message.HasEmoji.Value;
            return message;
        }

    }
}
