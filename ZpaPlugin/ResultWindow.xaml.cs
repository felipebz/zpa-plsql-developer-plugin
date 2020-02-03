using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ZpaPlugin.Models;
using ZpaPlugin.ViewModels;

namespace ZpaPlugin
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private readonly ResultViewModel vm;

        public ResultWindow()
        {
            InitializeComponent();

            Closed += ResultWindow_Closed;

            vm = (ResultViewModel)DataContext;
        }

        public ResultWindow(IPlsqlDevApi plsqlDevApi, List<Issue> issues) : this()
        {
            vm.Issues = new ObservableCollection<IssueView>(issues.Select(x => new IssueView(x)));
            vm.PlsqlDevApi = plsqlDevApi;
        }

        private void ResultWindow_Closed(object sender, System.EventArgs e)
        {
            vm.PlsqlDevApi.ClearError();
        }
    }
}
