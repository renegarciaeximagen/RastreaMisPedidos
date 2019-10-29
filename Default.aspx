<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administrador E-shop</title>
    <link href="http://www.promoline.com.mx/css/estilos.css" rel="stylesheet" type="text/css" />
    <link href="Estilos/Blue.css" rel="stylesheet" type="text/css" />
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: Gray">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <center>
        <table width="814" border="0" cellpadding="0" cellspacing="0" style="background-color: White">
            <tr>
                <td colspan="3">
                    <img src="Img/header.jpg" />
                </td>
            </tr>
            <tr>
                <td class="textnumtits" colspan="3" style="height: 15px">
                    <br />
                </td>
            </tr>
            <tr>
                <td class="textnumtits" colspan="3" style="height: 15px">
                    Rastreo de Pedidos&nbsp; Ultimos 15 Dias</td>
            </tr>
            <tr>
                <td colspan="3" style="height: 17px" class="textnumtits">
                    <asp:Label ID="lblNombreAdmin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 10px" align="center">
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="height: 44px">
                             <asp:Label ID="lblSeccion" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="grid_Rastreo" runat="server" AutoGenerateColumns="False" Caption="Consulta Facturas Ultimos 15 Dias"
                                    CaptionAlign="Top" CssClass="grid" ToolTip="Consulta Rastreo" 
                                    Width="800px" onrowcommand="grid_Rastreo_RowCommand" >
                                    <RowStyle Font-Names="Verdana" ForeColor="#000066" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Folio Factura">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FolioElec" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Facturas_DocNum") %>'
                                                    Visible="True" Width="100%">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Importe">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Total" runat="server" Text='<%# Eval("Total","{0:$#,##0.00}") %>'
                                                    Visible="True" Width="100%">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Fecha">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Facturas_DocDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Facturas_DocDate") %>'
                                                    Visible="True" Width="100%">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NumCliente ">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Facturas_CardCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Facturas_CardCode") %>'
                                                    Visible="True" Width="100%">                                                                          
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Implica Produccion">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Facturas_ImplicaProduccion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Facturas_ImplicaProduccion") %>'
                                                    Visible="True" Width="100%">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ultimo Estatus">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Estatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Cat_Status_Descrip") %>'
                                                    Visible="True" Width="100%">
                                                </asp:Label>
                                                   <asp:Label ID="lbl_FacturasId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FacturasId") %>'
                                                    Visible="false" Width="100%">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                         <asp:ButtonField ButtonType="Image" CommandName="Consulta" HeaderText="Consultar"
                                                                    ImageUrl="~/Img/whatsnext.png"   >                                  
                                             
                                             
                                        </asp:ButtonField>
                                    </Columns>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Names="Tahoma" Font-Overline="False"
                                        Font-Size="12px" Font-Strikeout="False" Font-Underline="False" ForeColor="#000066"
                                        HorizontalAlign="Center" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" >
                    &nbsp;
                </td>
            </tr>
        </table>
    </center>
    <asp:Panel ID="pnlProgress" runat="server" CssClass="updateProgress" Height="50px"
        Width="150px">
        <asp:UpdateProgress ID="uptpnlProgress" runat="server" DisplayAfter="0">
            <ProgressTemplate>
                <img src="Img/ajax-loader.gif" /><strong class="Textos2">
                    <h4>
                        Procesando...
                    </h4>
                </strong>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlProgress" TargetControlID="pnlProgress">
    </cc1:ModalPopupExtender>

    <script language="javascript" type="text/javascript">
     var ModalProgress = '<%= ModalProgress.ClientID %>';    
       
        
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);    
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);  

    function beginReq(sender, args){
            $find(ModalProgress).show();
        }

        function endReq(sender, args){
            $find(ModalProgress).hide();
        }           
              
        
    </script>

    </form>
</body>
</html>
