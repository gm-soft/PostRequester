using PostProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PostProcessor;

namespace PostRequester
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IEventListener
    {
        private IController _webController;

        private Dictionary<string, string> _dictionary;
        private string _baseAddress;

        public MainWindow()
        {
            InitializeComponent();
            SetKaspiData();
            _webController = new WebController(_baseAddress, _dictionary, this);
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_Url.Text) ||
                string.IsNullOrWhiteSpace(TextBox_CycleCount.Text))
            {
                OnEvent("Заполните поля");
            }

            string url = TextBox_Url.Text;
            int count = int.Parse(TextBox_CycleCount.Text);

            //string url = "https://www.hcsbk.kz/frequently-asked-question/";
            //int count = 1000;
            _webController.StartCycle(url, count);
        }

        public void OnEvent(string message)
        {
            Action action = () =>
            {
                TextBox_Log.Text += $"{DateTime.Now}: {message}\r\n";
                TextBox_Log.ScrollToEnd();
            };
            TextBox_Log.Dispatcher.InvokeAsync(action);
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            _webController.StopCycle();
        }

        private void SetKaspiData()
        {
            _dictionary = new Dictionary<string, string>
            {
                {"voting_user_comment", "Проверка на антифорджери" },
                {"voting_vote_type", "no" },
                {"proverko", "" },
                {"id", "337" }
            };
            _baseAddress = "kaspi.kz";
        }

        private void SetHcsbkData()
        {
            _dictionary = new Dictionary<string, string>
            {
                {"text_whyno", "This is check for penetration" },
                {"AJAX_PARAM", "Y" },
                {"ACTION", "whyno" },
                {"ID", "28976" }
            };
            _baseAddress = "www.hcsbk.kz";
        }
    }
}
