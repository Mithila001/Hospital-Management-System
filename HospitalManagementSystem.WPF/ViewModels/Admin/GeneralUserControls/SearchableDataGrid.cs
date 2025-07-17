using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace HospitalManagementSystem.WPF.Views.Admin.GeneralUserControls
{
    // Allow <SearchableDataGrid.Columns> ... </> in XAML
    [ContentProperty(nameof(Columns))]
    public partial class SearchableDataGrid : UserControl
    {
        public SearchableDataGrid()
        {
            InitializeComponent();
            Columns.CollectionChanged += OnColumnsChanged;
        }

        // 1) Columns injected from parent XAML
        public ObservableCollection<DataGridColumn> Columns { get; }
            = new ObservableCollection<DataGridColumn>();

        private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // whenever parent adds columns, push them into the inner DataGrid
            if (e.NewItems != null)
                foreach (DataGridColumn col in e.NewItems)
                    PART_DataGrid.Columns.Add(col);

            if (e.OldItems != null)
                foreach (DataGridColumn col in e.OldItems)
                    PART_DataGrid.Columns.Remove(col);
        }

        // 2) ItemsSource
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(SearchableDataGrid),
                new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // 3) SelectedItem
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(SearchableDataGrid),
                new PropertyMetadata(null));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // 4) SearchQuery
        public static readonly DependencyProperty SearchQueryProperty =
            DependencyProperty.Register(
                nameof(SearchQuery),
                typeof(string),
                typeof(SearchableDataGrid),
                new PropertyMetadata(string.Empty));

        public string SearchQuery
        {
            get => (string)GetValue(SearchQueryProperty);
            set => SetValue(SearchQueryProperty, value);
        }

        // 5) SearchCommand
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                nameof(SearchCommand),
                typeof(System.Windows.Input.ICommand),
                typeof(SearchableDataGrid),
                new PropertyMetadata(null));

        public System.Windows.Input.ICommand SearchCommand
        {
            get => (System.Windows.Input.ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        // 6) FilterCommand & FilterButtonText (optional)
        public static readonly DependencyProperty FilterCommandProperty =
            DependencyProperty.Register(
                nameof(FilterCommand),
                typeof(System.Windows.Input.ICommand),
                typeof(SearchableDataGrid),
                new PropertyMetadata(null));

        public System.Windows.Input.ICommand FilterCommand
        {
            get => (System.Windows.Input.ICommand)GetValue(FilterCommandProperty);
            set => SetValue(FilterCommandProperty, value);
        }

        public static readonly DependencyProperty FilterButtonTextProperty =
            DependencyProperty.Register(
                nameof(FilterButtonText),
                typeof(string),
                typeof(SearchableDataGrid),
                new PropertyMetadata("Filter"));

        public string FilterButtonText
        {
            get => (string)GetValue(FilterButtonTextProperty);
            set => SetValue(FilterButtonTextProperty, value);
        }

        // 7) SearchToolTip (optional)
        public static readonly DependencyProperty SearchToolTipProperty =
            DependencyProperty.Register(
                nameof(SearchToolTip),
                typeof(string),
                typeof(SearchableDataGrid),
                new PropertyMetadata("Enter search term"));

        public string SearchToolTip
        {
            get => (string)GetValue(SearchToolTipProperty);
            set => SetValue(SearchToolTipProperty, value);
        }
    }
}
