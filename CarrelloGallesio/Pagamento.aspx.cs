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
using System.Data.Common;
using System.Net.Mail;
using System.Net;

namespace CarrelloGallesio
{
    public partial class Pagamento : System.Web.UI.Page
    {
        private adoNet sqlConnection;
        private int categoria = 0;

        protected void btnPay_Click(object sender, EventArgs e)
        {

            if (Session["account"] is DataRow account)
            {
                sqlConnection = new adoNet();
                try
                {
                    InviaEmailRiepilogoOrdine(account);
                    sqlConnection.cmd.Parameters.AddWithValue("@idAccount", account["IdAccount"]);
                    sqlConnection.eseguiNonQuery(@"insert into StoricoOrdini(IdAccount, IdProdotto, Quantita, DataAcquisto) 
                                                  select IdAccount as Account, IdProdotto as Prodotto, Quantita, Data as DataAcquisto
                                                  from Carrello 
                                                  where IdAccount = @idAccount",
                                                  CommandType.Text);
                    sqlConnection.cmd.Parameters.AddWithValue("@idAccount", account["IdAccount"]);
                    sqlConnection.eseguiNonQuery(@"delete from Carrello where IdAccount = @idAccount", CommandType.Text);
                    Session["page"] = 1;

                    
                }
                catch
                {
                    Response.Redirect("errorPage.aspx?codErr=2");
                }
                Response.Redirect("UserPage.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void InviaEmailRiepilogoOrdine(DataRow account)
        {
            string emailDestinatario = account["MailAccount"].ToString();
            string corpoEmail = $"Gentile Cliente,\n\nGrazie per il tuo ordine. Ecco il riepilogo:\n\n";

            // Recupera tutti i prodotti acquistati dall'account
            DataTable prodottiAcquistati = GetProdottiAcquistati(account["IdAccount"].ToString());

            // Aggiungi ogni prodotto al corpo dell'email
            foreach (DataRow prodotto in prodottiAcquistati.Rows)
            {
                string nomeProdotto = prodotto["NomeProdotto"].ToString();
                int quantitaAcquistata = Convert.ToInt32(prodotto["Quantita"]);
                decimal prezzoUnitario = Convert.ToDecimal(prodotto["Prezzo"]);
                decimal prezzoTotale = quantitaAcquistata * prezzoUnitario;

                corpoEmail += $"Prodotto: {nomeProdotto}\nQuantità: {quantitaAcquistata}\nPrezzo unitario: {prezzoUnitario:C}\nPrezzo totale: {prezzoTotale:C}\n\n";
            }

            corpoEmail += $"Grazie per averci scelto!\n\nCordiali saluti,\nIl tuo negozio online";

            // Invia l'email
            using (MailMessage mail = new MailMessage("a.gallesio.2247@vallauri.edu", emailDestinatario))
            {
                mail.Subject = "Riepilogo Ordine";
                mail.Body = corpoEmail;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.UseDefaultCredentials = false;
                    // inserire email e password
                    client.Credentials = new NetworkCredential("a.gallesio.2247@vallauri.edu", "PASSOWORD");
                    client.EnableSsl = true;
                    client.Send(mail);
                }
            }


        }
        private DataTable GetProdottiAcquistati(string idAccount)
        {
            adoNet dbConnection = new adoNet();
            dbConnection.cmd.Parameters.AddWithValue("@idAccount", idAccount);

            string query = @"SELECT P.NomeProdotto, C.Quantita, P.Prezzo 
                     FROM Carrello AS C 
                     INNER JOIN Prodotti AS P ON C.IdProdotto = P.Id 
                     WHERE C.IdAccount = @idAccount";

            return dbConnection.eseguiQuery(query, CommandType.Text);
        }
    }
}