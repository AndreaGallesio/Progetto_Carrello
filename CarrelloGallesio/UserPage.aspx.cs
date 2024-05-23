using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using adoNetWebSQlServer;

namespace CarrelloGallesio
{
    public partial class UserPage : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;

        protected override void OnInit(EventArgs e)
        {
            if (Session["account"] is DataRow account)
            {
                if (!IsPostBack)
                {
                    GetCategorieFromDatabase();

                    lblNomeUser.Text = (account["CognomeAccount"].ToString()).ToUpper() + " " + (account["NomeAccount"].ToString()).ToUpper();

                    if (categoria == 0)
                        caricaProdotti();
                    else
                        caricaProdotti(categoria);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["account"] is DataRow account)
            {
                if (!IsPostBack)
                {
                    GetCategorieFromDatabase();

                    lblNomeUser.Text = (account["CognomeAccount"].ToString()).ToUpper() + " " + (account["NomeAccount"].ToString()).ToUpper();

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
        private void caricaProdotti(int categoria)
        {
            sqlConnection = new adoNet();
            sqlConnection.cmd.Parameters.AddWithValue("@categoria", categoria);
            DataTable prodotti = sqlConnection.eseguiQuery("SELECT * FROM Prodotti WHERE Categoria = @categoria and Validita=1", CommandType.Text);
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
                for (int i = 0; i < prodotti.Rows.Count; i += 4)
                {
                    HtmlGenericControl divRow = new HtmlGenericControl("div");
                    divRow.Attributes.Add("class", "row");
                    container.Controls.Add(divRow);

                    for (int j = i; j < Math.Min(i + 4, prodotti.Rows.Count); j++)
                    {
                        DataRow prodotto = prodotti.Rows[j];

                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "col-lg-3 col-md-4 col-sm-6 mb-4"); // Modifica qui per card più piccole
                        divRow.Controls.Add(div);

                        HtmlGenericControl div2 = new HtmlGenericControl("div");
                        div2.Attributes.Add("class", "card h-100");
                        div.Controls.Add(div2);

                        HtmlGenericControl a = new HtmlGenericControl("a");
                        a.Attributes.Add("href", "#");
                        div2.Controls.Add(a);

                        string percorsoImmagine = prodotto["Immagine"].ToString();
                        HtmlGenericControl img = new HtmlGenericControl("img");
                        img.Attributes.Add("class", "card-img-top img-fluid");
                        img.Attributes.Add("src", percorsoImmagine);
                        img.Attributes.Add("alt", "");
                        a.Controls.Add(img);

                        HtmlGenericControl div3 = new HtmlGenericControl("div");
                        div3.Attributes.Add("class", "card-body");
                        div2.Controls.Add(div3);

                        HtmlGenericControl h4 = new HtmlGenericControl("h4");
                        h4.Attributes.Add("class", "card-title");
                        h4.InnerHtml = prodotto["NomeProdotto"].ToString();
                        div3.Controls.Add(h4);

                        HtmlGenericControl h5 = new HtmlGenericControl("h5");
                        h5.InnerHtml = prodotto["Prezzo"].ToString() + " €";
                        div3.Controls.Add(h5);

                        HtmlGenericControl p = new HtmlGenericControl("p");
                        p.Attributes.Add("class", "card-text");
                        p.InnerHtml = prodotto["Descrizione"].ToString();
                        div3.Controls.Add(p);                        


                        HtmlButton btn = new HtmlButton();
                        btn.ID = "btnCard_" + prodotto["Id"];
                        btn.Attributes.Add("class", "btn btn-primary");
                        btn.InnerText = "Aggiungi al Carrello";
                        btn.Attributes.Add("onclick", "aggiungiAlCarrello(" + prodotto["Id"].ToString() + ")");
                        div2.Controls.Add(btn);
                    }
                }
            }
        }
        private void caricaProdotti()
        {
            sqlConnection = new adoNet();
            DataTable prodotti = sqlConnection.eseguiQuery("SELECT * FROM Prodotti where Validita=1", CommandType.Text);
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
                for (int i = 0; i < prodotti.Rows.Count; i += 4)
                {
                    HtmlGenericControl divRow = new HtmlGenericControl("div");
                    divRow.Attributes.Add("class", "row");
                    container.Controls.Add(divRow);

                    for (int j = i; j < Math.Min(i + 4, prodotti.Rows.Count); j++)
                    {
                        DataRow prodotto = prodotti.Rows[j];

                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "col-lg-3 col-md-4 col-sm-6 mb-4"); // Modifica qui per card più piccole
                        divRow.Controls.Add(div);

                        HtmlGenericControl div2 = new HtmlGenericControl("div");
                        div2.Attributes.Add("class", "card h-100");
                        div.Controls.Add(div2);

                        HtmlGenericControl a = new HtmlGenericControl("a");
                        a.Attributes.Add("href", "#");
                        div2.Controls.Add(a);

                        string percorsoImmagine = prodotto["Immagine"].ToString();
                        HtmlGenericControl img = new HtmlGenericControl("img");
                        img.Attributes.Add("class", "card-img-top img-fluid");
                        img.Attributes.Add("src", percorsoImmagine);
                        img.Attributes.Add("alt", "");
                        a.Controls.Add(img);

                        HtmlGenericControl div3 = new HtmlGenericControl("div");
                        div3.Attributes.Add("class", "card-body");
                        div2.Controls.Add(div3);

                        HtmlGenericControl h4 = new HtmlGenericControl("h4");
                        h4.Attributes.Add("class", "card-title");
                        h4.InnerHtml = prodotto["NomeProdotto"].ToString();
                        div3.Controls.Add(h4);

                        HtmlGenericControl h5 = new HtmlGenericControl("h5");
                        h5.InnerHtml = prodotto["Prezzo"].ToString() + " €";
                        div3.Controls.Add(h5);

                        HtmlGenericControl p = new HtmlGenericControl("p");
                        p.Attributes.Add("class", "card-text");
                        p.InnerHtml = prodotto["Descrizione"].ToString();
                        div3.Controls.Add(p);

                        HtmlButton btn = new HtmlButton();
                        btn.ID = "btnCard_" + prodotto["Id"];
                        btn.Attributes.Add("class", "btn btn-primary");
                        btn.InnerText = "Aggiungi al Carrello";
                        btn.Attributes.Add("onclick", "aggiungiAlCarrello(" + prodotto["Id"].ToString() + ")");
                        div2.Controls.Add(btn);
                    }
                }

            }
        }
        public void aggiungiAlCarrello(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string idProdotto = clickedButton.Attributes["value"];
            DataRow account = Session["account"] as DataRow;
            if (account != null)
            {
                sqlConnection = new adoNet();
                sqlConnection.cmd.Parameters.AddWithValue("@idProdotto", idProdotto);
                sqlConnection.cmd.Parameters.AddWithValue("@idAccount", account["IdAccount"]);
                if (Convert.ToBoolean(
                        Convert.ToInt32(
                        sqlConnection.eseguiScalar(
                            "SELECT COUNT(*) FROM Carrello WHERE IdAccount = @idAccount AND IdProdotto = @idProdotto",
                            CommandType.Text
                            )
                        )
                    )
                )
                {
                    sqlConnection.cmd.Parameters.AddWithValue("@idProdotto", idProdotto);
                    sqlConnection.cmd.Parameters.AddWithValue("@idAccount", account["IdAccount"]);
                    sqlConnection.cmd.Parameters.AddWithValue("@data", DateTime.Now);
                    sqlConnection.eseguiNonQuery(
                        "UPDATE Carrello SET Quantita = Quantita + 1, Data = @data WHERE IdAccount = @idAccount AND IdProdotto = @idProdotto",
                        CommandType.Text
                        );
                }
                else
                {
                    sqlConnection.cmd.Parameters.AddWithValue("@idProdotto", idProdotto);
                    sqlConnection.cmd.Parameters.AddWithValue("@idAccount", account["IdAccount"]);
                    sqlConnection.cmd.Parameters.AddWithValue("@data", DateTime.Now);
                    sqlConnection.cmd.Parameters.AddWithValue("@quantita", 1);
                    sqlConnection.eseguiNonQuery(
                        "INSERT INTO Carrello (IdAccount, IdProdotto, Data, Quantita) VALUES (@idAccount, @idProdotto, @data, @quantita)",
                        CommandType.Text
                        );
                }
            }
            else
            {
                Response.Redirect("errorPage.aspx?codErr=1");
            }

        }
        protected void btnCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("Carrello.aspx");
        }
        protected void btnOrdini_Click(object sender, EventArgs e)
        {
            if (Session["account"] is DataRow account)
            {
                sqlConnection = new adoNet();
                sqlConnection.cmd.Parameters.AddWithValue("@idAccount", account["IdAccount"]);
                DataTable ordini = sqlConnection.eseguiQuery(@"SELECT Prodotto.NomeProdotto AS Prodotto, 
                                                                       Carrello.Quantita AS Quantita, 
                                                                       Carrello.DataAcquisto AS DataAcquisto, 
                                                                       Prodotto.Prezzo, 
                                                                       Prodotto.Immagine
                                                                FROM StoricoOrdini AS Carrello 
                                                                INNER JOIN Prodotti AS Prodotto ON Carrello.IdProdotto = Prodotto.Id
                                                                WHERE Carrello.IdAccount = @idAccount
                                                                ORDER BY Carrello.DataAcquisto DESC", CommandType.Text);
                if (ordini.Rows.Count > 0)
                {
                    container.Controls.Clear();
                    DateTime dataPrec = DateTime.MinValue;
                    decimal totale = 0;

                    // Aggiungi intestazione tabella
                    Table table = new Table();
                    table.CssClass = "table table-striped"; // Applica lo stile Bootstrap per le tabelle

                    // Aggiungi riga per l'intestazione
                    TableHeaderRow headerRow = new TableHeaderRow();

                    // Aggiungi le celle per i nomi dei campi
                    headerRow.Cells.Add(new TableHeaderCell() { Text = "Prodotto" });
                    headerRow.Cells.Add(new TableHeaderCell() { Text = "Quantità" });
                    headerRow.Cells.Add(new TableHeaderCell() { Text = "Prezzo Unitario" });
                    headerRow.Cells.Add(new TableHeaderCell() { Text = "Subtotale" });
                    headerRow.Cells.Add(new TableHeaderCell() { Text = "Immagine" });

                    // Aggiungi l'intestazione alla tabella
                    table.Rows.Add(headerRow);

                    // Aggiungi la tabella al contenitore
                    container.Controls.Add(table);

                    foreach (DataRow ordine in ordini.Rows)
                    {
                        DateTime dataAcquisto = (DateTime)ordine["DataAcquisto"];

                        // Se la data di acquisto è diversa dalla data precedente, aggiungi una nuova riga per la data
                        if (dataAcquisto.Date != dataPrec.Date)
                        {
                            // Calcola e mostra il totale per la data precedente
                            if (totale > 0)
                            {
                                TableRow totaleRow = new TableRow();
                                table.Rows.Add(totaleRow);

                                TableCell totaleCell = new TableCell();
                                totaleCell.ColumnSpan = 5; // Espandi la cella per tutta la larghezza della tabella
                                totaleCell.CssClass = "text-center font-weight-bold";
                                totaleCell.Text = $"Totale: {totale} €";
                                totaleRow.Cells.Add(totaleCell);
                            }

                            dataPrec = dataAcquisto.Date;

                            // Aggiungi una nuova riga per la data di acquisto
                            TableRow dateRow = new TableRow();
                            table.Rows.Add(dateRow);

                            TableCell dateCell = new TableCell();
                            dateCell.ColumnSpan = 5; // Espandi la cella per tutta la larghezza della tabella
                            dateCell.CssClass = "font-weight-bold";
                            dateCell.Text = $"Data di acquisto: {dataAcquisto:dd/MM/yyyy}";
                            dateRow.Cells.Add(dateCell);
                        }

                        // Aggiungi una nuova riga per l'ordine
                        TableRow orderRow = new TableRow();
                        table.Rows.Add(orderRow);

                        // Aggiungi le celle con i dettagli dell'ordine
                        TableCell productNameCell = new TableCell();
                        productNameCell.Text = ordine["Prodotto"].ToString();
                        orderRow.Cells.Add(productNameCell);

                        TableCell quantityCell = new TableCell();
                        quantityCell.Text = ordine["Quantita"].ToString();
                        orderRow.Cells.Add(quantityCell);

                        TableCell priceCell = new TableCell();
                        priceCell.Text = $"{(int)ordine["Quantita"] * (decimal)ordine["Prezzo"]} €";
                        orderRow.Cells.Add(priceCell);

                        // Calcola il totale per l'ordine corrente
                        decimal subtotal = (int)ordine["Quantita"] * (decimal)ordine["Prezzo"];
                        totale += subtotal;

                        TableCell subtotalCell = new TableCell();
                        subtotalCell.Text = $"{subtotal} €";
                        orderRow.Cells.Add(subtotalCell);

                        TableCell imageCell = new TableCell();
                        Image image = new Image();
                        image.CssClass = "img-thumbnail"; // Applica la classe Bootstrap per le immagini con bordi arrotondati
                        image.Width = 100; // Imposta la larghezza dell'immagine
                        image.Height = 100; // Imposta l'altezza dell'immagine
                        image.ImageUrl = ordine["Immagine"].ToString();
                        imageCell.Controls.Add(image);
                        orderRow.Cells.Add(imageCell);
                    }

                    // Aggiungi l'ultima riga con il totale finale
                    TableRow finalTotalRow = new TableRow();
                    table.Rows.Add(finalTotalRow);

                    if (totale > 0)
                    {
                        HtmlGenericControl totLbl = new HtmlGenericControl("h5");
                        totLbl.Style.Add("text-align", "center");
                        totLbl.Attributes.Add("class", "my-3 text-black");
                        totLbl.InnerText = $"Totale: {totale} €";
                        container.Controls.Add(totLbl);
                    }
                }
                else
                {
                    container.Controls.Clear();
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "alert text-black text-center");
                    div.InnerText = "Non hai ancora effettuato ordini";
                    container.Controls.Add(div);
                }
            }
        }
    }
}
