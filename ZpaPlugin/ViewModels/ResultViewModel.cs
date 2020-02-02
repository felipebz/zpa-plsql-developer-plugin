using System;
using System.Collections;
using System.Collections.Generic;
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
        private bool showBlocker;
        private bool showCritical;
        private bool showMajor;
        private bool showMinor;
        private bool showInformative;

        public ResultViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Issues = new ObservableCollection<IssueView>
                {
                    new IssueView { Severity = "INFO", Message = "Example of message", StartLine = 10 },
                    new IssueView { Severity = "MINOR", Message = "Example of message", StartLine = 10 },
                    new IssueView { Severity = "MAJOR", Message = "Example of message", StartLine = 10 },
                    new IssueView { Severity = "CRITICAL", Message = "Example of message", StartLine = 10 },
                    new IssueView { Severity = "BLOCKER", Message = "Example of message", StartLine = 10 }
                };
            }

            ClearFilters = new RelayCommand(OnClearFilters);
        }

        public RelayCommand ClearFilters { get; set; }

        public ICollectionView IssueView
        {
            get { return issueView; }
        }

        public bool ShowBlocker
        {
            get { return showBlocker; }
            set
            {
                SetProperty(ref showBlocker, value, nameof(ShowBlocker));
                RefreshView();
            }
        }

        public bool ShowCritical
        {
            get { return showCritical; }
            set
            {
                SetProperty(ref showCritical, value, nameof(ShowCritical));
                RefreshView();
            }
        }

        public bool ShowMajor
        {
            get { return showMajor; }
            set
            {
                SetProperty(ref showMajor, value, nameof(ShowMajor));
                RefreshView();
            }
        }

        public bool ShowMinor
        {
            get { return showMinor; }
            set
            {
                SetProperty(ref showMinor, value, nameof(ShowMinor));
                RefreshView();
            }
        }

        public bool ShowInformative
        {
            get { return showInformative; }
            set
            {
                SetProperty(ref showInformative, value, nameof(ShowInformative));
                RefreshView();
            }
        }

        public ObservableCollection<IssueView> Issues
        {
            get { return issues; }
            set
            {
                SetProperty(ref issues, value, nameof(Issues));

                issueView = (ListCollectionView)CollectionViewSource.GetDefaultView(issues);
                issueView.Filter = ApplyFilters;
                issueView.CurrentChanged += CurrentIssueChanged;
                issueView.CustomSort = new CustomSorter();
                issueView.MoveCurrentToFirst();
                OnPropertyChanged(nameof(IssueView));
                RefreshView();
            }
        }

        public string FilterText
        {
            get
            {
                return $"Showing {issueView?.Count} of {issues?.Count}";
            }
        }

        private void RefreshView()
        {
            issueView.Refresh();
            OnPropertyChanged(nameof(FilterText));
        }

        private void CurrentIssueChanged(object sender, EventArgs e)
        {
            var view = (ListCollectionView)sender;
            var issue = view.CurrentItem as IssueView;
            if (issue == null)
            {
                ZpaPlugin.ClearError();
            }
            else
            {
                ZpaPlugin.SetError(issue.StartLine, issue.StartColumn);
            }
        }

        private bool ApplyFilters(object item)
        {
            var issue = item as IssueView;

            var severityResult = !ShowBlocker && !ShowCritical && !ShowMajor && !ShowMinor && !ShowInformative;
            severityResult |= ShowBlocker && issue.Severity == Severity.BLOCKER;
            severityResult |= ShowCritical && issue.Severity == Severity.CRITICAL;
            severityResult |= ShowMajor && issue.Severity == Severity.MAJOR;
            severityResult |= ShowMinor && issue.Severity == Severity.MINOR;
            severityResult |= ShowInformative && issue.Severity == Severity.INFO;

            return severityResult;
        }

        private void OnClearFilters()
        {
            ShowBlocker = false;
            ShowCritical = false;
            ShowMajor = false;
            ShowMinor = false;
            ShowInformative = false;

            issueView.MoveCurrentToFirst();
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
            RuleId = issue.RuleId;
            Severity = issue.Severity;
            Message = issue.PrimaryLocation.Message;
            StartLine = issue.PrimaryLocation.TextRange.StartLine;
            StartColumn = issue.PrimaryLocation.TextRange.StartColumn ?? 0;
        }

        public string RuleId { get; internal set; }
        public string Severity { get; internal set; }
        public string Message { get; internal set; }
        public int StartLine { get; internal set; }
        public int StartColumn { get; internal set; }
    }

    public class Severity
    {
        public const string BLOCKER = "BLOCKER";
        public const string CRITICAL = "CRITICAL";
        public const string MAJOR = "MAJOR";
        public const string MINOR = "MINOR";
        public const string INFO = "INFO";
    }
}
