<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Resumen.aspx.cs" Inherits="Resumen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mesa Contro KN</title>
    <style type="text/css">
        .TblEstilo
        {
            width: 900px;
        }
        .cabecera
        {
            background-image: url(  'Estilos/captionbckg.gif' );
            background-repeat: repeat-x;
            color: #15428B;
            text-align: center;
            font: normal 12px arial, tahoma, helvetica, sans-serif;
            font-weight: bold;
        }
        .letraGrid
        {
            color: #15428B;
            font-size: 11px;
            font-style: normal;
            font-weight: normal;
        }
        .TableHeder
        {
            width: 800px;
        }
        .TableHederTD
        {
            width: 200px;
        }
        .cabeceraTd
        {
            background-image: url(  'Estilos/captionbckg.gif' );
            background-repeat: repeat-x;
            color: #15428B;
            font: normal 12px arial, tahoma, helvetica, sans-serif;
            font-weight: bold;
            width: 200px;
        }
    </style>
    <link href="Estilos/Blue.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table align="center" class="TblEstilo">
        <tr>
            <td align="left">
                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                    ImageUrl="~/Img/atras.png" ToolTip="Regresar" />
            </td>
        </tr>
        <tr>
            <td>
                <table class="TableHeder">
                    <tr>
                        <td class="cabeceraTd" align="right">
                            Factura SAP :
                        </td>
                        <td class="letraGrid" align="left">
                            <asp:Label ID="lbl_DocNum" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="cabeceraTd" align="right">
                            Folio Hoja de Viaje :
                        </td>
                        <td class="letraGrid" align="left">
                            <asp:Label ID="lbl_IdHojaViaje" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="cabeceraTd" align="right">
                            Num Cliente :
                        </td>
                        <td align="left" class="letraGrid">
                            <asp:Label ID="lbl_NumCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="cabeceraTd">
                            Nombre Cliente :                         </td>
                        <td align="left" class="letraGrid">
                            <asp:Label ID="lbl_NombreCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="cabeceraTd">
                            Vendedor :
                        </td>
                        <td align="left" class="letraGrid">
                            <asp:Label ID="lbl_Vendedor" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="cabeceraTd">
                            Perfil Cliente :</td>
                        <td align="left" class="letraGrid">
                            <asp:Label ID="lbl_PerfilCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="cabeceraTd">
                            Almacen&nbsp; Facturacion :</td>
                        <td align="left" class="letraGrid">
                            <asp:Label ID="lblAlmacenfac" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="cabeceraTd">
                            Almacen Embarque :</td>
                        <td align="left" class="letraGrid">
                            <asp:Label ID="lblAlmacenEmb" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_DocEntry" runat="server" Visible="False"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grvTransporte" runat="server" AutoGenerateColumns="False" Width="600px"
                    CaptionAlign="Top" ToolTip="Transporte" Caption="Transporte" CssClass="grid">
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <Columns>
                        <asp:TemplateField HeaderText="Transporte" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_NumTransporte" runat="server" Text='<%# Bind("NumTransID") %>'></asp:Label></td>
                            </ItemTemplate>
                            <ControlStyle Width="100px"></ControlStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Placas" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Placas" runat="server" Text='<%# Bind("Placas") %>'></asp:Label></td>
                            </ItemTemplate>
                            <ControlStyle Width="100px"></ControlStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sello KN" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SelloKN" runat="server" Text='<%# Bind("Sello") %>'></asp:Label></td>
                            </ItemTemplate>
                            <ControlStyle Width="100px"></ControlStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad Bultos" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CantBultos" runat="server" Text='<%# Bind("TotalBultos") %>'></asp:Label></td>
                            </ItemTemplate>
                            <ControlStyle Width="100px"></ControlStyle>
                        </asp:TemplateField>
                              <asp:TemplateField HeaderText="Fecha" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Fecha" runat="server" Text='<%# Bind("FechaCreacion") %>'></asp:Label></td>
                            </ItemTemplate>
                            <ControlStyle Width="100px"></ControlStyle>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="letraGrid" />
                    <HeaderStyle CssClass="cabecera" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GridDir" runat="server" AutoGenerateColumns="False" Width="600px"
                    CaptionAlign="Top" ToolTip="Dir. Entrega" Caption="Dir. Entrega" CssClass="grid">
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo de Entrega" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ShipToCode" runat="server" Text='<%# Bind("TipoDeEntrega") %>'></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dir." ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Address2" runat="server" Text='<%# Bind("Facturas_DirEmbar") %>'></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paquetera" ControlStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_TrnspName" runat="server" Text='<%# Bind("Paquetera") %>'></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="letraGrid" />
                    <HeaderStyle CssClass="cabecera" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GridMesaControl1_Hijo" runat="server" AutoGenerateColumns="False"
                    Width="800px" CaptionAlign="Top" ToolTip="Detalle Partidas" Caption="Detalle Partidas Factura"
                    CssClass="grid" OnRowDataBound="GridMesaControl1_Hijo_RowDataBound">
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <Columns>
                        <asp:TemplateField HeaderText="Num Partidas">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacturasPartidas_Id" runat="server" Text='<%# Bind("Partida") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Clave Produc">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacturasPartidas_Clave" runat="server" Text='<%# Bind("Clave") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacturasPartidas_DescripCion" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Produccion">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacturasPartidas_Produccion" runat="server" Text='<%# Bind("PartProd") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OD">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkn_descargar" runat="server" ToolTip="Descargar OD" OnCommand="lkn_descargar_Command"
                                    CssClass="link" CausesValidation="False">
                                    <asp:Label ID="lbl_FacturasPartidas_OrdenDiseno" runat="server" Text='<%# Bind("OD") %>'
                                        ForeColor="#33cc33" Font-Bold="True" ToolTip="Ver Orden de Diseño"></asp:Label>
                                </asp:LinkButton>
                                <asp:Label ID="lbl_OrdenDisenoId" runat="server" Text='<%# Bind("OD") %>' style="display:none"></asp:Label>
                                <asp:Label ID="lbl_NombreLogoOd" runat="server" Text='<%# Bind("NombreLogoOd") %>' style="display:none"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad Original">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacturasPartidas_Cantidad" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NC">
                            <ItemTemplate>
                                <asp:Label ID="lbl_NC" runat="server" Text='<%# Bind("NC") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Neto">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Neto" runat="server" Text='<%# Bind("Neto") %>'   ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bufer">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacturasPartidas_Bufer" runat="server" Text='<%# Bind("Buffer") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vales">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Vales" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Suma">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Suma" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bultos">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Bultos" runat="server" Text='<%# Bind("Bultos") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        </Columns>
                    <RowStyle CssClass="letraGrid" />
                    <HeaderStyle CssClass="cabecera" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gridDocumentos" runat="server" AutoGenerateColumns="False" Width="400px"
                    CaptionAlign="Top" ToolTip="Documentos Relacionados" Caption="Documentos Relacionados"
                    CssClass="grid" OnRowDataBound="gridDocumentos_RowDataBound">
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbl_FacTurasId" runat="server" Text='<%# Bind("FacTurasId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CatDcumentosId" runat="server" Text='<%# Bind("CatDcumentosId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Folio Documento">
                            <ItemTemplate>
                                <asp:HyperLink ID="HypFolio" runat="server" Target="_blank">
                                    <asp:Label ID="lbl_DocumentosRelacioFolio" runat="server" Text='<%# Bind("DocumentosRelacioFolio") %>'></asp:Label>
                                </asp:HyperLink>
                                <asp:Label ID="lbl_DocStatus" runat="server" Text='<%# Bind("DocStatus") %>' style="display:none"></asp:Label>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CatDcumentosDescripcion" runat="server" Text='<%# Bind("CatDcumentosDescripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pzas">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Pzas" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="letraGrid" />
                    <HeaderStyle CssClass="cabecera" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grid_TimeEstatus" runat="server" AutoGenerateColumns="False"
                    Caption="Time Stamp" Width="500px" CaptionAlign="Top" ToolTip="Detalle Partidas"
                    CssClass="grid" OnRowDataBound="grid_TimeEstatus_RowDataBound">
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <Columns>
                        <asp:TemplateField HeaderText="Time Stamp">
                            <ItemTemplate>
                                <asp:Label ID="lbl_LogStatusFacTimeStamp" runat="server" Text='<%# Bind("LogStatusFacTimeStamp") %>'></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tiempo Transcurrido">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Time_Tras" runat="server" Text=""></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Cat_Status_Descrip" runat="server" Text='<%# Bind("Cat_Status_Descrip") %>'></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="Usuario">
                            <ItemTemplate>
                                <asp:Label ID="lbl_UsuarioDescrip" runat="server" Text='<%# Bind("UsuarioDescrip") %>'></asp:Label></td>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="letraGrid" />
                    <HeaderStyle CssClass="cabecera" />
                </asp:GridView>
                <asp:Label ID="lbl_TimeInicio" runat="server" Text="" Style="display: none"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
