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
    public partial class AggiungiAdmin : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategories();
            }
        }
        private void BindCategories()
        {
            adoNet adoWeb = new adoNet();
            string query = "SELECT DISTINCT C.Id, C.Descrizione " +
                   "FROM Categorie AS C " +
                   "INNER JOIN Prodotti AS P ON C.Id = P.Categoria " +
                   "WHERE P.Validita = 1";

            DataTable categorie = adoWeb.eseguiQuery(query, CommandType.Text);

            cmbCategorie.DataSource = categorie;
            cmbCategorie.DataTextField = "Descrizione";
            cmbCategorie.DataValueField = "Id";
            cmbCategorie.DataBind();
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
                ((FileUpload)this.FindControl("imagePicker")).SaveAs(Server.MapPath("~/img/") + ((FileUpload)this.FindControl("imagePicker")).FileName);
                sqlConnection.cmd.Parameters.AddWithValue(
                    "@nome",
                    ((TextBox)this.FindControl("txtNomeProdotto")).Text);
                sqlConnection.cmd.Parameters.AddWithValue(
                    "@prezzo",
                    Convert.ToDecimal(((TextBox)this.FindControl("txtPrezzo")).Text));
                sqlConnection.cmd.Parameters.AddWithValue(
                    "@descrizione",
                    ((TextBox)this.FindControl("txtDescrizione")).Text);
                sqlConnection.cmd.Parameters.AddWithValue(
                    "@categoria",
                    int.Parse(((DropDownList)this.FindControl("cmbCategorie")).SelectedValue));
                sqlConnection.cmd.Parameters.AddWithValue(
                "@fornitore",
                    idFornitore);
                sqlConnection.cmd.Parameters.AddWithValue(
                    "@immagine",
                    "Img/" + ((FileUpload)this.FindControl("imagePicker")).FileName);



                sqlConnection.eseguiNonQuery(
                    @"insert into Prodotti (Fornitore, NomeProdotto, Categoria, Prezzo, Descrizione, Immagine) 
                          values (@fornitore, @nome, @categoria, @prezzo, @descrizione, @immagine)",
                    CommandType.Text);

                Session["page"] = 0;
                Response.Redirect("AdminPage.aspx");
            }
        }
        private bool controllaCampi()
        {
            if (((TextBox)this.FindControl("txtNomeProdotto")).Text != string.Empty)
            {
                if (((DropDownList)this.FindControl("cmbCategorie")).SelectedValue != "0")
                {
                    if (((FileUpload)this.FindControl("imagePicker")).HasFile)
                    {
                        if (((TextBox)this.FindControl("txtPrezzo")).Text != string.Empty)
                        {
                            if (((TextBox)this.FindControl("txtDescrizione")).Text != string.Empty)
                            {
                                return true;
                            }
                            else
                                ((Label)this.FindControl("lblErrore")).Text = "Inserisci una descrizione";
                        }
                        else
                            ((Label)this.FindControl("lblErrore")).Text = "Inserisci un prezzo";
                    }
                    else
                        ((Label)this.FindControl("lblErrore")).Text = "Seleziona un'immagine";
                }
                else
                    ((Label)this.FindControl("lblErrore")).Text = "Seleziona una categoria";

            }
            return false;
        }
    }
}