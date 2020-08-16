using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Final
{
    /// <summary>
    /// Interaction logic for DatabaseWindow.xaml
    /// </summary>
    public partial class DatabaseWindow : Window
    {
        private DatabaseManager dbManager;
        public DatabaseWindow(DatabaseManager dbManager)
        {
            this.dbManager = dbManager;
            InitializeComponent();
            ShowDatabaseContents.ItemsSource = dbManager.GetMailingListRecipients();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ShowDatabaseContents.ItemsSource);
            view.Filter = UserFilter;
        }

        private void LastNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ShowDatabaseContents.ItemsSource).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(LastNameFilter.Text))
                return true;
            else
                return ((item as MailingListRecipient).Last_Name.IndexOf(LastNameFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void clearMenuOption_onMouseClick(object sender, RoutedEventArgs e)
        {
            dbManager.ClearDatabase();
            ShowDatabaseContents.ItemsSource = dbManager.GetMailingListRecipients();
        }

    }
}
