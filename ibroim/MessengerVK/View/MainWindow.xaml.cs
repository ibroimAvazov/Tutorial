using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace MessengerVK
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MessageManager messageManager;
        public MainWindow()
        {
            InitializeComponent();
           
        }
       //Метод который позволяет перетаскивать окно
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
