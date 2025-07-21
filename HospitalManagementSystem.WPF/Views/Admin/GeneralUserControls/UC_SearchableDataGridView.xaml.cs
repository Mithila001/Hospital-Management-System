using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.Views.Admin.GeneralUserControls
{
    /// <summary>
    /// Interaction logic for UC_SearchableDataGridView.xaml
    /// A reusable UserControl providing a DataGrid with integrated search and filter capabilities.
    /// It exposes Dependency Properties for data binding to enable MVVM compliance.
    /// </summary>
    public partial class UC_SearchableDataGridView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the UC_SearchableDataGridView class.
        /// Attaches the Loaded event handler for initial column application.
        /// </summary>
        public UC_SearchableDataGridView()
        {
            InitializeComponent();
            this.Loaded += UC_SearchableDataGridView_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl.
        /// Ensures columns are applied to the internal DataGrid if they are set before the control is fully loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void UC_SearchableDataGridView_Loaded(object sender, RoutedEventArgs e)
        {
            // Apply columns when loaded if they're already set, ensuring initial display
            if (Columns != null && MyDataGrid != null)
            {
                ApplyColumnsToDataGrid(Columns);
            }
        }

        /// <summary>
        /// Applies the provided collection of DataGridColumn objects to the internal DataGrid.
        /// This method clears existing columns and adds new ones, cloning each column to prevent sharing issues.
        /// </summary>
        /// <param name="newColumns">The collection of DataGridColumn objects to apply.</param>
        private void ApplyColumnsToDataGrid(ObservableCollection<DataGridColumn> newColumns)
        {
            MyDataGrid.Columns.Clear();
            foreach (var column in newColumns)
            {
                // Create a new instance of each column to avoid sharing issues
                if (column is DataGridTextColumn textColumn)
                {
                    var newColumn = new DataGridTextColumn
                    {
                        Header = textColumn.Header,
                        Width = textColumn.Width
                    };

                    // Clone the binding to ensure independent binding paths
                    if (textColumn.Binding is Binding binding)
                    {
                        newColumn.Binding = new Binding(binding.Path.Path)
                        {
                            Mode = binding.Mode,
                            UpdateSourceTrigger = binding.UpdateSourceTrigger,
                            Converter = binding.Converter,
                            StringFormat = binding.StringFormat
                        };
                    }
                    MyDataGrid.Columns.Add(newColumn);
                }
                // Extend this logic to handle other DataGridColumn types (e.g., DataGridTemplateColumn, DataGridCheckBoxColumn) as needed.
            }
        }

        /// <summary>
        /// Identifies the ItemsSource Dependency Property.
        /// This property binds the data collection to be displayed in the DataGrid.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// Gets or sets the collection that is used to generate the content of the DataGrid.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Callback method for when the ItemsSource Dependency Property changes.
        /// Explicitly sets the internal DataGrid's ItemsSource to the new value.
        /// </summary>
        /// <param name="d">The DependencyObject on which the property changed.</param>
        /// <param name="e">The event data for the property that changed.</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UC_SearchableDataGridView;
            if (control != null && control.MyDataGrid != null)
            {
                // Set the internal DataGrid's ItemsSource to the new collection
                control.MyDataGrid.ItemsSource = e.NewValue as IEnumerable;
            }
        }

        /// <summary>
        /// Identifies the Columns Dependency Property.
        /// This property allows external definition of the DataGrid's columns.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                "Columns",
                typeof(ObservableCollection<DataGridColumn>),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null, OnColumnsChanged));

        /// <summary>
        /// Gets or sets the collection of DataGridColumn objects that define the display columns.
        /// </summary>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Callback method for when the Columns Dependency Property changes.
        /// Clears existing columns and adds new ones to the internal DataGrid, ensuring proper column instantiation.
        /// </summary>
        /// <param name="d">The DependencyObject on which the property changed.</param>
        /// <param name="e">The event data for the property that changed.</param>
        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UC_SearchableDataGridView;
            if (control != null && control.MyDataGrid != null)
            {
                if (e.NewValue is ObservableCollection<DataGridColumn> newColumns)
                {
                    control.ApplyColumnsToDataGrid(newColumns);
                }
                else
                {
                    control.MyDataGrid.Columns.Clear(); // Clear columns if the new value is null
                }
            }
        }

        /// <summary>
        /// Identifies the SearchQuery Dependency Property.
        /// This property binds the text entered into the search TextBox.
        /// </summary>
        public static readonly DependencyProperty SearchQueryProperty =
            DependencyProperty.Register(
                "SearchQuery",
                typeof(string),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the search query string.
        /// </summary>
        public string SearchQuery
        {
            get { return (string)GetValue(SearchQueryProperty); }
            set { SetValue(SearchQueryProperty, value); }
        }

        /// <summary>
        /// Identifies the SearchCommand Dependency Property.
        /// This property binds the command executed when the search button is clicked.
        /// </summary>
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                "SearchCommand",
                typeof(ICommand),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command to execute when the search operation is triggered.
        /// </summary>
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        /// <summary>
        /// Identifies the SearchToolTip Dependency Property.
        /// This property binds the tooltip text for the search TextBox.
        /// </summary>
        public static readonly DependencyProperty SearchToolTipProperty =
            DependencyProperty.Register(
                "SearchToolTip",
                typeof(string),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata("Enter search term"));

        /// <summary>
        /// Gets or sets the tooltip text for the search input field.
        /// </summary>
        public string SearchToolTip
        {
            get { return (string)GetValue(SearchToolTipProperty); }
            set { SetValue(SearchToolTipProperty, value); }
        }

        /// <summary>
        /// Identifies the FilterOptionsCommand Dependency Property.
        /// This property binds the command executed when the filter options button is clicked.
        /// </summary>
        public static readonly DependencyProperty FilterOptionsCommandProperty =
            DependencyProperty.Register(
                "FilterOptionsCommand",
                typeof(ICommand),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command to execute when the filter options button is clicked.
        /// </summary>
        public ICommand FilterOptionsCommand
        {
            get { return (ICommand)GetValue(FilterOptionsCommandProperty); }
            set { SetValue(FilterOptionsCommandProperty, value); }
        }
    }
}