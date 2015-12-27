using System.Collections.ObjectModel;
using VkNet.Enums.Filters;
using VkNet.Model;
using static MessengerVK.FriendModel.FriendListSingelton;

namespace MessengerVK.FriendModel
{
    public class FriendProfileFields
    {
        public static void GetFriendProfileFields()
        {
            AddFriendsToList(GetOnlineFriendsIds(), GetFriendsListTemporaryReadOnly());
        }
       
        
        protected static void AddFriendsToList(ReadOnlyCollection<long>  OnlineFriendsIds, ReadOnlyCollection<User> friendsListTemporaryReadOnly)
        {
            for (int i = 0; i < friendsListTemporaryReadOnly.Count; i++)
            {
                Friend friendTemplate = new Friend();
                for (int j = 0; j < OnlineFriendsIds.Count; j++)
                {
                    
                    if (OnlineFriendsIds[j] == friendsListTemporaryReadOnly[i].Id)
                    {
                        friendTemplate.Online = true;
                    }
                }
                friendTemplate.Avatar = friendsListTemporaryReadOnly[i].PhotoPreviews.Photo100;
                friendTemplate.Name = friendsListTemporaryReadOnly[i].FirstName;
                friendTemplate.Id = friendsListTemporaryReadOnly[i].Id;
                friendTemplate.OnlineIcon = friendTemplate.Online ? "http://www.clker.com/cliparts/N/G/g/v/d/N/glossy-red-icon-button-md.png" : "https://upload.wikimedia.org/wikipedia/commons/6/6d/Ledred.png";
                GetInstance().FriendsList.Add(friendTemplate);
            }
            
        }

        protected static void RefreshFriendsList(ReadOnlyCollection<long> OnlineFriendsIds, ReadOnlyCollection<User> friendsListTemporaryReadOnly)
        {
            for (int i = 0; i < friendsListTemporaryReadOnly.Count; i++)
            {
                var friendTemplate = FriendTemplate(OnlineFriendsIds, friendsListTemporaryReadOnly, i);
                if (FriendListSingelton.GetInstance().FriendsList.Count != friendsListTemporaryReadOnly.Count)
                {
                    RefreshFriendsListClearListIfCountsNotEquals();
                }
                else
                {
                    RefreshFriendsListClearListIfCountsEquals(i,friendTemplate);
                }
            }
        }

        private static Friend FriendTemplate(ReadOnlyCollection<long> OnlineFriendsIds,
            ReadOnlyCollection<User> friendsListTemporaryReadOnly, int i)
        {
            Friend friendTemplate = new Friend();
            for (int j = 0; j < OnlineFriendsIds.Count; j++)
            {
                if (OnlineFriendsIds[j] == friendsListTemporaryReadOnly[i].Id)
                {
                    friendTemplate.Online = true;
                }
            }
            friendTemplate.Avatar = friendsListTemporaryReadOnly[i].PhotoPreviews.Photo100;
            friendTemplate.Name = friendsListTemporaryReadOnly[i].FirstName;
            friendTemplate.Id = friendsListTemporaryReadOnly[i].Id;
            friendTemplate.OnlineIcon = friendTemplate.Online ? "http://www.clker.com/cliparts/N/G/g/v/d/N/glossy-red-icon-button-md.png" : "https://upload.wikimedia.org/wikipedia/commons/6/6d/Ledred.png";

            return friendTemplate;
        }

        public static void GetFriendProfileFieldsTimerForAsyncMethod()
        {
            RefreshFriendsList(GetOnlineFriendsIds(), GetFriendsListTemporaryReadOnly());
        }

        protected static  void RefreshFriendsListClearListIfCountsNotEquals()
        {
            GetInstance().FriendsList.Clear();
            GetFriendProfileFields();
        }

        protected static void RefreshFriendsListClearListIfCountsEquals(int i,Friend friendTemplate)
        {
            GetInstance().FriendsList[i] = friendTemplate;

        }
        protected static ReadOnlyCollection<User> GetFriendsListTemporaryReadOnly()
        {
            ReadOnlyCollection<User> friendsListTemporaryReadOnly = Admin.GetInstance().ApiSingelton.Friends.Get(uid: (long)Admin.GetInstance().ApiSingelton.UserId, fields: ProfileFields.FirstName);
            friendsListTemporaryReadOnly = Admin.GetInstance().ApiSingelton.Friends.Get(uid: (long)Admin.GetInstance().ApiSingelton.UserId, fields: ProfileFields.LastName|ProfileFields.Uid|ProfileFields.Photo100);
     
           
            return friendsListTemporaryReadOnly;
        }
        protected static ReadOnlyCollection<long> GetOnlineFriendsIds()
        {
            ReadOnlyCollection<long> OnlineFriendsIds = Admin.GetInstance().ApiSingelton.Friends.GetOnline(Admin.GetInstance().UserSingelton.Id);
            return OnlineFriendsIds;
        } 

    }
}