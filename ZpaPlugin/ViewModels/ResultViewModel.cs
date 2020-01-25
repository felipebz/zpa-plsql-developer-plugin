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
        private ObservableCollection<IssueView> issues;

        public ResultViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Issues = new ObservableCollection<IssueView>
                {
                    new IssueView { Severity = "MAJOR", Message = "Example of message", StartLine = 10 }
                };
            }
        }

        public ICollectionView IssueView
        {
            get { return issueView; }
        }

        public ObservableCollection<IssueView> Issues
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
            var issue = view.CurrentItem as IssueView;
            ZpaPlugin.SetError(issue.StartLine, issue.StartColumn);
        }
    }

    internal class CustomSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            var issue1 = (IssueView)x;
            var issue2 = (IssueView)y;

            return issue1.StartLine.CompareTo(issue2.StartLine);
        }
    }

    public class IssueView
    {
        internal IssueView() { }

        public IssueView(Issue issue)
        {
            Severity = issue.Severity;
            Message = issue.PrimaryLocation.Message;
            StartLine = issue.PrimaryLocation.TextRange.StartLine;
            StartColumn = issue.PrimaryLocation.TextRange.StartColumn ?? 0;
        }

        public string Severity { get; internal set; }
        public string Message { get; internal set; }
        public int StartLine { get; internal set; }
        public int StartColumn { get; internal set; }
    }
}
