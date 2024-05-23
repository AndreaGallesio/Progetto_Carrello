<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Carrello.aspx.cs" Inherits="CarrelloGallesio.Carrello" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <title></title>
    <style>
        .btn-custom {
            background-color: #4CAF50; /* Colore di sfondo verde */
            color: white; /* Colore del testo bianco */
            border: none; /* Nessun bordo */
            padding: 10px 20px; /* Spaziatura interna */
            text-align: center; /* Allineamento del testo al centro */
            text-decoration: none; /* Nessun sottolineatura */
            display: inline-block; /* Mostra come elemento inline */
            font-size: 16px; /* Dimensione del testo */
            margin: 4px 2px; /* Margine esterno */
            cursor: pointer; /* Cambia il cursore al passaggio del mouse */
            border-radius: 8px; /* Bordo arrotondato */
            transition-duration: 0.4s; /* Durata dell'effetto di transizione */
        }

        .btn-custom:hover {
            background-color: #45a049; /* Cambia il colore di sfondo al passaggio del mouse */
            color: white; /* Cambia il colore del testo al passaggio del mouse */
        }
        .center-button {
            display: flex;
            justify-content: center;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container-fluid">
                <asp:PlaceHolder ID="container" runat="server">
    
                </asp:PlaceHolder>
            </div>
            <div class="center-button">
                <asp:Button ID="btnCard" runat="server" Text="Effettua Pagamento" OnClick="btnCart_Click" CssClass="btn btn-success" />
            </div>
        </div>
    </form>
</body>
</html>
