<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="CarrelloGallesio.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <title></title>
        <link href="Content/bootstrap.css" rel="stylesheet" />
        <style>
    /* Importing fonts from Google */
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800;900&display=swap');

    /* Reseting */
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
        font-family: 'Poppins', sans-serif;
    }

    body {
        background: #ecf0f3;
    }

    .wrapper {
        max-width: 100%; /* Modifica la larghezza massima del form */
        min-height: 100%;
        margin: 80px auto;
        padding: 40px 30px 30px 30px;
        background-color: #ecf0f3;
        border-radius: 15px;
        box-shadow: 13px 13px 20px #cbced1, -13px -13px 20px #fff;
    }

    .logo {
        width: 80px;
        margin: auto;
    }

    .logo img {
        width: 100%;
        height: 80px;
        object-fit: cover;
        border-radius: 50%;
        box-shadow: 0px 0px 3px #5f5f5f,
            0px 0px 0px 5px #ecf0f3,
            8px 8px 15px #a7aaa7,
            -8px -8px 15px #fff;
    }

    .wrapper .name {
        font-weight: 600;
        font-size: 1.4rem;
        letter-spacing: 1.3px;
        padding-left: 10px;
        color: #555;
    }

    .wrapper .form-field input {
        width: 100%;
        display: block;
        border: none;
        outline: none;
        background: none;
        font-size: 1.2rem;
        color: #666;
        padding: 10px 15px 10px 10px;
        /* border: 1px solid red; */
    }

    .wrapper .form-field {
        padding-left: 10px;
        margin-bottom: 20px;
        border-radius: 20px;
        box-shadow: inset 8px 8px 8px #cbced1, inset -8px -8px 8px #fff;
    }

    .wrapper .form-field .fas {
        color: #555;
    }

    .wrapper .btn {
        box-shadow: none;
        width: 100%;
        height: 40px;
        background-color: #03A9F4;
        color: #fff;
        border-radius: 25px;
        box-shadow: 3px 3px 3px #b1b1b1,
            -3px -3px 3px #fff;
        letter-spacing: 1.3px;
    }

    .wrapper .btn:hover {
        background-color: #039BE5;
    }

    .wrapper a {
        text-decoration: none;
        font-size: 0.8rem;
        color: #03A9F4;
    }

    .wrapper a:hover {
        color: #039BE5;
    }

    @media(max-width: 380px) {
        .wrapper {
            margin: 30px 20px;
            padding: 40px 15px 15px 15px;
        }
    }

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
    .btn-custom {
    font-size: 16px;
    padding: 10px 20px;
    border-radius: 8px;
    margin-bottom: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    transition: background-color 0.3s ease, box-shadow 0.3s ease;
}

.btn-custom:hover {
    box-shadow: 0 6px 8px rgba(0, 0, 0, 0.2);
}

.nav-link {
    display: block;
    padding: 0;
}

.nav-item {
    margin-bottom: 10px;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
    <nav class="navbar navbar-expand-lg bg-light">
        <div class="container-fluid">
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item d-flex justify-content-center">
                        <asp:Label ID="lblNomeUser" runat="server" Text="Label" CssClass="user-label"></asp:Label>
                    </li>
                    <li  class="nav-item d-flex justify-content-center">
                        <asp:DropDownList ID="ddlCategorie" runat="server" CssClass="form-select" AutoPostBack="True" OnSelectedIndexChanged="ddlCategorie_SelectedIndexChanged1">
                         <asp:ListItem Text="Seleziona una categoria" Value="" Disabled="true" Selected="true"></asp:ListItem>
                         </asp:DropDownList>
                    </li>
                                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            <asp:Button ID="btnAggiungi" runat="server" OnClick="btnAggiungi_Click" Text="Aggiungi Prodotto" CssClass="btn btn-primary btn-custom" />
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            <asp:Button ID="Button1" runat="server" OnClick="btnAggiungiCategoria_Click" Text="Aggiungi Categoria" CssClass="btn btn-success btn-custom" />
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            <asp:Button ID="Button2" runat="server" Text="Elimina Prodotto" CssClass="btn btn-danger btn-custom" OnClick="Button2_Click" />
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            <asp:Button ID="Button3" runat="server" Text="Modifica Prodotto" CssClass="btn btn-warning btn-custom" OnClick="Button3_Click" />
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            <asp:Button ID="Button4" runat="server" Text="Visualizza Grafico" CssClass="btn btn-info btn-custom" OnClick="Button4_Click" />
                        </a>
                    </li>  
                </ul>
                <asp:LoginView runat="server">                        
                </asp:LoginView>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="Button1_Click" CssClass="btn btn-danger btn-lg" />

                
            </div>
        </div>
    </nav>
    <div class="container-fluid">
        <asp:PlaceHolder ID="container" runat="server"></asp:PlaceHolder>
    </div>
    <asp:Label ID="lblErrore" runat="server"></asp:Label>
</form>
</body>
</html>
