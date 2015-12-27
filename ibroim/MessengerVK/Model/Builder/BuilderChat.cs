using MessengerVK.Interpreter;

namespace MessengerVK.Builder
{
    // "Director"

    class Director
    {
        // Builder uses a complex series of steps
        public string Construct(Builder builder,Message message, int IndexSelectedFriend,string SelectedFriendName,string SelectedFriendLastName)
        {
          return  builder.BuildChat(message,IndexSelectedFriend,SelectedFriendName,SelectedFriendLastName);
        }
        
    }

    // "Builder"

    abstract class Builder
    {
        public abstract string BuildChat(Message message, int IndexSelectedFriend,string SelectedFriendName,string SelectedFriendLastName);

    }

    // "Build Message with emoji"

    class ChatWithEmoji : Builder
    {
        
        public override string BuildChat(Message message, int IndexSelectedFriend,string SelectedFriendName,string SelectedFriendLastName)
        {
            lock (Admin.GetInstance())
            {
                AbstractExpression exp = new TerminalExpression();
                return exp.Interpret(message, IndexSelectedFriend,SelectedFriendName,SelectedFriendLastName);
            }
        }

    }
    // "Build Message without emoji"

    class ChatWithOutEmoji : Builder
    {
        public override string BuildChat(Message message,int IndexSelectedFriend,string SelectedFriendName,string SelectedFriendLastName)
        {
            lock (Admin.GetInstance())
            {
                AbstractExpression exp = new NonTerminalExpression();
                return exp.Interpret(message, IndexSelectedFriend, SelectedFriendName, SelectedFriendLastName);
            }
        }

    }


    
}
