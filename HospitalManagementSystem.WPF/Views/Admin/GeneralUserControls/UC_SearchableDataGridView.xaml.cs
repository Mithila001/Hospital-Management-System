using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.Views.Admin.GeneralUserControls
{
    public partial class UC_SearchableDataGridView : UserControl
    {
        public UC_SearchableDataGridView()
        {
            InitializeComponent();
            this.Loaded += UC_SearchableDataGridView_Loaded;
        }

        private void UC_SearchableDataGridView_Loaded(object sender, RoutedEventArgs e)
        {
            // Apply columns when loaded if they're already set
            if (Columns != null && MyDataGrid != null)
            {
                MyDataGrid.Columns.Clear();
                foreach (var column in Columns)
                {
                    // Create a new instance of each column to avoid sharing issues
                    if (column is DataGridTextColumn textColumn)
                    {
                        var newColumn = new DataGridTextColumn
                        {
                            Header = textColumn.Header,
                            Width = textColumn.Width
                        };

                        // We need to clone the binding as well
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
                    // Add handling for other column types if needed
                }

                System.Diagnostics.Debug.WriteLine($"Columns added: {MyDataGrid.Columns.Count}");

                // Check if ItemsSource is correctly set
                if (MyDataGrid.ItemsSource != null)
                {
                    var count = 0;
                    foreach (var item in MyDataGrid.ItemsSource)
                    {
                        count++;
                        if (count == 1)
                        {
                            System.Diagnostics.Debug.WriteLine($"First item type: {item.GetType().Name}");
                        }
                    }
                    System.Diagnostics.Debug.WriteLine($"Items count: {count}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ItemsSource is null");
                }
            }
        }

        // 1. ItemsSource Dependency Property
        // 1. ItemsSource Dependency Property with callback
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UC_SearchableDataGridView;
            if (control != null && control.MyDataGrid != null)
            {
                // This is the key part - explicitly setting the DataGrid's ItemsSource
                control.MyDataGrid.ItemsSource = e.NewValue as IEnumerable;

                // Debug info
                if (e.NewValue != null)
                {
                    System.Diagnostics.Debug.WriteLine($"ItemsSource updated with data of type: {e.NewValue.GetType().Name}");
                    // Try to count items
                    if (e.NewValue is IEnumerable items)
                    {
                        int count = 0;
                        foreach (var item in items)
                        {
                            count++;
                            if (count == 1)
                            {
                                System.Diagnostics.Debug.WriteLine($"First item type: {item.GetType().Name}");
                            }
                        }
                        System.Diagnostics.Debug.WriteLine($"ItemsSource contains {count} items");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ItemsSource was set to null");
                }
            }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // 2. Columns Dependency Property
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                "Columns",
                typeof(ObservableCollection<DataGridColumn>),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null, OnColumnsChanged));

        public ObservableCollection<DataGridColumn> Columns
        {
            get { return (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Callback method for when the Columns Dependency Property changes
        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UC_SearchableDataGridView;
            if (control != null && control.MyDataGrid != null)
            {
                // Clear existing columns from the internal DataGrid
                control.MyDataGrid.Columns.Clear();

                // Add new columns if the new value is not null
                if (e.NewValue is ObservableCollection<DataGridColumn> newColumns)
                {
                    foreach (var column in newColumns)
                    {
                        // Create new instances to avoid sharing
                        if (column is DataGridTextColumn textColumn)
                        {
                            var newColumn = new DataGridTextColumn
                            {
                                Header = textColumn.Header,
                                Width = textColumn.Width
                            };

                            // Clone the binding
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

                            control.MyDataGrid.Columns.Add(newColumn);
                        }
                        // Add handling for other column types if needed
                    }
                }
            }
        }

        // Other dependency properties remain the same...
        public static readonly DependencyProperty SearchQueryProperty =
            DependencyProperty.Register(
                "SearchQuery",
                typeof(string),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(string.Empty));

        public string SearchQuery
        {
            get { return (string)GetValue(SearchQueryProperty); }
            set { SetValue(SearchQueryProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                "SearchCommand",
                typeof(ICommand),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchToolTipProperty =
            DependencyProperty.Register(
                "SearchToolTip",
                typeof(string),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata("Enter search term"));

        public string SearchToolTip
        {
            get { return (string)GetValue(SearchToolTipProperty); }
            set { SetValue(SearchToolTipProperty, value); }
        }

        public static readonly DependencyProperty FilterOptionsCommandProperty =
            DependencyProperty.Register(
                "FilterOptionsCommand",
                typeof(ICommand),
                typeof(UC_SearchableDataGridView),
                new PropertyMetadata(null));

        public ICommand FilterOptionsCommand
        {
            get { return (ICommand)GetValue(FilterOptionsCommandProperty); }
            set { SetValue(FilterOptionsCommandProperty, value); }
        }
    }
}