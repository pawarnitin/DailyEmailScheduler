using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
namespace ActiveAccount
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblmsg.Text = "Account Acitviated Successfully";

            string email = Request.QueryString["EmailAddress"];

            SqlConnection con;            
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                con = new SqlConnection(strcon);
                con.Open();
                string status = "true";
                SqlCommand cmd = new SqlCommand("update Emp_Info set EmailStatus=" + "'" + status + "'" + "where EmailId=" + "'" + email + "'", con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
            }
        }
    }
}