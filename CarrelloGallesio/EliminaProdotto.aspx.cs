using adoNetWebSQlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarrelloGallesio
{
    public partial class EliminaProdotto : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CaricaNomi();
            }
        }
        private void CaricaNomi()
        {
            adoNet adoWeb = new adoNet();
            string query = "SELECT P.Id, P.NomeProdotto, P.Validita " +
                           "FROM Prodotti AS P ";

            DataTable prodotti = adoWeb.eseguiQuery(query, CommandType.Text);

            cmbCategorie.DataSource = prodotti;
            cmbCategorie.DataTextField = "NomeProdotto";
            cmbCategorie.DataValueField = "Id";
            cmbCategorie.DataBind();

            cmbCategorie.Items.Insert(0, new ListItem("Seleziona un Prodotto", "0"));
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            int prodottoId = int.Parse(cmbCategorie.SelectedValue);
            bool isChecked = CheckBoxProdotto.Checked;

            sqlConnection = new adoNet();
            string query = "UPDATE Prodotti SET Validita = @Validita WHERE Id = @Id";
            sqlConnection.cmd.Parameters.Clear(); // Puliamo i parametri precedenti

            // Imposta lo stato di Validita in base al valore della CheckBox
            sqlConnection.cmd.Parameters.AddWithValue("@Validita", isChecked ? 0 : 1);

            sqlConnection.cmd.Parameters.AddWithValue("@Id", prodottoId);

            sqlConnection.eseguiNonQuery(query, CommandType.Text);

            lblErrore.Text = "Stato di Validita del prodotto aggiornato con successo.";

            Response.Redirect("AdminPage.aspx");
        }

            
        

        protected void cmbCategorie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategorie.SelectedValue != "0")
            {
                int prodottoId = int.Parse(cmbCategorie.SelectedValue);
                DataTable prodotto = GetProdotto(prodottoId);

                if (prodotto.Rows.Count > 0)
                {
                    bool isValid = (bool)prodotto.Rows[0]["Validita"];
                    CheckBoxProdotto.Enabled = true;
                    CheckBoxProdotto.Checked = !isValid; // Se Validita è false, CheckBox è checked
                }
            }
            else
            {
                CheckBoxProdotto.Enabled = false;
                CheckBoxProdotto.Checked = false;
            }
        }

        private DataTable GetProdotto(int prodottoId)
        {
            sqlConnection = new adoNet();
            string query = "SELECT Validita FROM Prodotti WHERE Id = @Id";
            sqlConnection.cmd.Parameters.Clear(); // Puliamo i parametri precedenti
            sqlConnection.cmd.Parameters.AddWithValue("@Id", prodottoId);

            return sqlConnection.eseguiQuery(query, CommandType.Text);
        }
    }
}