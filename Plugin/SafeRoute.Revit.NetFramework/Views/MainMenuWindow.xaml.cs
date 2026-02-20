using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SafeRoute.Revit.NetFramework.Views
{
    public partial class MainMenuWindow : Window, INotifyPropertyChanged
    {
        private string _loggedUserText;
        private bool _isBusy;
        private int _progressValue;
        private string _progressText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string LoggedUserText
        {
            get => _loggedUserText;
            set { _loggedUserText = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public int ProgressValue
        {
            get => _progressValue;
            set { _progressValue = value; OnPropertyChanged(); }
        }

        public string ProgressText
        {
            get => _progressText;
            set { _progressText = value; OnPropertyChanged(); }
        }

        public MainMenuWindow(string userEmail)
        {
            InitializeComponent();

            try
            {
                var uri = new Uri("pack://application:,,,/SafeRoute.Revit.NetFramework;component/resources/menu-saferoute.png");
                var info = Application.GetResourceStream(uri);
                if (info == null)
                    MessageBox.Show("NÃO achou menu-saferoute.png no resources via pack.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            DataContext = this;

            LoggedUserText = $"Logado: {userEmail}";
            ProgressText = "Pronto";
            ProgressValue = 0;
            IsBusy = false;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsBusy = true;
                ProgressText = "Carregando...";
                ProgressValue = 0;

                // Simulação de progresso
                for (int i = 1; i <= 100; i += 10)
                {
                    ProgressValue = i;
                    await Task.Delay(120);
                }

                ProgressValue = 100;
                ProgressText = "Concluído";
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
