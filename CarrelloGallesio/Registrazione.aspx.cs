using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using adoNetWebSQlServer;

namespace CarrelloGallesio
{
    public partial class Registrazione : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                adoNet.impostaConnessione("App_Data/DBCarrello.mdf");
            }
        }

        protected void btnRegistra_Click(object sender, EventArgs e)
        {
            sqlConnection = new adoNet();

            sqlConnection.cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
            sqlConnection.cmd.Parameters.AddWithValue("@Cognome", txtCognome.Text);
            sqlConnection.cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
            sqlConnection.cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
            sqlConnection.cmd.Parameters.AddWithValue("@Passoword", txtPassword.Text);
            sqlConnection.cmd.Parameters.AddWithValue("@Tipo", ddlTipoAccount.SelectedValue);
            sqlConnection.cmd.Parameters.AddWithValue("@Validita", ddlValiditaAccount.SelectedValue);
            sqlConnection.eseguiNonQuery(@"INSERT INTO Account(NomeAccount, CognomeAccount, MailAccount, UsernameAccount, PasswordAccount, TipoAccount, ValiditaAccount) 
                          VALUES (@Nome, @Cognome, @Email, @Username, @Passoword, @Tipo, @Validita)",CommandType.Text);
            
            sqlConnection.cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
            sqlConnection.cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
            DataRow account = sqlConnection.eseguiQuery("SELECT * FROM Account where UsernameAccount = @Username and PasswordAccount = @Password", CommandType.Text).Rows[0];
            Session["account"] = account;

            Response.Redirect("UserPage.aspx");
        }

        protected void txtCognome_TextChanged(object sender, EventArgs e)
        {

        }
    }
}