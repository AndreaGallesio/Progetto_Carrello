using adoNetWebSQlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using adoNetWebSQlServer;
using System.Data.Common;
using System.Xml.Linq;
using System.Data.Odbc;

namespace CarrelloGallesio
{
    public partial class Fornitore : System.Web.UI.Page
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
                    Session["idFornitore"] = Convert.ToInt32(sqlConnection.eseguiScalar($"select Id from Fornitori where Account= 3", CommandType.Text));
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
        protected void ddlCategorie_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["pagina"] = 0;
            DropDownList ddlCategorie = sender as DropDownList;
            int categoria = Convert.ToInt32(ddlCategorie.SelectedValue);
            if (categoria == 0)
                caricaProdotti();
            else
                caricaProdotti(categoria);
        }

        private void caricaProdotti()
        {
            container.Controls.Clear();
            sqlConnection = new adoNet();
            if (Session["account"] is DataRow account)
            {
                sqlConnection.cmd.Parameters.AddWithValue("@Fornitore", Session["IdFornitore"].ToString());
                DataTable prodottiFornitore = sqlConnection.eseguiQuery(
                        "SELECT * from Prodotti where Fornitore = @Fornitore",
                        CommandType.Text
                    );
                if (prodottiFornitore.Rows.Count > 0)
                {
                    HtmlGenericControl tabellaProdotti = new HtmlGenericControl("table");
                    tabellaProdotti.Attributes.Add("class", "table table-hover text-white");
                    tabellaProdotti.Attributes.Add("id", "tabellaProdotti");
                    container.Controls.Add(tabellaProdotti);
                    HtmlGenericControl thead = new HtmlGenericControl("thead");
                    thead.Attributes.Add("class", "text-white");
                    tabellaProdotti.Controls.Add(thead);
                    HtmlGenericControl tr = new HtmlGenericControl("tr");
                    thead.Controls.Add(tr);
                    HtmlGenericControl th;
                    foreach (DataColumn colonna in prodottiFornitore.Columns)
                    {
                        if (
                            colonna.ColumnName != "Id" &&
                            colonna.ColumnName != "Fornitore" &&
                            colonna.ColumnName != "Validita"
                        )
                        {
                            th = new HtmlGenericControl("th")
                            {
                                InnerText = colonna.ColumnName
                            };
                            if (colonna.ColumnName == "Prezzo")
                               
                            th.Attributes.Add("class", "text-center");
                            tr.Controls.Add(th);
                        }
                    }
                    th = new HtmlGenericControl("th")
                    {
                        InnerText = "Gestione"
                    };
                    th.Attributes.Add("class", "text-center");
                    tr.Controls.Add(th);
                    HtmlGenericControl tbody = new HtmlGenericControl("tbody");
                    tbody.Attributes.Add("class", "text-white");
                    tabellaProdotti.Controls.Add(tbody);
                    HtmlGenericControl td;
                    foreach (DataRow prodotto in prodottiFornitore.Rows)
                    {
                        tr = new HtmlGenericControl("tr");
                        if (!(bool)prodotto["Validita"])
                            tr.Attributes.Add("style", "opacity:0.5");
                        tbody.Controls.Add(tr);
                        foreach (DataColumn colonna in prodottiFornitore.Columns)
                        {
                            if (
                                colonna.ColumnName != "Id" &&
                                colonna.ColumnName != "Fornitore" &&
                                colonna.ColumnName != "Validita"
                            )
                            {
                                if (colonna.ColumnName != "Immagine")
                                {
                                    td = new HtmlGenericControl("td");
                                    if (colonna.ColumnName != "Categoria")
                                        td.InnerText = prodotto[colonna.ColumnName].ToString();
                                    else
                                    {
                                        var nomeCategoria = sqlConnection.eseguiScalar(
                                            $@"SELECT Descrizione 
                                               from Categorie 
                                               where Id={prodotto[colonna.ColumnName]}",
                                            CommandType.Text);
                                        td.InnerText = nomeCategoria;
                                    }
                                    td.Attributes.Add("class", "text-center align-middle");
                                    tr.Controls.Add(td);
                                }
                                else
                                {
                                    td = new HtmlGenericControl("td");
                                    tr.Controls.Add(td);
                                    HtmlGenericControl img = new HtmlGenericControl("img");
                                    img.Attributes.Add("src", prodotto[colonna.ColumnName].ToString());
                                    img.Attributes.Add("class", "img-thumbnail");
                                    img.Attributes.Add("style", "height: 200px");
                                    td.Attributes.Add("class", "d-flex justify-content-center");
                                    td.Controls.Add(img);
                                }
                            }
                        }
                        td = new HtmlGenericControl("td");
                        td.Attributes.Add("style", "width: min-content");
                        tr.Controls.Add(td);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "btn-group d-flex justify-content-center align-items-center");
                        div.Attributes.Add("style", "margin-top:25%; transform: translate(0, -25%);");
                        td.Controls.Add(div);                        
                    }
                }
                else
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "alert alert-info");
                    div.InnerHtml = "Non hai ancora inserito nessun prodotto";
                    container.Controls.Add(div);
                }
            }
        }

        private void caricaProdotti(int categoria)
        {
            container.Controls.Clear();
            adoNet sqlConnection = new adoNet();
            if (Session["account"] is DataRow account)
            {
                sqlConnection.cmd.Parameters.AddWithValue("@Fornitore", Session["IdFornitore"].ToString());
                sqlConnection.cmd.Parameters.AddWithValue("@Categoria", categoria);
                DataTable prodottiCategoria = sqlConnection.eseguiQuery(
                    "SELECT * FROM Prodotti WHERE Fornitore = @Fornitore AND Categoria = @Categoria",
                    CommandType.Text
                );
                if (prodottiCategoria.Rows.Count > 0)
                {
                    HtmlGenericControl tabellaProdotti = new HtmlGenericControl("table");
                    tabellaProdotti.Attributes.Add("class", "table table-hover text-white");
                    tabellaProdotti.Attributes.Add("id", "tabellaProdotti");
                    container.Controls.Add(tabellaProdotti);
                    HtmlGenericControl thead = new HtmlGenericControl("thead");
                    thead.Attributes.Add("class", "text-white");
                    tabellaProdotti.Controls.Add(thead);
                    HtmlGenericControl tr = new HtmlGenericControl("tr");
                    thead.Controls.Add(tr);
                    HtmlGenericControl th;
                    foreach (DataColumn colonna in prodottiCategoria.Columns)
                    {
                        if (
                            colonna.ColumnName != "Id" &&
                            colonna.ColumnName != "Fornitore" &&
                            colonna.ColumnName != "Validita"
                        )
                        {
                            th = new HtmlGenericControl("th")
                            {
                                InnerText = colonna.ColumnName
                            };
                            if (colonna.ColumnName == "Prezzo")
                                th.Attributes.Add("class", "text-center");
                            tr.Controls.Add(th);
                        }
                    }
                    th = new HtmlGenericControl("th")
                    {
                        InnerText = "Gestione"
                    };
                    th.Attributes.Add("class", "text-center");
                    tr.Controls.Add(th);
                    HtmlGenericControl tbody = new HtmlGenericControl("tbody");
                    tbody.Attributes.Add("class", "text-white");
                    tabellaProdotti.Controls.Add(tbody);
                    HtmlGenericControl td;
                    foreach (DataRow prodotto in prodottiCategoria.Rows)
                    {
                        tr = new HtmlGenericControl("tr");
                        if (!(bool)prodotto["Validita"])
                            tr.Attributes.Add("style", "opacity:0.5");
                        tbody.Controls.Add(tr);
                        foreach (DataColumn colonna in prodottiCategoria.Columns)
                        {
                            if (
                                colonna.ColumnName != "Id" &&
                                colonna.ColumnName != "Fornitore" &&
                                colonna.ColumnName != "Validita"
                            )
                            {
                                if (colonna.ColumnName != "Immagine")
                                {
                                    td = new HtmlGenericControl("td");
                                    if (colonna.ColumnName != "Categoria")
                                        td.InnerText = prodotto[colonna.ColumnName].ToString();
                                    else
                                    {
                                        var nomeCategoria = sqlConnection.eseguiScalar(
                                            $@"SELECT Descrizione 
                                       FROM Categorie 
                                       WHERE Id={prodotto[colonna.ColumnName]}",
                                            CommandType.Text
                                        );
                                        td.InnerText = nomeCategoria;
                                    }
                                    td.Attributes.Add("class", "text-center align-middle");
                                    tr.Controls.Add(td);
                                }
                                else
                                {
                                    td = new HtmlGenericControl("td");
                                    tr.Controls.Add(td);
                                    HtmlGenericControl img = new HtmlGenericControl("img");
                                    img.Attributes.Add("src", prodotto[colonna.ColumnName].ToString());
                                    img.Attributes.Add("class", "img-thumbnail");
                                    img.Attributes.Add("style", "height: 200px");
                                    td.Attributes.Add("class", "d-flex justify-content-center");
                                    td.Controls.Add(img);
                                }
                            }
                        }
                        td = new HtmlGenericControl("td");
                        td.Attributes.Add("style", "width: min-content");
                        tr.Controls.Add(td);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "btn-group d-flex justify-content-center align-items-center");
                        div.Attributes.Add("style", "margin-top:25%; transform: translate(0, -25%);");
                        td.Controls.Add(div);
                    }
                }
                else
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "alert alert-info");
                    div.InnerHtml = "Non hai ancora inserito nessun prodotto per questa categoria";
                    container.Controls.Add(div);
                }
            }
        }

        protected void btnAggiungi_Click(object sender, EventArgs e)
        {
            Response.Redirect("Aggiungi.aspx");
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Modifica.aspx");
        }
    }
}