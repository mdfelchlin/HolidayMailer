using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Final
{
    /// <summary>
    /// Interaction logic for AddRecipient.xaml
    /// </summary>
    public partial class AddRecipient : Window
    {
        private DatabaseManager dbManager;

        public AddRecipient(DatabaseManager dbManager)
        {
            InitializeComponent();
            this.dbManager = dbManager;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dbManager.AddToMailingList(
                new MailingListRecipient
                {
                    First_Name = FirstNameInput.Text,
                    Last_Name = LastNameInput.Text,
                    Email = EmailInput.Text,
                    HaveSentMail = HasSentMailInput.IsEnabled
                });
            Close();
        }

    }
}
