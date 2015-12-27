using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using MessengerVK.Builder;
using VkNet.Enums;

namespace MessengerVK.Interpreter
{
    /// <summary>
    /// The 'AbstractExpression' abstract class
    /// </summary>
    public abstract class AbstractExpression
    {
        public abstract string Interpret(Message message, int IndexSelectedFriend, String SelectedFriendName, string SelectedFriendLastName);

        public static void HTMLTableCreater(out string path1, Message message, out string path2, string SelectedFriendName, string SelectedFriendLastName)
        {
            path1 = "<table align=" + "center" + "><tbody><tr><th width=" + "50px" + "><div><a><font color=" +
                    "#002133" + ">" + SelectedFriendName + "<br/>" +
                    SelectedFriendLastName +
                    "</font></div></th><th width=" + "400px" + "><div><a align=" + "left><font color=" +
                    "#001433>" + message.Body + "</font><a/></div></div></th><th width=" + "50px" + "><div><a><font color=" +
                    "#bfbfbf>" + message.DateTime +
                    "</font></a></div></th></tr></tbody></table>";
            path2 = "<table align=" + "center" + "><tbody><tr><th width=" + "50px" + "><div><a><font color=" +
                    "#002133" + ">" + Admin.GetInstance().UserSingelton.FirstName + "<br/>" +
                    Admin.GetInstance().UserSingelton.LastName +
                    "</font></div></th><th width=" + "400px" + "><div><a align=" + "left><font color=" +
                    "#001433>" + message.Body + "</font><a/></div></div></th><th width=" + "50px" + "><div><a><font color=" +
                    "#bfbfbf>" + message.DateTime +
                    "</font></a></div></th></tr></tbody></table>";
        }


    }
    /// <summary>
    /// The 'TerminalExpression' class for message whichtr no emoji
    /// </summary>
    public class TerminalExpression : AbstractExpression
    {
        public override string Interpret(Message message, int IndexSelectedFriend, string SelectedFriendName,
            string SelectedFriendLastName)
        {
           
        string[]
            patterns = new[]
            {":[)]", "(:[(])", "😊", "😃", ":-D", "😆", "😜", "😋", "😍", "😎", "😒", "😏"};
            string beginImgTag = "<img src=data:image/png;base64,";
            string endImgTag = "></img>";
            Bitmap bitmap = new Bitmap(MessengerVK.Properties.Resources._01);
            FlyWeight.SmileFactory smileFactory = new FlyWeight.SmileFactory();
            smileFactory.GetSmile(":-[)]");
            for (int i = 0; i < patterns.Length; i++)
            {
                message.Body = Regex.Replace(message.Body, patterns[i],
                    replacement: beginImgTag + ImageToBase64(smileFactory.GetSmile(patterns[i]), ImageFormat.Png) + endImgTag);
            }
            string HtmlTableWithMessage = string.Empty, path1, path2;
            HTMLTableCreater(out path1, message, out path2, SelectedFriendName, SelectedFriendLastName);
            HtmlTableWithMessage = message.TypeMessage == MessageType.Received ? path1 : path2;
            return HtmlTableWithMessage;

        }
        public string ImageToBase64(System.Drawing.Bitmap image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

    }
    /// <summary>
    /// The 'NonterminalExpression' class for message with emoji
    /// </summary>
    public class NonTerminalExpression : AbstractExpression
    {
        public override string Interpret(Message message, int IndexSelectedFriend, string SelectedFriendName, string SelectedFriendLastName)
        {

            string HtmlTableWithMessage = string.Empty, path1, path2;
            HTMLTableCreater(out path1, message, out path2, SelectedFriendName, SelectedFriendLastName);
            HtmlTableWithMessage = message.TypeMessage == MessageType.Received ? path1 : path2;
            return HtmlTableWithMessage;
        }
    }
}