using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Componente.Procesos;
using Componente.Conexion;
using System.Data.SqlTypes;
using System.IO;
using Componente.Catalogos;
using System.Data.Linq.SqlClient;
using System.Data.Linq;
using System.Reflection;

public partial class Resumen : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 }, Request["data"]);

          
            lbl_IdHojaViaje.Text = QueryString["id"].ToString();
            CargaDatos();
            ImageButton1.PostBackUrl = "Default.aspx?hash=" + Session["hash"].ToString();
        }

    }

    #region Eventos Pagina
    protected void GridMesaControl1_Hijo_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                

                Label lbl_FacturasPartidas_Produccion = (Label)e.Row.FindControl("lbl_FacturasPartidas_Produccion");
                Label lbl_FacturasPartidas_Cantidad = (Label)e.Row.FindControl("lbl_FacturasPartidas_Cantidad");
                Label lbl_NC = (Label)e.Row.FindControl("lbl_NC");
                Label lbl_Neto = (Label)e.Row.FindControl("lbl_Neto");
                Label lbl_FacturasPartidas_Clave = (Label)e.Row.FindControl("lbl_FacturasPartidas_Clave");
                Label lbl_FacturasPartidas_Bufer = (Label)e.Row.FindControl("lbl_FacturasPartidas_Bufer");
                Label lbl_Vales = (Label)e.Row.FindControl("lbl_Vales");
                Label lbl_Suma = (Label)e.Row.FindControl("lbl_Suma");
                LinkButton lkn_descargar = (LinkButton)e.Row.FindControl("lkn_descargar");
                Label lbl_FacturasPartidas_OrdenDiseno = (Label)e.Row.FindControl("lbl_FacturasPartidas_OrdenDiseno");
                Label lbl_NombreLogoOd = (Label)e.Row.FindControl("lbl_NombreLogoOd");

                if (lbl_FacturasPartidas_OrdenDiseno.Text.Length > 0)
                {
                    lbl_FacturasPartidas_OrdenDiseno.Text = lbl_NombreLogoOd.Text + " ( " + lbl_FacturasPartidas_OrdenDiseno.Text + " ) ";
                }

                lkn_descargar.CommandArgument = e.Row.DataItemIndex.ToString();

                DataTable dt = (DataTable)ViewState["dtVales"];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (lbl_FacturasPartidas_Clave.Text == dr["TrasManPartidasClavePro"].ToString())
                        {
                            lbl_Vales.Text = dr["TrasManPartidasCantidadEnvia"].ToString();
                        }
                    }

                }

                string Buffer = lbl_FacturasPartidas_Bufer.Text.Length <= 0 ? "0" : lbl_FacturasPartidas_Bufer.Text;
                string Vales = lbl_Vales.Text.Length <= 0 ? "0" : lbl_Vales.Text;
                lbl_Vales.Text = Vales;
               
     

                if (lbl_FacturasPartidas_Produccion.Text == "S")
                {
                    lbl_FacturasPartidas_Produccion.Text = "Si";
                }
                else
                {
                    lbl_FacturasPartidas_Produccion.Text = "No";
                }

                lbl_Suma.Text = (int.Parse(Buffer) + int.Parse(Vales) + int.Parse(lbl_Neto.Text)).ToString();

                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");

            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void gridDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lbl_CatDcumentosId = (Label)e.Row.FindControl("lbl_CatDcumentosId");
                Label lbl_Pzas = (Label)e.Row.FindControl("lbl_Pzas");
                Label lbl_DocumentosRelacioFolio = (Label)e.Row.FindControl("lbl_DocumentosRelacioFolio");
                Label lbl_DocStatus = (Label)e.Row.FindControl("lbl_DocStatus");
                
                HyperLink HypFolio = (HyperLink)e.Row.FindControl("HypFolio");

                DocumentosRelacio obj_Pzas = new DocumentosRelacio();

                obj_Pzas.CatDcumentosId = SqlInt32.Parse(lbl_CatDcumentosId.Text);
                obj_Pzas.FacturasId = SqlInt32.Parse(lbl_IdHojaViaje.Text);
                obj_Pzas.SelectOneDocRelacionadosPzas();

                if (!obj_Pzas.Pzas.IsNull)
                {
                    lbl_Pzas.Text = obj_Pzas.Pzas.ToString();
                }

                if (lbl_CatDcumentosId.Text == "1")
                {
                    Facturas obj_SelecDocentry = new Facturas();
                    obj_SelecDocentry.Facturas_DocNum = SqlInt32.Parse(lbl_DocumentosRelacioFolio.Text);
                    obj_SelecDocentry.SelectFacturaDocEntry();


                    TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 });
                    QueryString["DocEntry"] = obj_SelecDocentry.Facturas_DocEntry.ToString();
                    QueryString["DocStatus"] = lbl_DocStatus.Text;
                    QueryString["TipoDoc"] ="FACTURA";

            HypFolio.NavigateUrl = "http://www2.promoshop.com.mx/Factura_Electro/Default.aspx?data=" + HttpUtility.UrlEncode(QueryString.ToString());
       
                }
                else
                {

                    HypFolio.Enabled = false;
                }


            }


        }
        catch (Exception ex)
        {

        }
    }
    protected void grid_TimeEstatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_LogStatusFacTimeStamp = (Label)e.Row.FindControl("lbl_LogStatusFacTimeStamp");
            Label lbl_Time_Tras = (Label)e.Row.FindControl("lbl_Time_Tras");
            Label lbl_Cat_Status_Descrip = (Label)e.Row.FindControl("lbl_Cat_Status_Descrip");

            LogStatusFac obj_SelectTime = new LogStatusFac();

            if (lbl_TimeInicio.Text.Length == 0)
            {
                lbl_Time_Tras.Text = "0";
                lbl_TimeInicio.Text = lbl_LogStatusFacTimeStamp.Text;
            }
            else
            {
                DateTime dateInicio = DateTime.Parse(lbl_TimeInicio.Text.Length == 0 ? "0" : lbl_TimeInicio.Text);
                DateTime dateFin = DateTime.Parse(lbl_LogStatusFacTimeStamp.Text.Length == 0 ? lbl_TimeInicio.Text : lbl_LogStatusFacTimeStamp.Text);
                //'2010-09-06T09:00:00'

                String FechaInicio = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", dateInicio);
                String FechaFin = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", dateFin);
                
                string Hora = string.Empty;
                string Minutos = string.Empty;

                DataTable dtTiempo = IQueryableToDataTable(GetDatosTiempoEstatusPar(FechaInicio, FechaFin));
                if (dtTiempo.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTiempo.Rows)
                    {
                        Hora = dr["Horas"].ToString();
                        Minutos = dr["Minutos"].ToString();
                    }
                }

                lbl_Time_Tras.Text = Hora + " Horas " + Minutos + " Minutos ";


                lbl_TimeInicio.Text = lbl_LogStatusFacTimeStamp.Text;
            }



        }
    }
    protected void lkn_descargar_Command(object sender, CommandEventArgs e)
    {

        int index = Convert.ToInt16(e.CommandArgument);

        GridViewRow row = GridMesaControl1_Hijo.Rows[index];

        Label lbl_OrdenDisenoId = (Label)row.FindControl("lbl_OrdenDisenoId");
        string id = lbl_OrdenDisenoId.Text;

        TBL_O_ORDENES_DISENO obj_ruta_final = new TBL_O_ORDENES_DISENO();

        obj_ruta_final.Id_od = SqlInt32.Parse(id);
        obj_ruta_final.Select_Ruta_Archi_Od();
        string ruta = obj_ruta_final.Chr_Ruta_Arch_Final.ToString();

        FileInfo file2 = new FileInfo(ruta);


        Response.ClearContent();


        Response.AddHeader("Content-Disposition", "attachment; filename=" + file2.Name);


        Response.AddHeader("Content-Length", file2.Length.ToString());


        Response.ContentType = ReturnExtension(file2.Extension.ToLower());


        Response.TransmitFile(file2.FullName);

        Response.End();
        file2.Delete();
    }
    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {

            case ".tif":
                return "image/tiff";

            case ".gif":
                return "image/gif";
            case ".jpg":
            case ".bmp":
                return "image/bmp";
            case ".rtf":
                return "application/rtf";

            case ".pdf":
                return "application/pdf";

            default:
                return "application/octet-stream";
        }
    }
    #endregion

    #region Metodos
    private void CargaDatos()
    {
        try
        {

         DataTable dtDatCabeze=IQueryableToDataTable(GetDatosCabezeraResum());
         foreach (DataRow dr in dtDatCabeze.Rows)
         {
             lbl_DocNum.Text = dr["FacturaSAP"].ToString();
             lbl_IdHojaViaje.Text= dr["HojaViaje"].ToString();
             lbl_NumCliente.Text= dr["Cliente"].ToString();
             lbl_NombreCliente.Text= dr["N_Cliente"].ToString();
             lbl_Vendedor.Text= dr["SlpName"].ToString();
             lbl_PerfilCliente.Text= dr["PerfilClie"].ToString();
             lblAlmacenfac.Text = dr["AlmacenFact"].ToString();
             lblAlmacenEmb.Text = dr["AlmacenEmb"].ToString();

         }

         DataTable dtTrans = IQueryableToDataTable(GetDatosTransPorteResum());
         if (dtTrans.Rows.Count > 0)
         {
             grvTransporte.DataSource = dtTrans;
             grvTransporte.DataBind();

         }

         DataTable dtDirEntre = IQueryableToDataTable(GetDatosDirEntregaResum());
         if (dtDirEntre.Rows.Count > 0)
         {
             GridDir.DataSource = dtDirEntre;
             GridDir.DataBind();

         }

         DataTable dt = IQueryableToDataTable(GetDatosValesPar());
         ViewState["dtVales"] = dt;

         DataTable dtPartidas = IQueryableToDataTable(GetDatosPartidasResum());
         if (dtPartidas.Rows.Count > 0)
         {
             GridMesaControl1_Hijo.DataSource = dtPartidas;
             GridMesaControl1_Hijo.DataBind();
         
         }       
        

         DataTable dtDoc = IQueryableToDataTable(GetDatosDocumentPar());
         if (dtDoc.Rows.Count > 0)
         {
             gridDocumentos.DataSource = dtDoc;
             gridDocumentos.DataBind();
         }

         DataTable dtLogSta = IQueryableToDataTable(GetDatosLogStatus());
         if (dtLogSta.Rows.Count > 0)
         {
             grid_TimeEstatus.DataSource = dtLogSta;
             grid_TimeEstatus.DataBind();
         }

        }
        catch (Exception ex)
        {

        }
    }
    protected IQueryable GetDatosCabezeraResum()
    {
        DataClassesDataContext db = new DataClassesDataContext();       

        var c = from f in db.vwFactura
                where f.HojaViaje == int.Parse(lbl_IdHojaViaje.Text)
                select new { 
                    f.FacturaSAP , 
                     f.HojaViaje , 
                       f.Cliente , 
                     f.N_Cliente ,
                       f.SlpName , 
                     f.PerfilClie,
                     f.AlmacenEmb,
                     f.AlmacenFact,
                    
                    
                };

        return c;
    }
    protected IQueryable GetDatosTransPorteResum()
    {
        DataClassesDataContext db = new DataClassesDataContext();

        var c = from Tra in db.Transfer
                join TraP in db.TransferPartidas on Tra.TransferId equals TraP.TransferID
                where TraP.FacturasId == int.Parse(lbl_IdHojaViaje.Text)
                select new { Tra.NumTransID, Tra.Placas, Tra.Sello, Tra.TotalBultos, Tra.FechaCreacion };
        return c;
    }
    protected IQueryable GetDatosPartidasResum()
    {
        DataClassesDataContext db = new DataClassesDataContext();
        
        var c = from vP in db.vwPartidas
                where vP.HojaViaje == int.Parse(lbl_IdHojaViaje.Text)
                select new
                {
                    vP.Partida
                  ,
                    vP.Clave
                  ,
                    vP.Descripcion
                  ,
                    vP.PartProd
                  ,
                    vP.OD
                  ,
                    vP.Cantidad
                  ,
                    vP.NC
                  ,
                    vP.Neto
                  ,
                    vP.Buffer
                  ,
                    vP.Vales
                 ,
                    vP.Bultos 
                  ,
                 NombreLogoOd = db.ODNombreLogo(vP.OD)

                };
              
        return c;
    }
    protected IQueryable GetDatosDirEntregaResum()
    {
        DataClassesDataContext db = new DataClassesDataContext();

        var c = from vF in db.vwFactura
                join F in db.Facturas20 on vF.HojaViaje equals F.FacturasId
                where vF.HojaViaje == int.Parse(lbl_IdHojaViaje.Text)
                select new
                {
                    vF.HojaViaje,
                    vF.TipoDeEntrega,
                    F.Facturas_DirEmbar,
                    vF.Paquetera
                };
        return c;


    }
    protected IQueryable GetDatosValesPar()
    {
        DataClassesDataContext db = new DataClassesDataContext();

        var c = db.sp_post_SelecVales(int.Parse(lbl_IdHojaViaje.Text));
            
        return c.AsQueryable();
        
        
    }
    protected IQueryable GetDatosDocumentPar()
    {
        DataClassesDataContext db = new DataClassesDataContext();

        var c = db.sp_post_DocumentosRelacio_SelectDoc(int.Parse(lbl_IdHojaViaje.Text));

        return c.AsQueryable();


    }
    protected IQueryable GetDatosTiempoEstatusPar(string FechaInicio,string FechaFin)
    {
        DataClassesDataContext db = new DataClassesDataContext();

        var c = db.sp_post_CalculaTime(FechaInicio,FechaFin);

        return c.AsQueryable();


    }
    protected IQueryable GetDatosLogStatus()
    {
        DataClassesDataContext db = new DataClassesDataContext();
        var c = from Logs in db.LogStatusFac20
                join UsuDes in  db.Usuario on Logs.UsuarioId equals UsuDes.UsuarioId
                join CatEst in db.Cat_Status on Logs.Cat_Status_Id equals CatEst.Cat_Status_Id
                where Logs.FacturasId == int.Parse(lbl_IdHojaViaje.Text)
                orderby Logs.LogStatusFacId
                select new
                {
                    Logs.LogStatusFacTimeStamp
                   ,CatEst.Cat_Status_Descrip
                   ,UsuDes.UsuarioDescrip

                };

        return c;
    }
    
    protected DataTable IQueryableToDataTable(IQueryable iq)
    {
        DataTable dt = new DataTable();

        foreach (object obj in iq)
        {
            PropertyInfo[] pI = obj.GetType().GetProperties();
            if (dt.Columns.Count == 0)
            {
                DataColumn col;
                foreach (PropertyInfo c in pI)
                {
                    if (c.PropertyType.IsGenericType == false)
                    {
                        col = new DataColumn(c.Name, c.PropertyType);
                    }
                    else
                    {
                        //string x = c.PropertyType.FullName.ToString().Replace("`1", "|").Replace("[[", "").Replace("]]", "").Split('|')[1].Split(',')[0].ToString();
                        col = new DataColumn(c.Name, System.Type.GetType(c.PropertyType.FullName.ToString().Replace("`1", "|").Replace("[[", "").Replace("]]", "").Split('|')[1].Split(',')[0].ToString()));
                        col.AllowDBNull = true;
                    }
                    dt.Columns.Add(col);
                }
            }

            DataRow dr = dt.NewRow();
            foreach (PropertyInfo r in pI)
            {
                if (r.PropertyType.IsGenericType == false)
                {
                    dr[r.Name] = r.GetValue(obj, null);
                }
                else
                {
                    dr[r.Name] = (r.GetValue(obj, null) == null) ? DBNull.Value : r.GetValue(obj, null);
                }
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }
    #endregion




  
}
