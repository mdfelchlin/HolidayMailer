using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Forms;

namespace Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseManager dbManager = new DatabaseManager();
        private AddRecipient addRecipientView;
        private Attachment attachment;
        private DatabaseWindow databaseView;

        public MainWindow()
        {
            InitializeComponent();
            addRecipientView = new AddRecipient(dbManager);
            databaseView = new DatabaseWindow(dbManager);
            RemoveFile.Visibility = Visibility.Hidden;
        }

        private void MenuAddOption_Click(object sender, RoutedEventArgs e)
        {
            addRecipientView = new AddRecipient(dbManager);
            addRecipientView.Show();
        }

        private void MenuViewOption_Click(object sender, RoutedEventArgs e)
        {
            databaseView = new DatabaseWindow(dbManager);
            databaseView.Show();
        }

        private void MenuAboutOption_onClick(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(
                "Usage:" +
                "\n The goal of this program is to be able to send emails" +
                "\n to all your friends and family over the holidays." +
                "\n\n VERSION: 1.0.0" +
                "\n\n AUTHOR: Mark Felchlin",
                "About"
                );
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var emailList = "";

            if (AllRecipients.IsChecked == true)
                emailList = String.Join(",", dbManager.GetMailingListRecipients().Select(x => x.Email));
            else
                emailList = String.Join(",", dbManager.GetMailingListRecipients().Where(x => x.HaveSentMail).Select(x => x.Email));

            if (emailList != "")
            {
                MailMessage message = new MailMessage(
                    "mdfelchlin@gmail.com",
                    emailList
                    )
                {
                    Body = BodyInput.Text,
                    Subject = SubjectInput.Text
                };

                if (attachment != null)
                    message.Attachments.Add(attachment);

                try
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new System.Net.NetworkCredential("mdfelchlin@gmail.com", "ComputerScienceDegree"),
                        EnableSsl = true
                    };
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                        ex.ToString());
                }
                System.Windows.MessageBox.Show("Holiday Mail Sent", "Sent");
                ResetScreen();
            }
            else
                System.Windows.MessageBox.Show("No Recipients in your database please add people to be able to send mail", "Where is everyone?");
        }

        private void ResetScreen()
        {
            attachment = null;
            AttatchButton.IsEnabled = true;
            RemoveFile.Visibility = Visibility.Hidden;
            FileAttatchementLabel.Content = "";
            BodyInput.Text = "";
            SubjectInput.Text = "";
        }

        private void AttatchButton_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    attachment = new Attachment(openFileDialog.FileName);
                    RemoveFile.Visibility = Visibility.Visible;
                    AttatchButton.IsEnabled = false;
                    FileAttatchementLabel.Content = attachment.Name;
                }
            }
        }

        private void windowClose_onClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {
            attachment = null;
            AttatchButton.IsEnabled = true;
            RemoveFile.Visibility = Visibility.Hidden;
            FileAttatchementLabel.Content = "";
        }
    }
}
