using HospitalManagementSystem.WPF.ViewModels.Admin;
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

namespace HospitalManagementSystem.WPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddNewStaffMemberView.xaml
    /// </summary>
    public partial class AddNewStaffMemberView : Window
    {
        public AddNewStaffMemberView(AddNewStaffMemberViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Save logic in future
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
