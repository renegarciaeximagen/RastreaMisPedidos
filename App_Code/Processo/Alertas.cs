using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Componente.Conexion;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;


namespace Componente.Procesos
{
    /// <summary>
    /// Descripción breve de Alertas
    /// </summary>
    public class Alertas
    {
        private static string MyStringConexion()
        {
            return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["TrackingPostConnectionString"].ConnectionString;
            //return "Data Source=eximts; Initial Catalog=TrackingPost; User Id=sa; PWD=2NC]>=f.N^";
        }

        public void ValidarCredito(string CardCode, string Factura, Int32 FacturaID)
        {
            try
            {
                DataTable dt = new DataTable("MyDataTable");
                LogStatusFac obj_Insert = new LogStatusFac();

                using (SqlConnection miConex = new SqlConnection(MyStringConexion()))
                {
                    miConex.Open();
                    SqlCommand cmd = new SqlCommand("sp_CondCredito", miConex);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm = new SqlParameter("@CardCode", SqlDbType.VarChar);
                    prm.Value = CardCode;
                    cmd.Parameters.Add(prm);
                    prm = null;

                    prm = new SqlParameter("@DocNum", SqlDbType.Int);
                    prm.Value = SqlInt32.Parse(Factura);
                    cmd.Parameters.Add(prm);
                    prm = null;

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }

                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["TipoValidate"].ToString() != "A")
                    {
                        //enviarMail(Factura);
                        obj_Insert.LogStatusFacTimeStamp = DateTime.Now;
                        obj_Insert.Cat_Status_Id = 18;
                        obj_Insert.UsuarioId = 1;
                        obj_Insert.FacturasId = SqlInt32.Parse(FacturaID.ToString());
                        obj_Insert.Insert();

                    }
                    else
                    {
                        //enviarMail(Factura);
                        obj_Insert.LogStatusFacTimeStamp = DateTime.Now;
                        obj_Insert.Cat_Status_Id = 13;
                        obj_Insert.UsuarioId = 1;
                        obj_Insert.FacturasId = SqlInt32.Parse(FacturaID.ToString());
                        obj_Insert.Insert();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[ValidarCredito]-> " + ex.Message);
            }
        }

        public void ValidarEntregaCompletaKN(int FacturasId, int LineNum, int CantSurtida)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                String strDe, strPara, strAsunto, strMsj;
                ArrayList strConCopia = new ArrayList();

                strDe = string.Empty;
                strPara = string.Empty;
                strAsunto = string.Empty;
                strMsj = string.Empty;

                dt = GetDatos(FacturasId, LineNum);
                dt2 = GetCorreos(1);


                if (dt.Rows.Count != 0)
                {
                    //strDe = "test1@eximagen.com.mx";
                    strDe = dt2.Rows[0]["De"].ToString();
                    strPara = dt.Rows[0]["MailVend"].ToString();
                    //strPara = "marcos.barojas@eximagen.com.mx";
                   

                    for (int i = 0; i < dt2.Rows[0]["ConCopiaPara"].ToString().Split(',').Length; i++)
                    {
                        strConCopia.Add(dt2.Rows[0]["ConCopiaPara"].ToString().Split(',')[i]);
                    }

                    if (dt.Rows[0]["MailAC"].ToString() != "")
                    {
                        strConCopia.Add(dt.Rows[0]["MailAC"].ToString());
                    }

                    if (CantSurtida != (int.Parse(dt.Rows[0]["Cantidad"].ToString()) + int.Parse(dt.Rows[0]["Buffer"].ToString())))
                    {
                        if (CantSurtida == int.Parse(dt.Rows[0]["Cantidad"].ToString()) || ((CantSurtida > int.Parse(dt.Rows[0]["Cantidad"].ToString())) && (CantSurtida < (int.Parse(dt.Rows[0]["Cantidad"].ToString()) + int.Parse(dt.Rows[0]["Buffer"].ToString())))))
                        {
                            strAsunto = "PROBABLE ENTREGA INCOMPLETA – CLIENTE: " + dt.Rows[0]["CardCode"].ToString() + ", FACTURA: " + dt.Rows[0]["DocNum"].ToString() + ", PRODUCTO: " + dt.Rows[0]["Clave"].ToString();

                            strMsj = "<table>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table style=\"width: 100%\">" +
                                                    "<tr>" +
                                                        "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                                                        "<td></td>" +
                                                        "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Cliente:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Factura:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["DocNum"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Fecha:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["DocDate"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Cantidad:" +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["Cantidad"].ToString() +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Pzas." +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Producto:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                             dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Orden De Diseño:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["OD"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Vendedor:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["Vendedor"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Asesor Comercial:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["AC"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                                                "Reserva de inventario adicional al facturado para posibles mermas de producción: " +
                                                "<b>NO DISPONIBLE, PROBABLE ENTREGA MENOR AL TOTAL FACTURADO.</b>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table>" +
                                                    "<tr>" +
                                                        "<td>" +
                                                            "<img src=\"cid:face\"/>" +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                                            "<b>Hola Amigo Cliente…</b>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                                                "Para esta producción, nuestro sistema reservaría automáticamente " + dt.Rows[0]["Buffer"].ToString() + " Pzas. adicionales " +
                                                "a tu cantidad facturada, para prever el caso de que exista alguna merma de producción, " +
                                                "y esta se resolviera de manera inmediata con esta reserva.<br />" +
                                                "Desafortunadamente no" +
                                                "contamos con el inventario suficiente para hacer esta reserva, y así poder subsanar " +
                                                "posibles errores naturales de una producción. Haremos nuestro mejor esfuerzo para " +
                                                "no generar ninguna merma durante el proceso de producción.<br />" +
                                                "Sin embargo, en el desafortunado" +
                                                "caso de que si se presentarán, no nos será posible entregar el total de piezas facturadas." +
                                                "<br /><br />" +
                                                "En las siguientes horas comenzaremos con la producción de tus mercancías para entregarlas " +
                                                "en el tiempo acordado." +
                                                "<br /><br />" +
                                                "Si no te es posible recibir un probable faltante por mermas" +
                                                "de producción tienes la opción de cancelar tu factura, si así lo deseas, te solicitamos " +
                                                "de la manera más atenta contactes a tu vendedor para solicitar esta cancelación " +
                                                "previo a que iniciemos producción, ya que una vez iniciada la producción, no existe " +
                                                "la opción de cancelación." +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                                                "<b>Saludos</b></td>" +
                                        "</tr>" +
                                    "</table>";
                        }
                        else if (CantSurtida < int.Parse(dt.Rows[0]["Cantidad"].ToString()))
                        {
                            strAsunto = "DIFERENCIA  DE INVENTARIO. ENTREGA INCOMPELTA – CLIENTE: " + dt.Rows[0]["CardCode"].ToString() + ", FACTURA: " + dt.Rows[0]["DocNum"].ToString() + ", PRODUCTO: " + dt.Rows[0]["Clave"].ToString();

                            strMsj = "<table>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table style=\"width: 100%\">" +
                                                    "<tr>" +
                                                        "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                                                        "<td></td>" +
                                                        "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Cliente:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Factura:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["DocNum"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Fecha:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["DocDate"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Cantidad Original:" +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                                            dt.Rows[0]["Cantidad"].ToString() +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Pzas." +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Cantidad Surtida:" +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                                            CantSurtida +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Pzas." +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Producto:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                             dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Orden De Diseño:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["OD"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Vendedor:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["Vendedor"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                            "Asesor Comercial:" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                            dt.Rows[0]["AC"].ToString() +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                                                "Diferencia de Inventario: <b>DIFERENCIA DE INVENTARIO, ENTREGA INCOMPLETA.</b>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table>" +
                                                    "<tr>" +
                                                        "<td>" +
                                                            "<img src=\"cid:face\"/>" +
                                                        "</td>" +
                                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                                            "<b>Hola Amigo Cliente…</b>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                                            "Nos disculpamos contigo por la diferencia de inventario que nuestro sistema muestra, lo que nos impide surtir la totalidad de la mercancía antes descrita, y para el caso de una producción no contar con margen de posibles errores naturales de una producción." +
                                            "<br />" +
                                            "En breve te contactará tu vendedor para ofrecerte alternativas para resolver esta situación." +
                                            "<br />" +
                                            "Por tu compresión, Gracias." +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                                                "<b>Saludos</b></td>" +
                                        "</tr>" +
                                    "</table>";
                        }
                        enviarMail(strDe, strPara, strAsunto, strMsj, strConCopia);
                    }
                   // enviarMail(strDe, strPara, strAsunto, strMsj, strConCopia);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error:[Alertas].[ValidarEntregaCompletaKN]->" + ex.Message);
            }
        }

