using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DailyEmailScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> mailAddresses = GetAddresses();
            List<string> distinct = mailAddresses.Distinct().ToList();
            if (distinct.ToList().Count() > 0)
            {
                SendMail(distinct);
            }
        }
        static List<string> GetAddresses()
        {
            SqlConnection con;
            SqlDataReader reader;
            List<string> addresses = new List<string>();
            try
            {
                con = new SqlConnection(Properties.Settings.Default.ConnectionString);
                con.Open();

                string status = "false";
                reader = new SqlCommand("select EmailAddress from Emp_Info where EmailStatus=" + "'" + status + "'", con).ExecuteReader();

                foreach (var row in reader)
                {
                    string mailaddress = Convert.ToString(reader.GetValue(0));
                    if (mailaddress != "")
                        addresses.Add(mailaddress);
                }
            }
            catch (Exception ex)
            {               
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
            }
            return addresses;
        }

        public static void SendMail(List<string> mailAddresses)
        {
            foreach (string email in mailAddresses)
            {
                try
                {
                    //Console.WriteLine("Method call at" + DateTime.Now);
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("Sender mail address"); // Give sender mail address Ex. asdf@gmail.com                    
                    mail.To.Add(email);  // Give receiver mail address Ex. asdf@gmail.com
                    mail.Subject = "Test Mail";
                    var body = new StringBuilder();
                    body.AppendFormat("Hello Sir/Madam, {0}\n", email);
                    body.AppendLine(@"Your ABC Account about to activate click 
                        the link below to complete the actination process");
                    //body.AppendLine("<a href=\"http://localhost:49496/Activated.aspx\">login</a>");
                    body.AppendLine("<a href=\"http://localhost:63099/Default?EmailAddress=" + email+"\">login</a>");
                    mail.Body = body.ToString();

                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.EnableSsl = true;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("Sender mail address", "Password");
                    SmtpServer.Send(mail);
                    Console.WriteLine("Email Sent Successfully To : {0}", email);
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(string.Format("Email sent unsucessfully..!{0}.", ex.Message));
                    //Console.ReadLine();
                }
            }
        }
    }
}
