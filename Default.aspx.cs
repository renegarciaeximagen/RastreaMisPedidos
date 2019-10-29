using System;
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
using System.Text;
using System.Security.Cryptography;
using System.IO;
using NeoSmart.Utils;


public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if (!IsPostBack)
            {


                string hash = Request.QueryString["hash"].ToString();
                byte[] bytesToBeDecrypted = UrlBase64.Decode(hash);
                byte[] passwordBytesDec = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Pass"].ToString());
                passwordBytesDec = SHA256.Create().ComputeHash(passwordBytesDec);

                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytesDec);

                string IDCliente = Encoding.UTF8.GetString(bytesDecrypted);

                //DataSet DatadataSet = new DataSet();
                //SeccionMySql objSecpromo = new SeccionMySql();

                //string customer_id = string.Empty;
                //string NumCliente = string.Empty;


                Session["hash"] = hash;
                //objSecpromo.Logineshop =  Request.QueryString["ww"].ToString();
                //DatadataSet = objSecpromo.SeccionID();
                //DataTable myDataTable = DatadataSet.Tables[0];
                //foreach (DataRow ColumSeccion in myDataTable.Rows)
                //{
                string customer_id = IDCliente;
                EXIMSAPDataContext dbsap = new EXIMSAPDataContext();
                OCRD cliente = (from a in dbsap.OCRD
                                where a.CardCode == IDCliente
                                select a).FirstOrDefault();

                    
                //}

                if (customer_id != "0" && customer_id != "")
                {
                    cargaDatos(IDCliente);
                    lblSeccion.Text = "Si";
                    lblNombreAdmin.Text = IDCliente + " - " + cliente.CardName;
                }
                else
                {
                    lblSeccion.Text = "No";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Seccion.aspx");
        }
        finally
        {

            if (lblSeccion.Text == "No" || lblSeccion.Text.Length ==0)
            {
                Response.Redirect("Seccion.aspx");
            }
        }

    }

    private void cargaDatos(string NumCliente)
    {
        try
        {
            Facturas obj_Select = new Facturas();
            obj_Select.Facturas_CardCode = NumCliente;
            DataTable dt = obj_Select.SelectOne15Dias();

            if (dt.Rows.Count > 0)
            {
                grid_Rastreo.DataSource = dt;
                grid_Rastreo.DataBind();
            }

 

        }
        catch (Exception ex)
        {
            Response.Redirect("Seccion.aspx");
        }

    }
    protected void grid_Rastreo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Consulta")
        {
            int index = Convert.ToInt32(e.CommandArgument);


            GridViewRow row = grid_Rastreo.Rows[index];
            Label lbl_FacturasId = (Label)row.FindControl("lbl_FacturasId");
  



            //TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 });

            //QueryString["id"] = lbl_FacturasId.Text;
            Response.Redirect("http://www2.promoshop.com.mx/postfact2prod/Resumen.aspx?id=" + lbl_FacturasId.Text);




        }
    }
    public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        byte[] decryptedBytes = null;
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.Close();
                }
                decryptedBytes = ms.ToArray();
            }
        }

        return decryptedBytes;
    }
}
