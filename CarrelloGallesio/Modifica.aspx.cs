using adoNetWebSQlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarrelloGallesio
{
    public partial class Modifica : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["account"] != null)
                {
                    int idFornitore = Convert.ToInt32(((DataRow)Session["account"])["IdAccount"]);
                    BindNome(idFornitore);
                    BindCategories();
                }
                else
                {
                    lblErrore.Text = "ID Fornitore non trovato. Effettua nuovamente il login.";
                }
            }
        }

        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            // Prima di procedere, verifica la validità dei campi
            if (!controllaCampi())
            {
                // Se i controlli falliscono, interrompi l'esecuzione del metodo
                return;
            }
            // Recupera l'ID del prodotto selezionato dalla DropDownList (se necessario per altre operazioni)
            int idProdotto = int.Parse(cmbNomeProdotto.SelectedValue);

            // Recupera il nome del prodotto selezionato dalla DropDownList
            string nomeProdotto = cmbNomeProdotto.SelectedItem.Text;
            try
            {
                adoNet dbConnection = new adoNet();
                dbConnection.cmd.Parameters.Clear(); // Pulizia dei parametri precedenti

                // Assicurati che l'ordine di aggiunta dei parametri corrisponda all'ordine nella query SQL
                dbConnection.cmd.Parameters.AddWithValue("@nome", nomeProdotto);
                dbConnection.cmd.Parameters.AddWithValue("@prezzo", Convert.ToDecimal(txtPrezzo.Text));
                dbConnection.cmd.Parameters.AddWithValue("@descrizione", txtDescrizione.Text);
                dbConnection.cmd.Parameters.AddWithValue("@categoria", int.Parse(cmbCategorie.SelectedValue));
                dbConnection.cmd.Parameters.AddWithValue("@fornitore", Convert.ToInt32(((DataRow)Session["account"])["IdAccount"])); // Assicurati che questo parametro sia corretto
                dbConnection.cmd.Parameters.AddWithValue("@immagine", "Img/" + imagePicker.FileName);
                dbConnection.cmd.Parameters.AddWithValue("@idProdotto", idProdotto); // Assicurati che questo sia l'ultimo parametro aggiunto se è l'ultimo nella tua query

                int rowsAffected = dbConnection.eseguiNonQuery(
                    @"UPDATE Prodotti 
              SET NomeProdotto = @nome, Prezzo = @prezzo, Descrizione = @descrizione, Categoria = @categoria, Fornitore = @fornitore, Immagine = @immagine 
              WHERE Id = @idProdotto",
                    CommandType.Text);

                if (rowsAffected > 0)
                {
                    // Success, redirect or notify user
                    Response.Redirect("Fornitore.aspx");
                }
                else
                {
                    // No rows affected, handle accordingly
                    lblErrore.Text = "Nessun prodotto aggiornato. Verifica l'ID del prodotto.";
                }
            }
            catch (Exception ex)
            {
                lblErrore.Text = "Si è verificato un errore durante l'aggiornamento del prodotto: " + ex.Message;
            }
        }

        private bool controllaCampi()
        {
            if (cmbNomeProdotto.SelectedValue != "0")
            {
                if (cmbCategorie.SelectedValue != "0")
                {
                    if (imagePicker.HasFile)
                    {
                        if (txtPrezzo.Text != string.Empty)
                        {
                            if (txtDescrizione.Text != string.Empty)
                            {
                                return true;
                            }
                            else
                                lblErrore.Text = "Inserisci una descrizione";
                        }
                        else
                            lblErrore.Text = "Inserisci un prezzo";
                    }
                    else
                        lblErrore.Text = "Seleziona un'immagine";
                }
                else
                    lblErrore.Text = "Seleziona una categoria";
            }
            else
                lblErrore.Text = "Seleziona un prodotto";

            return false;
        }

        private void BindNome(int idFornitore)
        {
            adoNet dbConnection = new adoNet();
            dbConnection.cmd.Parameters.Clear();
            dbConnection.cmd.Parameters.AddWithValue("@idFornitore", idFornitore);
            DataTable dtProdotti = dbConnection.eseguiQuery("SELECT Id, NomeProdotto FROM Prodotti WHERE Fornitore = @idFornitore", CommandType.Text);

            cmbNomeProdotto.DataSource = dtProdotti;
            cmbNomeProdotto.DataValueField = "Id"; // L'ID del prodotto
            cmbNomeProdotto.DataTextField = "NomeProdotto"; // Il nome del prodotto
            cmbNomeProdotto.DataBind();

            // Aggiungi un elemento predefinito alla DropDownList
            cmbNomeProdotto.Items.Insert(0, new ListItem("Seleziona un prodotto", "0"));
        }

        private DataTable GetProdottoById(int idProdotto)
        {
            adoNet adoWeb = new adoNet();
            string query = "SELECT * FROM Prodotti WHERE Id = @Id";
            adoWeb.cmd.Parameters.AddWithValue("@Id", idProdotto);

            return adoWeb.eseguiQuery(query, CommandType.Text);
        }

        private void BindCategories()
        {
            adoNet adoWeb = new adoNet();
            string query = "SELECT Id, Descrizione FROM Categorie WHERE Validita = 1";

            DataTable categorie = adoWeb.eseguiQuery(query, CommandType.Text);

            cmbCategorie.DataSource = categorie;
            cmbCategorie.DataTextField = "Descrizione";
            cmbCategorie.DataValueField = "Id";
            cmbCategorie.DataBind();

            // Aggiungi un elemento predefinito alla DropDownList
            cmbCategorie.Items.Insert(0, new ListItem("Seleziona una categoria", "0"));
        }

        protected void cmbNomeProdotto_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recupera l'ID del prodotto selezionato
            int idProdottoSelezionato = Convert.ToInt32(cmbNomeProdotto.SelectedValue);

            // Recupera le informazioni del prodotto dal database utilizzando l'ID selezionato
            DataTable prodottoSelezionato = GetProdottoById(idProdottoSelezionato);

            // Popola i campi di input con le informazioni del prodotto selezionato
            if (prodottoSelezionato.Rows.Count > 0)
            {
                txtPrezzo.Text = prodottoSelezionato.Rows[0]["Prezzo"].ToString();
                txtDescrizione.Text = prodottoSelezionato.Rows[0]["Descrizione"].ToString();

                // Imposta la categoria selezionata nella ComboBox
                string categoriaSelezionata = prodottoSelezionato.Rows[0]["Categoria"].ToString();
                cmbCategorie.SelectedValue = categoriaSelezionata;
            }
        }
        
    }
}