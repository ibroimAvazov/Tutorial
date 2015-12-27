using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using MessengerVK.Helpers.Control;
using MessengerVK.ViewModel;
using VkNet.Model;


namespace MessengerVK
{
    /// <summary>
    /// Логика взаимодействия для MessageManager.xaml
    /// </summary>
    public partial class MessageManager : Window
    {
        public MessageManager()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageManagerViewModel myDataCtx = new MessageManagerViewModel();
            DataContext = myDataCtx;
        }


    }
}
