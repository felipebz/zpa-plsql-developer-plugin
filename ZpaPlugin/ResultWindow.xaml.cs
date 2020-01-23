using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ZpaPlugin.Models;

namespace ZpaPlugin
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private List<Issue> issues;

        public ResultWindow()
        {
            InitializeComponent();
        }

        public ResultWindow(List<Issue> issues) : this()
        {
            this.issues = issues;
            label.Content = $"Found {issues.Count} issues.";
        }
    }
}
