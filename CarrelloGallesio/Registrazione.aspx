<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrazione.aspx.cs" Inherits="CarrelloGallesio.Registrazione" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registrazione</title>
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
        max-width: 350px;
        min-height: 500px;
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
</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" CssClass="wrapper">
            <div class="logo">
                <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ0J_xYRkncr3n2xKAtFodbUfCC1epVy86sJS1YIBtMRA&s" alt="" />
            </div>
            <div class="text-center mt-4 name">
                Registrazione
            </div>
            <asp:Panel runat="server" CssClass="p-3 mt-3">
                <div class="form-field d-flex align-items-center">
                    <span class="far fa-user"></span>
                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" placeholder="Nome"></asp:TextBox>
                </div>
                <div class="form-field d-flex align-items-center">
                    <span class="far fa-user"></span>
                    <asp:TextBox ID="txtCognome" runat="server" CssClass="form-control" placeholder="Cognome"></asp:TextBox>
                </div>
                <div class="form-field d-flex align-items-center">
                    <span class="far fa-envelope"></span>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                </div>
                <div class="form-field d-flex align-items-center">
                    <span class="far fa-user"></span>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
                </div>
                <div class="form-field d-flex align-items-center">
                    <span class="fas fa-key"></span>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                </div>
                <div class="form-field d-flex align-items-center">
                    <span class="far fa-user"></span>
                    <asp:DropDownList ID="ddlTipoAccount" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">User</asp:ListItem>
                        <asp:ListItem Value="2">Fornitore</asp:ListItem>
                        <asp:ListItem Value="3">Admin</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-field d-flex align-items-center">
                    <span class="far fa-user"></span>
                    <asp:DropDownList ID="ddlValiditaAccount" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Attivo</asp:ListItem>
                        <asp:ListItem Value="0">Non Attivo</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Button ID="btnRegistra" runat="server" Text="Registrati" CssClass="btn mt-3" OnClick="btnRegistra_Click" />
                <br />
                <asp:Label ID="lblMessaggio" runat="server"></asp:Label>
            </asp:Panel>
            <div class="text-center fs-6">
                Hai già un Account? <a href="Default.aspx">Accedi</a>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
