using Microsoft.Win32;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Media;

namespace _01_smtp
{
    public partial class MainWindow : Window
    {
        const string myEmail = "prodoq@gmail.com";
        const string myPassword = "bfcosbbxkfeuiokd";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AttachFile(MailMessage mail)
        {
            var result = MessageBox.Show("Do you want to attach a file?", "Attach File", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    mail.Attachments.Add(new Attachment(dialog.FileName));
                }
            }
        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            // --------------- send email using SMTP ---------------

            // create mail message object
            MailMessage mail = new MailMessage(myEmail, toTxtBox.Text)
            {
                Subject = subjectTxtBox.Text,
                Body = $"<h1>Hello HTML in mail</h1><p>{bodyTxtBox.Text}</p>",
                IsBodyHtml = true,
                Priority = MailPriority.High
            };

            // add attachments
            AttachFile(mail);

            // send email with SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(myEmail, myPassword)
            };

            client.SendCompleted += Client_SendCompleted;

            try
            {
                client.Send(mail);
                sendBtn.Background = Brushes.LightGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Mail sent!");
        }
    }
}
