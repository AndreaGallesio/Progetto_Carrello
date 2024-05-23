<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grafici.aspx.cs" Inherits="CarrelloGallesio.Grafici" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Grafico che rappresenta la quantità dei prodotti venduti
            <asp:Chart ID="Chart1" runat="server" DataSourceID="DBCarrello">
                <series>
                    <asp:Series Name="Series1" XValueMember="NomeUtente" YValueMembers="NumeroOrdini">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>
            <asp:SqlDataSource ID="DBCarrello" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT a.NomeAccount + ' ' + a.CognomeAccount AS NomeUtente, COUNT(s.Id) AS NumeroOrdini FROM Account AS a LEFT OUTER JOIN StoricoOrdini AS s ON a.IdAccount = s.IdAccount GROUP BY a.NomeAccount, a.CognomeAccount"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