        public void AlertaSegundoPicking(int FacturasId, int LineNum, int CantSurtida)
        {
            DataTable dt2 = new DataTable();
            DataTable dt = new DataTable();
            string strDe, strPara, strAsunto, strMsj;
            ArrayList strConCopia = new ArrayList();

            strDe = string.Empty;
            strPara = string.Empty;
            strAsunto = string.Empty;
            strMsj = string.Empty;

            dt = GetDatos(FacturasId, LineNum);
            dt2 = GetCorreos(2);

            //strConCopia = new string[] {"roberto.tovar@eximagen.com.mx"};

            if (dt.Rows.Count != 0)
            {
                if (CantSurtida < int.Parse(dt.Rows[0]["Cantidad"].ToString()))
                {
                    //strDe = "test1@eximagen.com.mx";
                    //strPara = "ruben.miranda@eximagen.com.mx";
                    strDe = dt2.Rows[0]["De"].ToString();
                    strPara = dt2.Rows[0]["Para"].ToString();

                    for (int i = 0; i < dt2.Rows[0]["ConCopiaPara"].ToString().Split(',').Length; i++)
                    {
                        strConCopia.Add(dt2.Rows[0]["ConCopiaPara"].ToString().Split(',')[i]);
                    }

                    strAsunto = "RECEPCION DE PRODUCTO INCOMPLETA EN XHALA – FOLIO DE VIAJE " + dt.Rows[0]["FacturasId"].ToString();

                    strMsj = "<table>" +
                                    "<tr>" +
                                        "<td>" +
                                            "<table style=\"width: 100%\">" +
                                                "<tr>" +
                                                    "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                                                    "<td></td>" +
                                                    "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td>" +
                                            "<table>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Cliente:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                        dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Factura:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                        dt.Rows[0]["DocNum"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Fecha:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                        dt.Rows[0]["DocDate"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Cantidad:" +
                                                    "</td>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                                        dt.Rows[0]["Cantidad"].ToString() +
                                                    "</td>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Pzas." +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Producto:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                         dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Orden De Diseño:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                        dt.Rows[0]["OD"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Vendedor:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                        dt.Rows[0]["Vendedor"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                                        "Asesor Comercial:" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                                        dt.Rows[0]["AC"].ToString() +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                                            "<b>ENTREGA INCOMPLETA.</b>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td>" +
                                            "<table>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<img src=\"cid:face\"/>" +
                                                    "</td>" +
                                                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                                        "<b>Hola</b>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                                        "LAS MERCANCIAS ENVIADAS A XHALA BAJO EL FOLIO DE VIAJE " + dt.Rows[0]["FacturasId"].ToString() + " FUERON RECIBIDAS PARCIALMENTE, POR LO QUE NO SERAN PROCESADAS HASTA RECIBIR LA CANTIDAD EXACTA." +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                                            "<b>Saludos</b></td>" +
                                    "</tr>" +
                                "</table>";
                }
            }
            enviarMail(strDe, strPara, strAsunto, strMsj, strConCopia);
        }

        public void AlertaFaltantesProduccion(int FacturasId, int LineNum, int Cantidad) {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            string strDe, strPara, strAsunto, strMsj;
            ArrayList strConCopia = new ArrayList();

            strDe = string.Empty;
            strPara = string.Empty;
            strAsunto = string.Empty;
            strMsj = string.Empty;

            //strDe = "test1@eximagen.com.mx";

            //strPara = "roberto.tovar@eximagen.com.mx";

            //strConCopia = new string[] {"antonio.camacho@eximagen.com.mx" };

            dt = GetDatos(FacturasId, LineNum);
            dt2 = GetCorreos(3);

            strDe = dt2.Rows[0]["De"].ToString();
            strPara = dt2.Rows[0]["Para"].ToString();

            for (int i = 0; i < dt2.Rows[0]["ConCopiaPara"].ToString().Split(',').Length; i++)
            {
                strConCopia.Add(dt2.Rows[0]["ConCopiaPara"].ToString().Split(',')[i]);
            }

            strAsunto = "FALTANTES DE PRODUCCIÓN – FOLIO DE VIAJE " + dt.Rows[0]["FacturasId"].ToString();

            strMsj = "<table>" +
                "<tr>" +
                    "<td>" +
                        "<table style=\"width: 100%\">" +
                            "<tr>" +
                                "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                                "<td></td>" +
                                "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Cliente:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Factura:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["DocNum"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Fecha:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["DocDate"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Cantidad:" +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                    dt.Rows[0]["Cantidad"].ToString() +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Pzas." +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Producto:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                     dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Orden De Diseño:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["OD"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Vendedor:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["Vendedor"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Asesor Comercial:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["AC"].ToString() +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                        "<b>FALTANTES DE PRODUCCIÓN.</b>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table>" +
                            "<tr>" +
                                "<td>" +
                                    "<img src=\"cid:face\"/>" +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                    "<b>Hola</b>" +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                    "La hoja de viaje " + dt.Rows[0]["DocNum"].ToString() + " presento un faltante de producción por " + Cantidad + " Pzas. para el producto " + dt.Rows[0]["DocNum"].ToString() + "." +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                        "<b>Saludos</b></td>" +
                "</tr>" +
            "</table>";

            enviarMail(strDe, strPara, strAsunto, strMsj, strConCopia);
        }

        public void AlertaFaltantesOrigen(int FacturasId, int LineNum, int Cantidad)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            string strDe, strPara, strAsunto, strMsj;
            ArrayList strConCopia = new ArrayList();

            strDe = string.Empty;
            strPara = string.Empty;
            strAsunto = string.Empty;
            strMsj = string.Empty;

            //strDe = "test1@eximagen.com.mx";

            //strPara = "marcos.barojas@eximagen.com.mx";

            //strConCopia = new string[] { "oliverio.vargas@eximagen.com.mx", "alejandra.medina@eximagen.com.mx" };

            dt2 = GetCorreos(4);

            strDe = dt2.Rows[0]["De"].ToString();
            strPara = dt2.Rows[0]["Para"].ToString();

            for (int i = 0; i < dt2.Rows[0]["ConCopiaPara"].ToString().Split(',').Length; i++)
            {
                strConCopia.Add(dt2.Rows[0]["ConCopiaPara"].ToString().Split(',')[i]);
            }

            strAsunto = "FALTANTES DE ORIGEN – FOLIO DE VIAJE " + dt.Rows[0]["FacturasId"].ToString();

            dt = GetDatos(FacturasId, LineNum);

            strMsj = "<table>" +
                "<tr>" +
                    "<td>" +
                        "<table style=\"width: 100%\">" +
                            "<tr>" +
                                "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                                "<td></td>" +
                                "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Cliente:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Factura:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["DocNum"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Fecha:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["DocDate"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Cantidad:" +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                    dt.Rows[0]["Cantidad"].ToString() +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Pzas." +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Producto:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                     dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Orden De Diseño:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["OD"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Vendedor:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["Vendedor"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Asesor Comercial:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["AC"].ToString() +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                        "<b>FALTANTES DE ORIGEN.</b>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table>" +
                            "<tr>" +
                                "<td>" +
                                    "<img src=\"cid:face\"/>" +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                    "<b>Hola</b>" +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                    "La hoja de viaje " + dt.Rows[0]["DocNum"].ToString() + " presento un faltante de origen por " + Cantidad + " Pzas. para el producto " + dt.Rows[0]["DocNum"].ToString() + "." +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                        "<b>Saludos</b></td>" +
                "</tr>" +
            "</table>";

            enviarMail(strDe, strPara, strAsunto, strMsj, strConCopia);
        }

        public void AlertaFacturarMercancias(int FacturasId, int LineNum, int Cantidad, string FacturarA) {

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            string strDe, strPara, strAsunto, strMsj;
            
            strDe = string.Empty;
            strPara = string.Empty;
            strAsunto = string.Empty;
            strMsj = string.Empty;

            //strDe = "test1@eximagen.com.mx";
            //strPara = "todocontabilidad@eximagen.com.mx";

            dt2 = GetCorreos(5);

            strDe = dt2.Rows[0]["De"].ToString();
            strPara = dt2.Rows[0]["Para"].ToString();

            strAsunto = "Facturar Mercancias";

            dt = GetDatos(FacturasId, LineNum);

            strMsj = "<table>" +
                "<tr>" +
                    "<td>" +
                        "<table style=\"width: 100%\">" +
                            "<tr>" +
                                "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                                "<td></td>" +
                                "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Cliente:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Factura:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["DocNum"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Fecha:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["DocDate"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Cantidad:" +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                    dt.Rows[0]["Cantidad"].ToString() +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Pzas." +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Producto:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                     dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Orden De Diseño:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["OD"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Vendedor:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["Vendedor"].ToString() +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                    "Asesor Comercial:" +
                                "</td>" +
                                "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                    dt.Rows[0]["AC"].ToString() +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                        "<b>Facturar Mercancias.</b>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table>" +
                            "<tr>" +
                                "<td>" +
                                    "<img src=\"cid:face\"/>" +
                                "</td>" +
                                "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                    "<b>Hola</b>" +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                    "Por favor realizar un factura para " + FacturarA + " a causa de un faltante del producto " + dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() + " por la cantidad de " + Cantidad  + " pzas." + 
                    "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                        "<b>Saludos gracias.</b></td>" +
                "</tr>" +
            "</table>";

            enviarMail(strDe, strPara, strAsunto, strMsj);  
        }

        public void AlertaSolicitudSalidaMercancia(int FacturasId, int LineNum, int Cantidad) {

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            string strDe, strPara, strAsunto, strMsj;

            strDe = string.Empty;
            strPara = string.Empty;
            strAsunto = string.Empty;
            strMsj = string.Empty;

            //strDe = "test1@eximagen.com.mx";
            //strPara = "todocontabilidad@eximagen.com.mx";
            dt2 = GetCorreos(6);

            strDe = dt2.Rows[0]["De"].ToString();
            strPara = dt2.Rows[0]["Para"].ToString();

            strAsunto = "Solicitud salida de Mercancias";

            dt = GetDatos(FacturasId, LineNum);

            strMsj = "<table>" +
                "<tr>" +
                "<td>" +
                    "<table style=\"width: 100%\">" +
                        "<tr>" +
                            "<td style=\"width: 20%\"><img src=\"cid:logo\"/></td>" +
                            "<td></td>" +
                            "<td style=\"width: 20%\"><img src=\"cid:logo2\"/></td>" +
                        "</tr>" +
                    "</table>" +
                "</td>" +
            "</tr>" +
            "<tr>" +
                "<td>" +
                    "<table>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Cliente:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                dt.Rows[0]["CardCode"].ToString() + " - " + dt.Rows[0]["CardName"].ToString() +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Factura:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                dt.Rows[0]["DocNum"].ToString() +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Fecha:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                dt.Rows[0]["DocDate"].ToString() +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Cantidad:" +
                            "</td>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: right;\">" +
                                dt.Rows[0]["Cantidad"].ToString() +
                            "</td>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Pzas." +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Producto:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                 dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Orden De Diseño:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                dt.Rows[0]["OD"].ToString() +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Vendedor:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                dt.Rows[0]["Vendedor"].ToString() +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; font-weight: bold;\">" +
                                "Asesor Comercial:" +
                            "</td>" +
                            "<td colspan=\"2\" style=\"font-family: Arial, Sans-Serif; font-size: 11.05px; text-align: left;\">" +
                                dt.Rows[0]["AC"].ToString() +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                "</td>" +
            "</tr>" +
            "<tr>" +
                "<td style=\"font-family: Arial, Sans-Serif; font-size: 11.5px; text-align: justify; padding-top: 14px; padding-bottom: 14px;\">" +
                    "<b>Facturar Mercancias.</b>" +
                "</td>" +
            "</tr>" +
            "<tr>" +
                "<td>" +
                    "<table>" +
                        "<tr>" +
                            "<td>" +
                                "<img src=\"cid:face\"/>" +
                            "</td>" +
                            "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify;\">" +
                                "<b>Hola</b>" +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                "</td>" +
            "</tr>" +
            "<tr>" +
                "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align: justify; padding-top: 14px;\">" +
                "Por favor realizar una salida de mercancias del producto " + dt.Rows[0]["Clave"].ToString() + " - " + dt.Rows[0]["Descripcion"].ToString() + " por la cantidad de " + Cantidad + " pzas." +
                "</td>" +
            "</tr>" +
            "<tr>" +
                "<td style=\"font-family: Arial, Sans-Serif; font-size: 13px; text-align:left; padding-top: 14px;\">" +
                    "<b>Saludos gracias.</b></td>" +
            "</tr>" +
        "</table>";

        enviarMail(strDe, strPara, strAsunto, strMsj);
   }
        
        public void enviarMail(string strDe, string strPara, string strAsunto, string strMsj, params ArrayList[] strConCopia)
        {
            try
            {
                MailMessage msj = new MailMessage();

                msj.From = new MailAddress(strDe);
                //msj.To.Add("marcos.barojas@eximagen.com.mx");
                msj.To.Add(strPara);

                for (int i = 0; i <= strConCopia[0].Count-1; i++)
                {
                    if (strConCopia[0][i].ToString() != "")
                    {
                        msj.Bcc.Add(strConCopia[0][i].ToString());
                    }
                }

                msj.Subject = strAsunto;

                String path = @"E:\Img\eximagen.gif";
                String path2 = @"E:\Img\nopic_64.gif";
                String path3 = @"E:\Img\promoline.gif";

                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "logo";
                logo.ContentType.MediaType = "image/gif";

                LinkedResource face = new LinkedResource(path2);
                face.ContentId = "face";
                face.ContentType.MediaType = "image/gif";

                LinkedResource logo2 = new LinkedResource(path3);
                logo2.ContentId = "logo2";
                logo2.ContentType.MediaType = "image/gif";

                AlternateView av = AlternateView.CreateAlternateViewFromString(strMsj, null, MediaTypeNames.Text.Html);

                av.LinkedResources.Add(logo);
                av.LinkedResources.Add(face);
                av.LinkedResources.Add(logo2);

                msj.AlternateViews.Add(av);
                msj.IsBodyHtml = true;
                msj.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
                client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
                client.Send(msj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error:[Alertas].[enviarMail]->" + ex.Message);
            }
        }

        public DataTable GetDatos(int FacturasId, int LineNum)
        {
            try
            { 
                DataTable dt = new DataTable("MiTabla");
                using (SqlConnection con = new SqlConnection(MyStringConexion()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("spValidarEntregaCompletaKN", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacturasId", FacturasId);
                    cmd.Parameters.AddWithValue("@LineNum", LineNum);

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex) {
                throw new Exception("Error:[Alertas].[GetDatos]->" + ex.Message);
            }
        }

        public DataTable GetCorreos(int AlertaId){
            try
            {
                string strSQL = "SELECT T0.* \n" +
                                "FROM dbo.CorreoAlertas T0 \n" +
                                "JOIN dbo.Alertas T1 ON T1.AlertaId = T0.AlertaID AND T1.AlertaId = " + AlertaId.ToString(); 

                DataTable dt = new DataTable("TablaCorreos");
                using (SqlConnection con = new SqlConnection(MyStringConexion()))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error:[Alertas].[GetCorreos]->" + ex.Message);
            }
        }
/**************************************************************************************************************************************************/

        public void ValidarEntregaBuffer(int CantidadSurtida, string CveProducto, int NumFactura)
        {
            try
            {
                DataTable dt = new DataTable("MiTabla");

                using (SqlConnection miCon = new SqlConnection(MyStringConexion()))
                {
                    miCon.Open();

                    SqlCommand miCmd = new SqlCommand("sp_DatosValidarBufferKN", miCon);
                    miCmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm;

                    prm = new SqlParameter("@DocNum", System.Type.GetType("SqlDbType.Int"));
                    prm.Value = NumFactura;
                    miCmd.Parameters.Add(prm);
                    prm = null;

                    prm = new SqlParameter("@ItemCode", System.Type.GetType("SqlDbType.VarChar"));
                    prm.Value = CveProducto;
                    miCmd.Parameters.Add(prm);
                    prm = null;

                    SqlDataAdapter adp = new SqlDataAdapter(miCmd);
                    adp.Fill(dt);


                    if (dt.Rows.Count != 0)
                    {
                        if (int.Parse(dt.Rows[0]["TotalASurtir"].ToString()) != CantidadSurtida)
                        {

                            if ((CantidadSurtida - int.Parse(dt.Rows[0]["Cantidad"].ToString())) != 0)
                            {
                                if ((CantidadSurtida - int.Parse(dt.Rows[0]["Cantidad"].ToString())) - int.Parse(dt.Rows[0]["Buffer"].ToString()) != 0)
                                {
                                    enviarMail(Int32.Parse(dt.Rows[0]["FacturasId"].ToString()), Int32.Parse(dt.Rows[0]["PartidaId"].ToString()), dt.Rows[0]["DocNum"].ToString(), dt.Rows[0]["Producto"].ToString(), dt.Rows[0]["EmailVend"].ToString(), dt.Rows[0]["EmailAC"].ToString(), 1);
                                }

                                else
                                {

                                }
                            }
                            else
                            {
                                enviarMail(Int32.Parse(dt.Rows[0]["FacturasId"].ToString()), Int32.Parse(dt.Rows[0]["PartidaId"].ToString()), dt.Rows[0]["DocNum"].ToString(), dt.Rows[0]["Producto"].ToString(), dt.Rows[0]["EmailVend"].ToString(), dt.Rows[0]["EmailAC"].ToString(), 0);

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error ValidarEntregaBuffer " + ex.Message);
            }
        }

        public void ValidadEntregaDeProduccion(int CantProd, int Factura, string ItemCode)
        {
            try
            {
                DataTable dt = new DataTable("MyTable");

                using (SqlConnection miConex = new SqlConnection(MyStringConexion()))
                {
                    miConex.Open();
                    SqlCommand cmd = new SqlCommand("sp_ValidarProd", miConex);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm;

                    prm = new SqlParameter("@DocNum", SqlDbType.VarChar);
                    prm.Value = Factura;
                    cmd.Parameters.Add(prm);
                    prm = null;

                    prm = new SqlParameter("@ItemCode", SqlDbType.VarChar);
                    prm.Value = ItemCode;
                    cmd.Parameters.Add(prm);
                    prm = null;

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }

                if (dt.Rows.Count != 0)
                {
                    if (int.Parse(dt.Rows[0]["CantAProd"].ToString()) != CantProd)
                    {
                        enviarMail(int.Parse(dt.Rows[0]["FacturasId"].ToString()), int.Parse(dt.Rows[0]["Partidas"].ToString()), dt.Rows[0]["DocNum"].ToString(), dt.Rows[0]["ItemCode"].ToString(), dt.Rows[0]["Correo1"].ToString(), dt.Rows[0]["Correo2"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[ValidadEntregaDeProduccion]-> " + ex.Message);
            }
        }

        public void SolicitarNotaDeCredito(int FacturasId, int FacturasPartidasId)
        {
            try
            {
                DataTable dt = new DataTable("MyTable");

                using (SqlConnection miConex = new SqlConnection(MyStringConexion()))
                {
                    miConex.Open();
                    SqlCommand cmd = new SqlCommand("sp_ValidarProd2", miConex);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prm;

                    prm = new SqlParameter("@FacturasId", SqlDbType.Int);
                    prm.Value = FacturasId;
                    cmd.Parameters.Add(prm);
                    prm = null;

                    prm = new SqlParameter("@FacturasPartidasId", SqlDbType.Int);
                    prm.Value = FacturasPartidasId;
                    cmd.Parameters.Add(prm);
                    prm = null;

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }

                if (dt.Rows.Count != 0)
                {
                    enviarMail(dt.Rows[0]["DocNum"].ToString(), dt.Rows[0]["ItemCode"].ToString(), int.Parse(dt.Rows[0]["DifEnProd"].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[SolicitarNotaDeCredito]-> " + ex.Message);
            }
        }

        public void enviarMail()
        {
            try
            {
                MailMessage msj = new MailMessage();
                string msjAlerta;

                msj.From = new MailAddress("test1@eximagen.com.mx");
                msj.To.Add("lmhr200781@yahoo.com.mx");

                msjAlerta = "Prueba correo con imagenes incrustradas";

                //msj.Bcc.Add("todoproduccion@eximagen.com.mx");

                //msj.Bcc.Add("lmhr200781@yahoo.com.mx");

                msj.Subject = "Aviso Post-facturación";

                String path = @"E:\Img\eximagen.gif";
                String path2 = @"E:\Img\nopic_64.gif";
                String path3 = @"E:\Img\promoline.gif";
                //String path = Path.GetFullPath(@"\PosTFacturaKN\Img\eximagen.gif");
                //String path2 = Path.GetFullPath(@"\PosTFacturaKN\Img\nopic_64.gif");
                //String path3 = Path.GetFullPath(@"\PosTFacturaKN\Img\promoline.gif");

                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "logo";
                logo.ContentType.MediaType = "image/png";

                LinkedResource face = new LinkedResource(path2);
                face.ContentId = "face";
                face.ContentType.MediaType = "image/gif";

                LinkedResource logo2 = new LinkedResource(path3);
                logo2.ContentId = "logo2";
                logo2.ContentType.MediaType = "image/gif";


                string s = "<html>" +
                            "<body style=\" background-color:#F5FFFA;\">" +
                                "<table style=\"height: 200px\">" +
                                         "<tr>" +
                                             "<td style=\"padding-bottom: 85px;\">" +
                                                       "<img  src=\"cid:logo\"/>" +
                                             "</td>" +
                                             "<td>" +
                                                "<center><h2>Alerta Post-Facturaci&oacute;n</h2></center>" +
                                                       "<table>" +
                                                            "<tr>" +
                                                               "<td>" +
                                                                  "<img src=\"cid:face\"/>" +
                                                                "</td>" +
                                                                "<td>" +
                                                                    "<b>Hola</b>" +
                                                                    "<br>" +
                                                                     msjAlerta +

                                                                    "<table>" +
                                                                        "<tr>" +
                                                                           "<td colspan=3>" +
                                                                                "¿Cliente acepta entrega incompleta del producto?" +
                                                                           "<td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                          "<td>" +
                                                                            "<center>" +
                                                                             "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://www.google.com.mx/ig?hl=es>Si</a></p>" +
                                                                            "</center>" +
                                                                          "</td>" +
                                                                          "<td>" +
                                                                          "</td>" +
                                                                          "<td>" +
                                                                             "<center>" +
                                                                                "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://localhost:2411/PosTFacturaKN/ValidarEntregaParcial.aspx?Val1=No&Val2=  >No</a></p>" +
                                                                             "</center>" +
                                                                          "</td>" +
                                                                        "</tr>" +
                                                                    "</table>" +

                                                                "</td>" +
                                                             "</tr>" +
                                                          "</table>" +
                                                           "</td>" +
                                                           "<td style=\"padding-bottom: 85px;\">" +
                                                                "<img src=\"cid:logo2\"/>" +
                                                           "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                        "<td colspan=3 style=\" font-size: xx-small; color:#D8D8D8;\">" +
                                                            "Enviado el " + DateTime.Now.ToString() +
                                                        "</td>" +
                                                       "</tr>" +
                                                    "</table>" +
                                                    "</body>" +
                                                    "</html>";

                AlternateView av = AlternateView.CreateAlternateViewFromString(s, null, MediaTypeNames.Text.Html);

                av.LinkedResources.Add(logo);
                av.LinkedResources.Add(face);
                av.LinkedResources.Add(logo2);

                msj.AlternateViews.Add(av);
                msj.IsBodyHtml = true;
                msj.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
                client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
                client.Send(msj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[enviarMail]-> " + ex.Message);
            }
        }

        //Credito autorizacion manual
        public void enviarMail(string DocNum)
        {
            try
            {
                MailMessage msj = new MailMessage();

                msj.From = new MailAddress("test1@eximagen.com.mx");
                //msj.To.Add("Todotesoreria@eximagen.com.mx");

                //msj.Bcc.Add("todoproduccion@eximagen.com.mx");

                msj.Bcc.Add("lmhr200781@yahoo.com.mx");

                msj.Subject = "Aviso Post-facturación";

                String path = @"E:\Img\eximagen.gif";
                String path2 = @"E:\Img\nopic_64.gif";
                String path3 = @"E:\Img\promoline.gif";

                //String path = Path.GetFullPath(@"\PosTFacturaKN\Img\eximagen.gif");
                //String path2 = Path.GetFullPath(@"\PosTFacturaKN\Img\nopic_64.gif");
                //String path3 = Path.GetFullPath(@"\PosTFacturaKN\Img\promoline.gif");

                //String path =  HttpServerUtility.Server.MapPath(@"\Img\eximagen.gif");
                //String path2 = Server.MapPath(@"\Img\nopic_64.gif");
                //String path3 = Server.MapPath(@"\Img\promoline.gif");

                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "logo";
                logo.ContentType.MediaType = "image/png";

                LinkedResource face = new LinkedResource(path2);
                face.ContentId = "face";
                face.ContentType.MediaType = "image/gif";

                LinkedResource logo2 = new LinkedResource(path3);
                logo2.ContentId = "logo2";
                logo2.ContentType.MediaType = "image/gif";

                string s = "<html>" +
                            "<body style=\" background-color:#F5FFFA;\">" +
                                "<table style=\"height: 200px\">" +
                                         "<tr>" +
                                             "<td style=\"padding-bottom: 85px;\">" +
                                                       "<img  src=\"cid:logo\"/>" +
                                             "</td>" +
                                             "<td>" +
                                                "<center><h2>Alerta Post-Facturaci&oacute;n</h2></center>" +
                                                       "<table>" +
                                                            "<tr>" +
                                                               "<td>" +
                                                                  "<img src=\"cid:face\"/>" +
                                                                "</td>" +
                                                                "<td>" +
                                                                    "<b>Hola</b>" +
                                                                    "<br>" +
                                                                    GetMsj(DocNum) +
                                                           "</td>" +
                                                             "</tr>" +
                                                          "</table>" +
                                                           "</td>" +
                                                           "<td style=\"padding-bottom: 85px;\">" +
                                                                "<img src=\"cid:logo2\"/>" +
                                                           "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                        "<td colspan=3 style=\" font-size: xx-small; color:#D8D8D8;\">" +
                                                            "Enviado el " + DateTime.Now.ToString() +
                                                        "</td>" +
                                                       "</tr>" +
                                                    "</table>" +
                                                    "</body>" +
                                                    "</html>";

                AlternateView av = AlternateView.CreateAlternateViewFromString(s, null, MediaTypeNames.Text.Html);

                av.LinkedResources.Add(logo);
                av.LinkedResources.Add(face);
                av.LinkedResources.Add(logo2);

                msj.AlternateViews.Add(av);
                msj.IsBodyHtml = true;
                msj.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
                client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
                client.Send(msj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[enviarMail]-> " + ex.Message);
            }
        }

        //Credito solicitud de Nota de Credito
        public void enviarMail(string DocNum, string ItemCode, int Cantidad)
        {
            try
            {
                MailMessage msj = new MailMessage();

                msj.From = new MailAddress("test1@eximagen.com.mx");
                msj.To.Add("Todotesoreria@eximagen.com.mx");

                //msj.Bcc.Add("todoproduccion@eximagen.com.mx");

                //msj.Bcc.Add("lmhr200781@yahoo.com.mx");

                msj.Subject = "Aviso Post-facturación";

                String path = @"E:\Img\eximagen.gif";
                String path2 = @"E:\Img\nopic_64.gif";
                String path3 = @"E:\Img\promoline.gif";

                //String path = Path.GetFullPath(@"\PosTFacturaKN\Img\eximagen.gif");
                //String path2 = Path.GetFullPath(@"\PosTFacturaKN\Img\nopic_64.gif");
                //String path3 = Path.GetFullPath(@"\PosTFacturaKN\Img\promoline.gif");

                //String path =  HttpServerUtility.Server.MapPath(@"\Img\eximagen.gif");
                //String path2 = Server.MapPath(@"\Img\nopic_64.gif");
                //String path3 = Server.MapPath(@"\Img\promoline.gif");

                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "logo";
                logo.ContentType.MediaType = "image/png";

                LinkedResource face = new LinkedResource(path2);
                face.ContentId = "face";
                face.ContentType.MediaType = "image/gif";

                LinkedResource logo2 = new LinkedResource(path3);
                logo2.ContentId = "logo2";
                logo2.ContentType.MediaType = "image/gif";

                string s = "<html>" +
                            "<body style=\" background-color:#F5FFFA;\">" +
                                "<table style=\"height: 200px\">" +
                                         "<tr>" +
                                             "<td style=\"padding-bottom: 85px;\">" +
                                                       "<img  src=\"cid:logo\"/>" +
                                             "</td>" +
                                             "<td>" +
                                                "<center><h2>Alerta Post-Facturaci&oacute;n</h2></center>" +
                                                       "<table>" +
                                                            "<tr>" +
                                                               "<td>" +
                                                                  "<img src=\"cid:face\"/>" +
                                                                "</td>" +
                                                                "<td>" +
                                                                    "<b>Hola</b>" +
                                                                    "<br>" +
                                                                    GetMsj(DocNum, ItemCode, Cantidad) +
                                                           "</td>" +
                                                             "</tr>" +
                                                          "</table>" +
                                                           "</td>" +
                                                           "<td style=\"padding-bottom: 85px;\">" +
                                                                "<img src=\"cid:logo2\"/>" +
                                                           "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                        "<td colspan=3 style=\" font-size: xx-small; color:#D8D8D8;\">" +
                                                            "Enviado el " + DateTime.Now.ToString() +
                                                        "</td>" +
                                                       "</tr>" +
                                                    "</table>" +
                                                    "</body>" +
                                                    "</html>";

                AlternateView av = AlternateView.CreateAlternateViewFromString(s, null, MediaTypeNames.Text.Html);

                av.LinkedResources.Add(logo);
                av.LinkedResources.Add(face);
                av.LinkedResources.Add(logo2);

                msj.AlternateViews.Add(av);
                msj.IsBodyHtml = true;
                msj.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
                client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
                client.Send(msj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[enviarMail]-> " + ex.Message);
            }
        }

        //Vendedor Entrega incompleta al final de producción
        public void enviarMail(Int32 FacturasId, Int32 PartidaId, string DocNum, string Item, string correo1, string correo2)
        {
            try
            {
                MailMessage msj = new MailMessage();

                msj.From = new MailAddress("test1@eximagen.com.mx");
                msj.To.Add(correo1);

                if (correo2 != "")
                {
                    msj.CC.Add(correo2);
                }

                //msj.Bcc.Add("todoproduccion@eximagen.com.mx");

                //msj.Bcc.Add("lmhr200781@yahoo.com.mx");

                msj.Subject = "Aviso Post-facturación";

                String path = @"E:\Img\eximagen.gif";
                String path2 = @"E:\Img\nopic_64.gif";
                String path3 = @"E:\Img\promoline.gif";

                //String path = Path.GetFullPath(@"\PosTFacturaKN\Img\eximagen.gif");
                //String path2 = Path.GetFullPath(@"\PosTFacturaKN\Img\nopic_64.gif");
                //String path3 = Path.GetFullPath(@"\PosTFacturaKN\Img\promoline.gif");

                //String path =  HttpServerUtility.Server.MapPath(@"\Img\eximagen.gif");
                //String path2 = Server.MapPath(@"\Img\nopic_64.gif");
                //String path3 = Server.MapPath(@"\Img\promoline.gif");

                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "logo";
                logo.ContentType.MediaType = "image/png";

                LinkedResource face = new LinkedResource(path2);
                face.ContentId = "face";
                face.ContentType.MediaType = "image/gif";

                LinkedResource logo2 = new LinkedResource(path3);
                logo2.ContentId = "logo2";
                logo2.ContentType.MediaType = "image/gif";

                string s = "<html>" +
                            "<body style=\" background-color:#F5FFFA;\">" +
                                "<table style=\"height: 200px\">" +
                                         "<tr>" +
                                             "<td style=\"padding-bottom: 85px;\">" +
                                                       "<img  src=\"cid:logo\"/>" +
                                             "</td>" +
                                             "<td>" +
                                                "<center><h2>Alerta Post-Facturaci&oacute;n</h2></center>" +
                                                       "<table>" +
                                                            "<tr>" +
                                                               "<td>" +
                                                                  "<img src=\"cid:face\"/>" +
                                                                "</td>" +
                                                                "<td>" +
                                                                    "<b>Hola</b>" +
                                                                    "<br>" +
                                                                    GetMsj(DocNum, Item, FacturasId, PartidaId) +
                                                           "</td>" +
                                                             "</tr>" +
                                                          "</table>" +
                                                           "</td>" +
                                                           "<td style=\"padding-bottom: 85px;\">" +
                                                                "<img src=\"cid:logo2\"/>" +
                                                           "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                        "<td colspan=3 style=\" font-size: xx-small; color:#D8D8D8;\">" +
                                                            "Enviado el " + DateTime.Now.ToString() +
                                                        "</td>" +
                                                       "</tr>" +
                                                    "</table>" +
                                                    "</body>" +
                                                    "</html>";

                AlternateView av = AlternateView.CreateAlternateViewFromString(s, null, MediaTypeNames.Text.Html);

                av.LinkedResources.Add(logo);
                av.LinkedResources.Add(face);
                av.LinkedResources.Add(logo2);

                msj.AlternateViews.Add(av);
                msj.IsBodyHtml = true;
                msj.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
                client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
                client.Send(msj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[enviarMail]-> " + ex.Message);
            }
        }

        //Vendedores aviso de entregas incompletas al solicitar producto a KN
        public void enviarMail(Int32 FacturasId, Int32 PartidaId, string DocNum, string Item, string correo1, string correo2, int Flag)
        {
            try
            {
                MailMessage msj = new MailMessage();

                msj.From = new MailAddress("test1@eximagen.com.mx");
                // msj.To.Add(correo1);
                msj.To.Add("marcos.barojas@eximagen.com.mx");

                if (correo2 != "")
                {
                    msj.CC.Add(correo2);
                }


                //msj.Bcc.Add("todoproduccion@eximagen.com.mx");

                //msj.Bcc.Add("lmhr200781@yahoo.com.mx");

                msj.Subject = "Aviso Post-facturación";

                String path = @"E:\Img\eximagen.gif";
                String path2 = @"E:\Img\nopic_64.gif";
                String path3 = @"E:\Img\promoline.gif";

                //String path = Path.GetFullPath(@"\PosTFacturaKN\Img\eximagen.gif");
                //String path2 = Path.GetFullPath(@"\PosTFacturaKN\Img\nopic_64.gif");
                //String path3 = Path.GetFullPath(@"\PosTFacturaKN\Img\promoline.gif");

                //String path =  HttpServerUtility.Server.MapPath(@"\Img\eximagen.gif");
                //String path2 = Server.MapPath(@"\Img\nopic_64.gif");
                //String path3 = Server.MapPath(@"\Img\promoline.gif");

                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "logo";
                logo.ContentType.MediaType = "image/png";

                LinkedResource face = new LinkedResource(path2);
                face.ContentId = "face";
                face.ContentType.MediaType = "image/gif";

                LinkedResource logo2 = new LinkedResource(path3);
                logo2.ContentId = "logo2";
                logo2.ContentType.MediaType = "image/gif";

                string s = "<html>" +
                            "<body style=\" background-color:#F5FFFA;\">" +
                                "<table style=\"height: 200px\">" +
                                         "<tr>" +
                                             "<td style=\"padding-bottom: 85px;\">" +
                                                       "<img  src=\"cid:logo\"/>" +
                                             "</td>" +
                                             "<td>" +
                                                "<center><h2>Alerta Post-Facturaci&oacute;n</h2></center>" +
                                                       "<table>" +
                                                            "<tr>" +
                                                               "<td>" +
                                                                  "<img src=\"cid:face\"/>" +
                                                                "</td>" +
                                                                "<td>" +
                                                                    "<b>Hola</b>" +
                                                                    "<br>" +
                                                                    GetMsj(Flag, DocNum, Item, FacturasId, PartidaId) +
                                                           "</td>" +
                                                             "</tr>" +
                                                          "</table>" +
                                                           "</td>" +
                                                           "<td style=\"padding-bottom: 85px;\">" +
                                                                "<img src=\"cid:logo2\"/>" +
                                                           "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                        "<td colspan=3 style=\" font-size: xx-small; color:#D8D8D8;\">" +
                                                            "Enviado el " + DateTime.Now.ToString() +
                                                        "</td>" +
                                                       "</tr>" +
                                                    "</table>" +
                                                    "</body>" +
                                                    "</html>";

                AlternateView av = AlternateView.CreateAlternateViewFromString(s, null, MediaTypeNames.Text.Html);

                av.LinkedResources.Add(logo);
                av.LinkedResources.Add(face);
                av.LinkedResources.Add(logo2);

                msj.AlternateViews.Add(av);
                msj.IsBodyHtml = true;
                msj.Priority = System.Net.Mail.MailPriority.High;

                SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
                client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
                client.Send(msj);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [Alertas].[enviarMail]-> " + ex.Message);
            }
        }

        //public void enviarMail(Int32 FacturasId, Int32 PartidaId, string DocNum, string Item, string correo1, string correo2, int Flag)
        //{
        //    try
        //    {
        //        MailMessage msj = new MailMessage();

        //        msj.From = new MailAddress("test1@eximagen.com.mx");
        //        msj.To.Add(correo1);

        //        if (correo2 != "")
        //        {
        //            msj.CC.Add(correo2);
        //        }

        //        string msjAlerta;

        //        if (Flag == 1)
        //        {
        //            msjAlerta = "Posible entrega incompleta para el producto " + Item + " con número de factura <b>" + DocNum + "</b>.";
        //            msj.CC.Add("todoproduccion@eximagen.com.mx");
        //        }
        //        else {
        //            msjAlerta = "Entrega incompleta para el producto " + Item + " con número de factura <b>" + DocNum + "</b>.";
        //        }

        //        //msj.Bcc.Add("todoproduccion@eximagen.com.mx");

        //        //msj.Bcc.Add("lmhr200781@yahoo.com.mx");

        //        msj.Subject = "Aviso Post-facturación";

        //        String path = @"E:\Img\eximagen.gif";
        //        String path2 = @"E:\Img\nopic_64.gif";
        //        String path3 = @"E:\Img\promoline.gif";

        //        //String path = Path.GetFullPath(@"\PosTFacturaKN\Img\eximagen.gif");
        //        //String path2 = Path.GetFullPath(@"\PosTFacturaKN\Img\nopic_64.gif");
        //        //String path3 = Path.GetFullPath(@"\PosTFacturaKN\Img\promoline.gif");

        //        //String path =  HttpServerUtility.Server.MapPath(@"\Img\eximagen.gif");
        //        //String path2 = Server.MapPath(@"\Img\nopic_64.gif");
        //        //String path3 = Server.MapPath(@"\Img\promoline.gif");

        //        LinkedResource logo = new LinkedResource(path);
        //        logo.ContentId = "logo";
        //        logo.ContentType.MediaType = "image/png";

        //        LinkedResource face = new LinkedResource(path2);
        //        face.ContentId = "face";
        //        face.ContentType.MediaType = "image/gif";

        //        LinkedResource logo2 = new LinkedResource(path3);
        //        logo2.ContentId = "logo2";
        //        logo2.ContentType.MediaType = "image/gif";

        //        string s = "<html>" +
        //                    "<body style=\" background-color:#F5FFFA;\">" +
        //                        "<table style=\"height: 200px\">" +
        //                                 "<tr>" +
        //                                     "<td style=\"padding-bottom: 85px;\">" +
        //                                               "<img  src=\"cid:logo\"/>" +
        //                                     "</td>" +
        //                                     "<td>" +
        //                                        "<center><h2>Alerta Post-Facturaci&oacute;n</h2></center>" +
        //                                               "<table>" +
        //                                                    "<tr>" +
        //                                                       "<td>" +
        //                                                          "<img src=\"cid:face\"/>" +
        //                                                        "</td>" +
        //                                                        "<td>" +
        //                                                            "<b>Hola</b>" +
        //                                                            "<br>" +
        //                                                             msjAlerta;

        //                                                             if (Flag != 1 ){
        //                                                                s += "<table>" +
        //                                                                    "<tr>" +
        //                                                                       "<td colspan=3>" +
        //                                                                            "¿Cliente acepta entrega incompleta del producto?" +
        //                                                                       "<td>" +
        //                                                                    "</tr>" +
        //                                                                    "<tr>" +
        //                                                                      "<td>" +
        //                                                                        "<center>" +
        //                                                                         "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://localhost:2411/PosTFacturaKN/ValidarEntregaParcial.aspx?Val1=Si&Val2=" + FacturasId.ToString() + "&Val3=" + PartidaId.ToString() + ">Si</a></p>" +
        //                                                                        "</center>" +
        //                                                                      "</td>" +
        //                                                                      "<td>" +
        //                                                                      "</td>" +
        //                                                                      "<td>" +
        //                                                                         "<center>" +
        //                                                                            "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://localhost:2411/PosTFacturaKN/ValidarEntregaParcial.aspx?Val1=No&Val2=" + FacturasId.ToString() + "&Val3=" + PartidaId.ToString() + ">No</a></p>" +
        //                                                                         "</center>" +
        //                                                                      "</td>" +
        //                                                                    "</tr>" +
        //                                                                "</table>";
        //                                                            }

        //                                                  s += "</td>" +
        //                                                     "</tr>" +
        //                                                  "</table>" +
        //                                                   "</td>" +
        //                                                   "<td style=\"padding-bottom: 85px;\">" +
        //                                                        "<img src=\"cid:logo2\"/>" +
        //                                                   "</td>" +
        //                                               "</tr>" +
        //                                               "<tr>" +
        //                                                "<td colspan=3 style=\" font-size: xx-small; color:#D8D8D8;\">" +
        //                                                    "Enviado el " + DateTime.Now.ToString() +
        //                                                "</td>" +
        //                                               "</tr>" +
        //                                            "</table>" +
        //                                            "</body>" +
        //                                            "</html>";

        //        AlternateView av = AlternateView.CreateAlternateViewFromString(s, null, MediaTypeNames.Text.Html);

        //        av.LinkedResources.Add(logo);
        //        av.LinkedResources.Add(face);
        //        av.LinkedResources.Add(logo2);

        //        msj.AlternateViews.Add(av);
        //        msj.IsBodyHtml = true;
        //        msj.Priority = System.Net.Mail.MailPriority.High;

        //        SmtpClient client = new SmtpClient("pop.eximagen.com.mx", 26);
        //        client.Credentials = new System.Net.NetworkCredential("test1@eximagen.com.mx", "1soportec$4");
        //        client.Send(msj);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error: [Alertas].[enviarMail]-> " + ex.Message);
        //    }
        //}

        private string GetMsj(string DocNum)
        {
            string msjAlerta = "";

            msjAlerta = "La Factura:<b>" + DocNum + "</b> esta lista para embarcarse pero es necesario analizar el crédito del cliente para autorizarla manualmente.";

            return msjAlerta;
        }

        private string GetMsj(string DocNum, string Item, Int32 FacturasId, Int32 PartidaId)
        {

            string msjAlerta = "";

            msjAlerta = "Entrega incompleta para el producto " + Item + " con número de factura <b>" + DocNum + "</b>.";
            msjAlerta += "<table>" +
                        "<tr>" +
                           "<td colspan=3>" +
                                "¿Decisión tomada por el cliente?" +
                           "<td>" +
                        "</tr>" +
                        "<tr>" +
                          "<td>" +
                            "<center>" +
                             "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://localhost:2411/PosTFacturaKN/ProdReembSust.aspx?Val1=Reembolso&Val2=" + FacturasId.ToString() + "&Val3=" + PartidaId.ToString() + ">Reembolso</a></p>" +
                            "</center>" +
                          "</td>" +
                          "<td>" +
                          "</td>" +
                          "<td>" +
                             "<center>" +
                                "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://localhost:2411/PosTFacturaKN/ProdReembSust.aspx?Val1=Sustitucion&Val2=" + FacturasId.ToString() + "&Val3=" + PartidaId.ToString() + ">Sustituci&oacute;n del producto</a></p>" +
                             "</center>" +
                          "</td>" +
                        "</tr>" +
                    "</table>";

            return msjAlerta;
        }

        private string GetMsj(int Flag, string DocNum, string Item, Int32 FacturasId, Int32 PartidaId)
        {
            string msjAlerta = "";

            if (Flag == 1)
            {

                TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 });


                QueryString["Val1"] = "Si";
                QueryString["Val2"] = FacturasId.ToString();
                QueryString["Val3"] = PartidaId.ToString();



                TSHAK.Components.SecureQueryString QueryStringNo = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 });


                QueryStringNo["Val1"] = "No";
                QueryStringNo["Val2"] = FacturasId.ToString();
                QueryStringNo["Val3"] = PartidaId.ToString();



                msjAlerta = "Posible entrega incompleta para el producto " + Item + " con número de factura <b>" + DocNum + "</b>.";
                msjAlerta += "<table>" +
                        "<tr>" +
                          "<td colspan=3>" +
                            "¿Cliente acepta entrega incompleta del producto?" +
                          "<td>" +
                        "</tr>" +
                        "<tr>" +
                          "<td>" +
                            "<center>" +
                              "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://192.168.0.15/posfack/ValidarEntregaParcial.aspx?data=" + HttpUtility.UrlEncode(QueryString.ToString()) + ">Si</a></p>" +
                            "</center>" +
                          "</td>" +
                          "<td>" +
                          "</td>" +
                          "<td>" +
                            "<center>" +
                              "<p><a style=\"color: Blue; font-size: 14px; font-weight: bold; font-family: MS Sans Serif;\" href=http://192.168.0.15/posfack/ValidarEntregaParcial.aspx?data=" + HttpUtility.UrlEncode(QueryStringNo.ToString()) + ">No</a></p>" +
                            "</center>" +
                          "</td>" +
                       "</tr>" +
                     "</table>";
            }
            else
            {
                msjAlerta = "Entrega incompleta para el producto " + Item + " con número de factura <b>" + DocNum + "</b>.";
            }

            return msjAlerta;
        }

        private string GetMsj(string DocNum, string ItemCode, int Cantidad)
        {
            string msjAlerta = "";

            msjAlerta = "Favor de realizar una nota de crédito para la factura " + DocNum + " para el producto " + ItemCode + " por la cantidad de " + Cantidad.ToString() + ".";

            return msjAlerta;
        }
    }
}

