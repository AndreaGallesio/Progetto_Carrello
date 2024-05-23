using adoNetWebSQlServer;
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
    public partial class AggiungiCategoria : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (controllaCampi())
            {
                sqlConnection = new adoNet();
                int idFornitore = int.Parse(
                    sqlConnection.eseguiScalar(
                        $"select IdAccount from Account where TipoAccount = 3",
                        CommandType.Text)
                    );

                sqlConnection.cmd.Parameters.AddWithValue(
                "@descrizione",
                ((TextBox)this.FindControl("txtNomeCategoria")).Text);



                sqlConnection.eseguiNonQuery(
                    @"insert into Categorie (Descrizione) 
                          values (@descrizione)",
                    CommandType.Text);

                Session["page"] = 0;
                Response.Redirect("AdminPage.aspx");
            }
        }
        private bool controllaCampi()
        {
            if (((TextBox)this.FindControl("txtNomeCategoria")).Text != string.Empty)
            {
                return true;
            }
            else
                ((Label)this.FindControl("lblErrore")).Text = "Inserisci una descrizione";

            return false;
        }
    }
}