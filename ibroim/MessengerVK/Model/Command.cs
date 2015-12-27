using System;
using MessengerVK.Builder;
using VkNet.Enums;
using Word = Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerVK
{
    interface ICommandSaveToWord
    {
        void Save(string AdminFristName, string AdminLastName, 
            string FriendFristName, string FriendLastName,int count, List<Message> MessageList,int indexSelectedFriend);
        void Delete(string wordDocumentPath);
    }
    // Receiver
    class ChatHistorySaver
    {
      
        public void Save(string AdminFristName, string AdminLastName, 
            string FriendFristName, string FriendLastName,int count, List<Message> MessageList,int indexSelectedFriend)
        {
           
            Word.Application wordApp = new Word.Application();
            Word.Document wordDocument= wordApp.Documents.Add(DocumentType: Word.WdNewDocumentType.wdNewBlankDocument);
            try
            {   
                var range = wordDocument.Content;
                Word.Tables Tables = wordDocument.Tables;
                Tables.Add(range, count, 3, true, true);
                FilTable(Tables, AdminFristName, AdminLastName, FriendFristName, FriendLastName, count, MessageList);
                wordDocument.Save();
                FriendModel.FriendListSingelton.GetInstance().FriendsList[indexSelectedFriend].WordDocumentPath =wordDocument.FullName;
                wordDocument.Close();
                wordApp.Quit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
         }
        private static void FilTable( Word.Tables Tables, string AdminFristName, string AdminLastName, string FriendFristName, string FriendLastName,int count, List<Message> MessageList)
        {
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <=count; j++)
                {
                    SwitchUserName(Tables, i, j,  AdminFristName,  AdminLastName,  FriendFristName,  FriendLastName, MessageList);

                }
            }
        }
        private static void SwitchUserName( Word.Tables Tables, int i, int j,string AdminFristName,string AdminLastName,string FriendFristName,string FriendLastName, List<Message> MessageList)
        {
            switch (i)
            {
                case 1:
                    Tables[1].Cell(j, i).Range.Text =
                        MessageList[j-1].DateTime.ToString();
                    break;
                case 2:
                    Tables[1].Cell(j, i).Range.Text =
                        MessageList[j-1].Body;
                    break;
                default:
                    Tables[1].Cell(j, i).Range.Text =
                        MessageList[j-1].TypeMessage != MessageType.Received
                            ? AdminFristName + "\n" +
                              AdminLastName
                            : FriendFristName + "\n" +
                              FriendLastName;
                    break;
            }
        }
    }
    class ChatHistoryOnCommand : ICommandSaveToWord
    {
        ChatHistorySaver chatSaver;
        public ChatHistoryOnCommand(ChatHistorySaver tvSet)
        {
            chatSaver = tvSet;
        }
        public void Save(string AdminFristName, string AdminLastName, string FriendFristName, string FriendLastName,int count, List<Message> MessageList,int indexSelectedFriend)
        {
            chatSaver.Save(AdminFristName,  AdminLastName,  FriendFristName,  FriendLastName,count, MessageList, indexSelectedFriend);
        }

        public void Delete(string wordDocumentPath)
        {
            if (wordDocumentPath != null)
            {
                File.Delete(wordDocumentPath);
            }
        }
    }
    // Invoker 
    class Saver
    {
        ICommandSaveToWord command;

        public Saver() { }

        public void SetCommand(ICommandSaveToWord com)
        {
            command = com;
        }

        public void PressSave(string AdminFristName, string AdminLastName, string FriendFristName, string FriendLastName,int count, List<Message> MessageList,int indexSelectedFriend)
        {
            if (command != null) command.Save(AdminFristName,  AdminLastName,  FriendFristName,  FriendLastName, count, MessageList, indexSelectedFriend);
        }

        public void PressDelete(string wordDocumentPath)
        {
            if (command != null)
            {
                command.Delete(wordDocumentPath);
                MessageBox.Show(wordDocumentPath);
            }
        }

    }
}