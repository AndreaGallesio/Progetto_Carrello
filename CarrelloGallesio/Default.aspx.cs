using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using adoNetWebSQlServer;

namespace CarrelloGallesio
{
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                adoNet.impostaConnessione("App_Data/DBCarrello.mdf");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser1.Text != string.Empty)
            {
                if (txtPassword.Text != string.Empty)
                {
                    adoNet con = new adoNet();
                    con.cmd.Parameters.AddWithValue("@User", txtUser1.Text);
                    DataTable account = con.eseguiQuery(
                        @"SELECT * FROM Account WHERE UsernameAccount = @User", CommandType.Text);
                    if (account.Rows.Count > 0)
                    {
                        if (account.Rows[0]["PasswordAccount"].ToString() == txtPassword.Text)
                        {
                            Session["account"] = account.Rows[0];
                            con.cmd.Parameters.AddWithValue("@User", account.Rows[0]["UsernameAccount"].ToString());
                            switch (con.eseguiScalar(
                                @"SELECT TipoAccount.Nome 
                                  FROM TipoAccount INNER JOIN Account
                                    ON TipoAccount.Id = Account.TipoAccount
                                    WHERE Account.UsernameAccount = @User
                                  ", CommandType.Text))
                            {
                                case "ADMIN":
                                    Response.Redirect("AdminPage.aspx");
                                    break;
                                case "FORNITORE":
                                    Response.Redirect("Fornitore.aspx");
                                    break;
                                case "USER":
                                    Session["page"] = 0;
                                    Response.Redirect("UserPage.aspx");
                                    break;
                            }

                        }
                    }
                    else
                    {
                        lblErrore.Text = "errore";
                    }
                }
                else
                    lblErrore.Text = "password mancante";
            }
            else
                lblErrore.Text = "username mancante";
        }
    }
}