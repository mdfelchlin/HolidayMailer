using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SQLite;

namespace Final
{
    public class DatabaseManager
    {
        private SQLiteConnection sqlite_conn;
        private SQLiteCommand sqlite_cmd;
        private SQLiteDataReader sqlite_datareader;

        private DbSet<MailingListRecipient> DBMailingList { get; set; }

        public DatabaseManager()
        {
            sqlite_conn = new SQLiteConnection("Data Source=holidayMailer.db;Version=3;New=True;Compress=True;");
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            
            sqlite_cmd.CommandText = 
                "CREATE TABLE if not exists " +
                "MailingList (" +
                "First_Name varchar(30), " +
                "Last_Name varchar(30), " +
                "Email varchar(200), " +
                "HaveSentMail int" +
                ");";

            sqlite_cmd.ExecuteNonQuery();
        }

        public void AddToMailingList(MailingListRecipient mailingListRecipient)
        {
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText =
                $"INSERT INTO MailingList (First_Name, Last_Name, Email, HaveSentMail) " +
                $"VALUES " +
                $"('{mailingListRecipient.First_Name}'," +
                $" '{mailingListRecipient.Last_Name}', " +
                $" '{mailingListRecipient.Email}', " +
                $"'{ Convert.ToInt32(mailingListRecipient.HaveSentMail) }');";
            sqlite_cmd.ExecuteNonQuery();
        }

        public ObservableCollection<MailingListRecipient> GetMailingListRecipients()
        {
            ObservableCollection<MailingListRecipient> mailingListRecipients = new ObservableCollection<MailingListRecipient>();

            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM MailingList ORDER BY Last_Name";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                mailingListRecipients.Add(new MailingListRecipient()
                {
                    First_Name = (string) sqlite_datareader["First_Name"],
                    Last_Name = (string) sqlite_datareader["Last_Name"],
                    Email = (string) sqlite_datareader["Email"],
                    HaveSentMail = ((int) sqlite_datareader["HaveSentMail"]) != 0
                });
            }

            return mailingListRecipients;
        }

        public List<string> GetEmailList()
        {
            List<string> emailList = new List<string>();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT Email FROM MailingList";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
                emailList.Add((string)sqlite_datareader["Email"]);

            return emailList;
        }

        public void ClearDatabase()
        {
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM MailingList";
            sqlite_cmd.ExecuteNonQuery();
        }

        public void OnExit()
        {
            sqlite_conn.Close();
        }
    }
}
