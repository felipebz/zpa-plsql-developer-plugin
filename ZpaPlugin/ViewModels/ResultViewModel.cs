using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using ZpaPlugin.Models;
using ZpaPlugin.Mvvm;

namespace ZpaPlugin.ViewModels
{
    public class ResultViewModel : BindableBase
    {
        private ListCollectionView issueView;
        private ObservableCollection<Issue> issues;

        public ResultViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Issues = new ObservableCollection<Issue>
                {
                    new Issue { Severity = "MAJOR", PrimaryLocation = new PrimaryLocation { Message = "Example of message", TextRange = new TextRange { StartLine = 10 } } }
                };
            }
        }

        public ICollectionView IssueView
        {
            get { return issueView; }
        }

        public ObservableCollection<Issue> Issues
        {
            get { return issues; }
            set
            {
                SetProperty(ref issues, value, nameof(Issues));

                issueView = (ListCollectionView)CollectionViewSource.GetDefaultView(issues);
                issueView.CurrentChanged += CurrentIssueChanged;
                issueView.CustomSort = new CustomSorter();
                issueView.MoveCurrentToFirst();
                OnPropertyChanged(nameof(IssueView));
                issueView.Refresh();
            }
        }

        private void CurrentIssueChanged(object sender, EventArgs e)
        {
            var view = (ListCollectionView)sender;
            var issue = view.CurrentItem as Issue;
            ZpaPlugin.SetError(issue.PrimaryLocation.TextRange.StartLine, issue.PrimaryLocation.TextRange.StartColumn);
        }
    }

    internal class CustomSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            var issue1 = (Issue)x;
            var issue2 = (Issue)y;

            return issue1.PrimaryLocation.TextRange.StartLine.CompareTo(issue2.PrimaryLocation.TextRange.StartLine);
        }
    }
}
