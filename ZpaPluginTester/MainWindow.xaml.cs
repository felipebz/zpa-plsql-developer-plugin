using System;
using System.Windows;
using Microsoft.Win32;

namespace ZpaPluginTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog { Title = "Select a file..." };
                if (dialog.ShowDialog() == true)
                {
                    /*var runner = new ZpaRunner(new NullPlsqlDevApi());
                    runner.Analyze(File.ReadAllText(dialog.FileName));*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
