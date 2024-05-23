using adoNetWebSQlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using adoNetWebSQlServer;

namespace CarrelloGallesio
{
    public partial class Carrello : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            DataRow account = Session["account"] as DataRow;
            if (account != null)
            {
                sqlConnection = new adoNet();
                sqlConnection.cmd.Parameters.AddWithValue("@IdAccount", account["IdAccount"]);
                DataTable carrello = sqlConnection.eseguiQuery(
                    "SELECT * FROM Carrello WHERE IdAccount = @IdAccount",
                    CommandType.Text
                    );
                if (carrello.Rows.Count > 0)
                {
                    // Creazione dell'etichetta per il titolo
                    HtmlGenericControl h2Title = new HtmlGenericControl("h2");
                    h2Title.Attributes.Add("class", "text-center text-black");
                    h2Title.InnerText = "Carrello di " + account["NomeAccount"].ToString() + " " + account["CognomeAccount"].ToString(); // Assicurati di sostituire "NomeUtente" con il nome effettivo della colonna
                    container.Controls.Add(h2Title);

                    // Creazione della tabella per mostrare i prodotti nel carrello
                    HtmlGenericControl table = new HtmlGenericControl("table");
                    table.Attributes.Add("class", "table table-hover text-white");
                    container.Controls.Add(table);

                    // Aggiunta delle intestazioni della tabella
                    HtmlGenericControl thead = new HtmlGenericControl("thead");
                    thead.Attributes.Add("class", "text-white");
                    table.Controls.Add(thead);

                    HtmlGenericControl tr = new HtmlGenericControl("tr");
                    thead.Controls.Add(tr);

                    HtmlGenericControl th = new HtmlGenericControl("th");
                    th.InnerText = "Prodotto";
                    tr.Controls.Add(th);

                    th = new HtmlGenericControl("th");
                    th.InnerText = "Descrizione";
                    tr.Controls.Add(th);

                    th = new HtmlGenericControl("th");
                    th.InnerText = "Prezzo";
                    tr.Controls.Add(th);

                    th = new HtmlGenericControl("th");
                    th.InnerText = "Quantità";
                    tr.Controls.Add(th);

                    th = new HtmlGenericControl("th");
                    th.InnerText = "Data";
                    tr.Controls.Add(th);

                    // Aggiunta dei prodotti nel carrello alla tabella
                    HtmlGenericControl tbody = new HtmlGenericControl("tbody");
                    tbody.Attributes.Add("class", "text-white");
                    table.Controls.Add(tbody);
                    foreach (DataRow carRow in carrello.Rows)
                    {
                        tr = new HtmlGenericControl("tr");
                        tbody.Controls.Add(tr);
                        sqlConnection.cmd.Parameters.AddWithValue("@IdProdotto", carRow["IdProdotto"]);
                        DataRow prodotto = sqlConnection.eseguiQuery(
                            "SELECT * FROM Prodotti WHERE Id = @IdProdotto",
                            CommandType.Text
                            ).Rows[0];
                        HtmlGenericControl td = new HtmlGenericControl("td")
                        {
                            InnerText = prodotto["NomeProdotto"].ToString()
                        };
                        tr.Controls.Add(td);
                        td = new HtmlGenericControl("td")
                        {
                            InnerText = prodotto["Descrizione"].ToString()
                        };
                        tr.Controls.Add(td);
                        td = new HtmlGenericControl("td")
                        {
                            InnerText = (decimal)prodotto["Prezzo"] * (int)carRow["Quantita"] + " €"
                        };
                        tr.Controls.Add(td);
                        td = new HtmlGenericControl("td")
                        {
                            InnerText = carRow["Quantita"].ToString()
                        };
                        tr.Controls.Add(td);
                        td = new HtmlGenericControl("td")
                        {
                            InnerText = ((DateTime)carRow["Data"]).Date.ToString("dd/MM/yyyy")
                        };
                        tr.Controls.Add(td);
                        td = new HtmlGenericControl("td");

                    }
                    HtmlGenericControl btnWrapper = new HtmlGenericControl("div");
                    btnWrapper.Attributes.Add("class", "d-flex justify-content-center");
                    container.Controls.Add(btnWrapper);
                }
                else
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "alert alert-info text-center");
                    div.InnerText = "Il carrello è vuoto";
                    container.Controls.Add(div);
                }
            }
        }

        protected void btnCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pagamento.aspx");
        }
    }
}