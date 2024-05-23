<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="CarrelloGallesio.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Navbar</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
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

        .btn-danger {
            background-color: #D9534F; /* Colore di sfondo rosso */
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

        .btn-danger:hover {
            background-color: #C9302C; /* Cambia il colore di sfondo al passaggio del mouse */
            color: white; /* Cambia il colore del testo al passaggio del mouse */
        }

        .user-label {
            font-size: 18px; /* Dimensione del testo */
            font-weight: bold; /* Grassetto */
            color: #333; /* Colore del testo */
            margin-right: 20px; /* Margine destro per separare dal pulsante Aggiungi */
            margin-top: 25px
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg bg-light">
            <div class="container-fluid">
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                        <li class="nav-item">
                            <asp:Label ID="lblNomeUser" runat="server" Text="Label" CssClass="user-label"></asp:Label>                           
                        </li>
                        <li class="nav-item">
                            <asp:DropDownList ID="ddlCategorie" runat="server" CssClass="form-select" AutoPostBack="True" OnSelectedIndexChanged="ddlCategorie_SelectedIndexChanged">
                             <asp:ListItem Text="Seleziona una categoria" Value="" Disabled="true" Selected="true"></asp:ListItem>
                             </asp:DropDownList>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#"></a>
                        </li>
                        <li class="nav-item">
                            <asp:Button ID="btnOrdini" runat="server" OnClick="btnOrdini_Click" Text="I miei Ordini" CssClass="btn btn-warning" />

                            <asp:Button ID="btnCard" runat="server" Text="Carrello" OnClick="btnCart_Click" CssClass="btn btn-success" />
                        </li>
                    </ul>
                    <asp:LoginView runat="server">                        
                    </asp:LoginView>
                    <asp:Button class="btn btn-danger" ID="btnLogout" runat="server" Text="Logout" OnClick="Button1_Click" />
                </div>                
            </div>
        </nav>
    </form>
    <div class="container-fluid">
        <asp:PlaceHolder id="container" runat="server">
        
        </asp:PlaceHolder>
    </div>
</body>
</html>
