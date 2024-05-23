using adoNetWebSQlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using adoNetWebSQlServer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;

namespace CarrelloGallesio
{
    public partial class AdminPage : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["account"] is DataRow account)
            {
                if (!IsPostBack)
                {
                    sqlConnection = new adoNet();                    
                    lblNomeUser.Text = (account["CognomeAccount"].ToString()).ToUpper() + " " + (account["NomeAccount"].ToString()).ToUpper();

                    GetCategorieFromDatabase();

                    if (categoria == 0)
                        caricaProdotti();
                    else
                        caricaProdotti(categoria);
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["account"] = null;
            Response.Redirect("default.aspx");
        }
        private void GetCategorieFromDatabase()
        {
            adoNet adoWeb = new adoNet();
            string query = "SELECT DISTINCT C.Id, C.Descrizione " +
                   "FROM Categorie AS C " +
                   "INNER JOIN Prodotti AS P ON C.Id = P.Categoria " +
                   "WHERE P.Validita = 1";

            DataTable categorie = adoWeb.eseguiQuery(query, CommandType.Text);

            ddlCategorie.DataSource = categorie;
            ddlCategorie.DataTextField = "Descrizione";
            ddlCategorie.DataValueField = "Id";
            ddlCategorie.DataBind();

            ddlCategorie.Items.Insert(0, new ListItem("Tutte le Categorie", "0"));
        }
        protected void ddlCategorie_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DropDownList ddlCategorie = sender as DropDownList;
            int categoria = Convert.ToInt32(ddlCategorie.SelectedValue);
            if (categoria == 0)
                caricaProdotti();
            else
                caricaProdotti(categoria);
        }
        private void caricaProdotti()
        {
            sqlConnection = new adoNet();
            DataTable prodotti = sqlConnection.eseguiQuery("SELECT Prodotti.*, Fornitori.NomeFornitore, Categorie.Descrizione AS NomeCategoria FROM Prodotti INNER JOIN Fornitori ON Prodotti.Fornitore = Fornitori.Id INNER JOIN Categorie ON Prodotti.Categoria = Categorie.Id WHERE Prodotti.Validita=1", CommandType.Text);
            if (prodotti.Rows.Count == 0)
            {
                container.Controls.Clear();
                HtmlGenericControl p = new HtmlGenericControl("h3");
                p.Attributes.Add("class", "text-white mt-5 text-center");
                p.InnerHtml = "Non sono presenti prodotti!";
                container.Controls.Add(p);
            }
            else
            {
                HtmlGenericControl table = new HtmlGenericControl("table");
                table.Attributes.Add("class", "table table-hover text-white");
                container.Controls.Add(table);

                HtmlGenericControl thead = new HtmlGenericControl("thead");
                table.Controls.Add(thead);

                HtmlGenericControl trHead = new HtmlGenericControl("tr");
                thead.Controls.Add(trHead);

                // Definizione delle colonne della tabella
                string[] colonne = { "Fornitore", "Nome Prodotto", "Descrizione", "Immagine", "Categoria", "Prezzo" };
                foreach (var colonna in colonne)
                {
                    HtmlGenericControl th = new HtmlGenericControl("th");
                    th.InnerText = colonna;
                    trHead.Controls.Add(th);
                }

                HtmlGenericControl tbody = new HtmlGenericControl("tbody");
                table.Controls.Add(tbody);

                foreach (DataRow prodotto in prodotti.Rows)
                {
                    HtmlGenericControl tr = new HtmlGenericControl("tr");
                    tbody.Controls.Add(tr);

                    // Colonna Fornitore
                    HtmlGenericControl tdFornitore = new HtmlGenericControl("td");
                    tdFornitore.InnerText = prodotto["NomeFornitore"].ToString(); // Modificato per mostrare il nome del fornitore
                    tr.Controls.Add(tdFornitore);

                    // Colonna Nome Prodotto
                    HtmlGenericControl tdNome = new HtmlGenericControl("td");
                    tdNome.InnerText = prodotto["NomeProdotto"].ToString();
                    tr.Controls.Add(tdNome);

                    // Colonna Descrizione
                    HtmlGenericControl tdDesc = new HtmlGenericControl("td");
                    tdDesc.InnerText = prodotto["Descrizione"].ToString();
                    tr.Controls.Add(tdDesc);

                    // Colonna Immagine
                    HtmlGenericControl tdImg = new HtmlGenericControl("td");
                    HtmlGenericControl img = new HtmlGenericControl("img");
                    img.Attributes.Add("src", prodotto["Immagine"].ToString());
                    img.Attributes.Add("class", "img-fluid");
                    img.Attributes.Add("style", "width: 100px; height: auto;");
                    tdImg.Controls.Add(img);
                    tr.Controls.Add(tdImg);

                    // Colonna Categoria
                    HtmlGenericControl tdCategoria = new HtmlGenericControl("td");
                    tdCategoria.InnerText = prodotto["NomeCategoria"].ToString(); // Modificato per mostrare il nome della categoria
                    tr.Controls.Add(tdCategoria);

                    // Colonna Prezzo
                    HtmlGenericControl tdPrezzo = new HtmlGenericControl("td");
                    tdPrezzo.InnerText = $"{prodotto["Prezzo"]} €";
                    tr.Controls.Add(tdPrezzo);

                    // Colonna Azione - Se necessario, aggiungere qui il codice per la colonna Azione
                }
            }
        }


        private void caricaProdotti(int categoria)
        {
            sqlConnection = new adoNet();
            sqlConnection.cmd.Parameters.AddWithValue("@categoria", categoria);

            DataTable prodotti = sqlConnection.eseguiQuery("SELECT Prodotti.*, Fornitori.NomeFornitore AS NomeFornitore, Categorie.Descrizione AS NomeCategoria " +
    "FROM Prodotti " +
    "INNER JOIN Fornitori ON Prodotti.Fornitore = Fornitori.Id " +
    "INNER JOIN Categorie ON Prodotti.Categoria = Categorie.Id " +
    "WHERE Prodotti.Categoria = @categoria AND Prodotti.Validita = 1", CommandType.Text);
            if (prodotti.Rows.Count == 0)
            {
                container.Controls.Clear();
                HtmlGenericControl p = new HtmlGenericControl("h3");
                p.Attributes.Add("class", "text-white mt-5 text-center");
                p.InnerHtml = "Non sono presenti prodotti di questa categoria!";
                container.Controls.Add(p);
            }
            else
            {
                HtmlGenericControl table = new HtmlGenericControl("table");
                table.Attributes.Add("class", "table");
                container.Controls.Add(table);

                // Creazione dell'intestazione della tabella
                HtmlGenericControl thead = new HtmlGenericControl("thead");
                table.Controls.Add(thead);
                HtmlGenericControl trHead = new HtmlGenericControl("tr");
                thead.Controls.Add(trHead);
                string[] colonne = { "Fornitore", "Nome Prodotto", "Descrizione", "Immagine", "Categoria", "Prezzo" };
                foreach (var colonna in colonne)
                {
                    HtmlGenericControl th = new HtmlGenericControl("th");
                    th.InnerText = colonna;
                    trHead.Controls.Add(th);
                }

                // Creazione del corpo della tabella
                HtmlGenericControl tbody = new HtmlGenericControl("tbody");
                table.Controls.Add(tbody);

                foreach (DataRow prodotto in prodotti.Rows)
                {
                    HtmlGenericControl tr = new HtmlGenericControl("tr");
                    tbody.Controls.Add(tr);

                    // Colonna Fornitore
                    HtmlGenericControl tdFornitore = new HtmlGenericControl("td");
                    tdFornitore.InnerText = prodotto["NomeFornitore"].ToString(); // Modificato per mostrare il nome del fornitore
                    tr.Controls.Add(tdFornitore);

                    // Colonna Nome Prodotto
                    HtmlGenericControl tdNome = new HtmlGenericControl("td");
                    tdNome.InnerText = prodotto["NomeProdotto"].ToString();
                    tr.Controls.Add(tdNome);

                    // Colonna Descrizione
                    HtmlGenericControl tdDesc = new HtmlGenericControl("td");
                    tdDesc.InnerText = prodotto["Descrizione"].ToString();
                    tr.Controls.Add(tdDesc);

                    // Colonna Immagine
                    HtmlGenericControl tdImg = new HtmlGenericControl("td");
                    HtmlGenericControl img = new HtmlGenericControl("img");
                    img.Attributes.Add("src", prodotto["Immagine"].ToString());
                    img.Attributes.Add("class", "img-fluid");
                    img.Attributes.Add("style", "width: 100px; height: auto;");
                    tdImg.Controls.Add(img);
                    tr.Controls.Add(tdImg);

                    // Colonna Categoria
                    HtmlGenericControl tdCategoria = new HtmlGenericControl("td");
                    tdCategoria.InnerText = prodotto["NomeCategoria"].ToString(); // Modificato per mostrare il nome della categoria
                    tr.Controls.Add(tdCategoria);

                    // Colonna Prezzo
                    HtmlGenericControl tdPrezzo = new HtmlGenericControl("td");
                    tdPrezzo.InnerText = $"{prodotto["Prezzo"]} €";
                    tr.Controls.Add(tdPrezzo);

                    // Colonna Azione - Se necessario, aggiungere qui il codice per la colonna Azione


                }
            }
        }







        protected void btnAggiungi_Click(object sender, EventArgs e)
        {
            Response.Redirect("AggiungiAdmin.aspx");
        }

        protected void btnAggiungiCategoria_Click(object sender, EventArgs e)
        {
            Response.Redirect("AggiungiCategoria.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("EliminaProdotto.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("ModificaProdotto.aspx");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("Grafici.aspx");
        }
    }
}