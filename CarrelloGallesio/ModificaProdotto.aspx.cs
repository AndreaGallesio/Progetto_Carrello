using adoNetWebSQlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarrelloGallesio
{
    public partial class ModificaProdotto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNome();

                BindCategories();

                BindFornitori();
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
                dbConnection.cmd.Parameters.AddWithValue("@prezzo", Convert.ToDecimal(((TextBox)this.FindControl("txtPrezzo")).Text));
                dbConnection.cmd.Parameters.AddWithValue("@descrizione", ((TextBox)this.FindControl("txtDescrizione")).Text);
                dbConnection.cmd.Parameters.AddWithValue("@categoria", int.Parse(((DropDownList)this.FindControl("cmbCategorie")).SelectedValue));
                dbConnection.cmd.Parameters.AddWithValue("@fornitore", int.Parse(((DropDownList)this.FindControl("cmbFornitore")).SelectedValue));
                dbConnection.cmd.Parameters.AddWithValue("@immagine", "Img/" + ((FileUpload)this.FindControl("imagePicker")).FileName);
                dbConnection.cmd.Parameters.AddWithValue("@idProdotto", idProdotto); // Assicurati che questo sia l'ultimo parametro aggiunto se è l'ultimo nella tua query

                int rowsAffected = dbConnection.eseguiNonQuery(
                    @"UPDATE Prodotti 
              SET NomeProdotto = @nome, Prezzo = @prezzo, Descrizione = @descrizione, Categoria = @categoria, Fornitore = @fornitore, Immagine = @immagine 
              WHERE Id = @idProdotto",
                    CommandType.Text);

                if (rowsAffected > 0)
                {
                    // Success, redirect or notify user
                    Response.Redirect("AdminPage.aspx");
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
            if (((DropDownList)this.FindControl("cmbNomeProdotto")).SelectedValue != "0")
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

        private void BindNome()
        {
            adoNet dbConnection = new adoNet();
            DataTable dtProdotti = dbConnection.eseguiQuery("SELECT Id, NomeProdotto FROM Prodotti", CommandType.Text);

            cmbNomeProdotto.DataSource = dtProdotti;
            cmbNomeProdotto.DataValueField = "Id"; // L'ID del prodotto
            cmbNomeProdotto.DataTextField = "NomeProdotto"; // Il nome del prodotto
            cmbNomeProdotto.DataBind();
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

        private void BindFornitori()
        {
            adoNet adoWeb = new adoNet();
            string query = "SELECT Id, NomeFornitore FROM Fornitori";

            DataTable fornitori = adoWeb.eseguiQuery(query, CommandType.Text);

            cmbFornitore.DataSource = fornitori;
            cmbFornitore.DataTextField = "NomeFornitore";
            cmbFornitore.DataValueField = "Id";
            cmbFornitore.DataBind();
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

                // Imposta il fornitore selezionato nella ComboBox
                string fornitoreSelezionato = prodottoSelezionato.Rows[0]["Fornitore"].ToString();
                cmbFornitore.SelectedValue = fornitoreSelezionato;
            }
        }
    }
}