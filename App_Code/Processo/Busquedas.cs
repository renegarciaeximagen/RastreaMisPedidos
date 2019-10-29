using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Componente.Conexion;
/// <summary>
/// Descripción breve de Busquedas
/// </summary>
namespace Componente.Procesos
{
    public class Busquedas : DBInteractionBase
    {
        #region Class Member Declarations

        private SqlString _facturasId, _facturas_DocEntry, _facturas_CardCode, _vendedor, _filtro;

        #endregion


        #region Luis

            #region Propiedades de Clase
            private SqlString _hojaViaje, _pedido, _factSAP, _factElect, _numCli, _ProdSiNo, _estatus;

                public SqlString HojaViaje
                {
                    get
                    {
                        return _hojaViaje;
                    }
                    set
                    {
                        //SqlString hojaViaje = (SqlString)value;
                        //if (hojaViaje.IsNull)
                        //{
                        //    throw new ArgumentOutOfRangeException("HojaViaje. HojaViaje no puede ser NULL.");
                        //}
                        _hojaViaje = value;
                    }
                }

                public SqlString Pedido
                {
                    get
                    {
                        return _pedido;
                    }
                    set
                    {

                        _pedido = value;
                    }
                }

                public SqlString FactSAP
                {
                    get
                    {
                        return _factSAP;
                    }
                    set
                    {
                        //SqlString factSAP = (SqlString)value;
                        //if (factSAP.IsNull)
                        //{
                        //    throw new ArgumentOutOfRangeException("FactSAP. FactSAP no puede ser NULL.");
                        //}
                        _factSAP = value;
                    }
                }

                public SqlString FactElect
                {
                    get
                    {
                        return _factElect;
                    }
                    set
                    {
                        //SqlString factElect = (SqlString)value;
                        //if (factElect.IsNull)
                        //{
                        //    throw new ArgumentOutOfRangeException("FactElect. FactElect no puede ser NULL.");
                        //}
                        _factElect = value;
                    }
                }

                public SqlString NumCli
                {
                    get
                    {
                        return _numCli;
                    }
                    set
                    {
                        //SqlString numCli = (SqlString)value;
                        //if (numCli.IsNull)
                        //{
                        //    throw new ArgumentOutOfRangeException("NumCli. NumCli no puede ser NULL.");
                        //}
                        _numCli = value;
                    }
                }

                public SqlString ProduccionSiNo {
                    get {
                        return _ProdSiNo;
                    }
                    set {
                        //SqlString Prod = (SqlString)value;
                        //if (Prod.IsNull)
                        //{
                        //    throw new ArgumentOutOfRangeException("Busquedas. ProduccionSiNo no puede ser NULL");
                        //}
                        _ProdSiNo = value;

                    }
                }

                public SqlString Estatus {
                    get {
                        return _estatus;
                    }
                    set {
                        //SqlString estatus = (SqlString)value;
                        //if (estatus.IsNull) {
                        //    throw new ArgumentOutOfRangeException("Busquedas. Estatus no puede ser NULL");
                        //}
                        _estatus = value; 
                    }
                }
            #endregion
            
            #region Metodos

                public Busquedas(String prmEstatus, String prmProduccionSiNo, String prmHojaViaje, String prmFacturaSAP, String prmFactElect, String prmNumCli, String prmVendedor, String prmPedido) {
                    Estatus = prmEstatus;
                    ProduccionSiNo = prmProduccionSiNo;
                    HojaViaje = prmHojaViaje;
                    FactSAP = prmFacturaSAP;
                    FactElect = prmFactElect;
                    NumCli = prmNumCli;
                    Vendedor = prmVendedor;
                    Pedido = prmPedido; 
                }

                public DataTable busquedaPopup()
                {
                    SqlCommand cmdToExecute = new SqlCommand();
                    cmdToExecute.CommandText = "dbo.[spBusquedaPopUp]";
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    DataTable toReturn = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

                    // Use base class' connection object
                    cmdToExecute.Connection = _mainConnection;

                    try
                    {
                        SqlParameter prm;

                        prm = new SqlParameter("@HojaViaje", SqlDbType.VarChar);
                        prm.Value = HojaViaje;
                        cmdToExecute.Parameters.Add(prm);
                        prm = null;

                        prm = new SqlParameter("@Pedido", SqlDbType.VarChar);
                        prm.Value = Pedido;
                        cmdToExecute.Parameters.Add(prm);
                        prm = null;

                        prm = new SqlParameter("@FactSAP", SqlDbType.VarChar);
                        prm.Value = FactSAP;
                        cmdToExecute.Parameters.Add(prm);
                        prm = null;

                        prm = new SqlParameter("@FactElect", SqlDbType.VarChar);
                        prm.Value = FactElect;
                        cmdToExecute.Parameters.Add(prm);
                        prm = null;

                        prm = new SqlParameter("@NumCli", SqlDbType.VarChar);
                        prm.Value = NumCli;
                        cmdToExecute.Parameters.Add(prm);
                        prm = null;

                        cmdToExecute.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _errorCode));

                        if (_mainConnectionIsCreatedLocal)
                        {
                            // Open connection.
                            _mainConnection.Open();
                        }
                        else
                        {
                            if (_mainConnectionProvider.IsTransactionPending)
                            {
                                cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                            }
                        }

                        // Execute query.
                        adapter.Fill(toReturn);
                        _errorCode = (SqlInt32)cmdToExecute.Parameters["@iErrorCode"].Value;

                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            throw new Exception("Stored Procedure 'spBusquedaPopUp' reported the ErrorCode: " + _errorCode);
                        }
                        return toReturn;
                    }
                    catch (Exception ex)
                    {
                        // some error occured. Bubble it to caller and encapsulate Exception object
                        throw new Exception("Busquedas::busquedaPopup::Error occured.", ex);
                    }
                    finally
                    {
                        if (_mainConnectionIsCreatedLocal)
                        {
                            // Close connection.
                            _mainConnection.Close();
                        }
                        cmdToExecute.Dispose();
                        adapter.Dispose();
                    }
                }

                public DataTable busquedaAvanzada(string strTableName)
                {
                    SqlCommand cmdToExecute = new SqlCommand();
                    cmdToExecute.CommandText = "dbo.[spFiltrosPrinPostFact]";
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    DataTable toReturn = new DataTable(strTableName);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

                    // Use base class' connection object
                    cmdToExecute.Connection = _mainConnection;

                    try
                    {
                        cmdToExecute.Parameters.Add(new SqlParameter("@iFacturasId", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, HojaViaje));
                        cmdToExecute.Parameters.Add(new SqlParameter("@iFacturas_DocNum", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FactSAP));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sFacturas_CardCode", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, NumCli));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sPedido", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Pedido));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sVendedor", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Vendedor));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sFiltro", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Estatus));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sFactElect", SqlDbType.NVarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FactElect));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sProdSiNo", SqlDbType.NVarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ProduccionSiNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _errorCode));

                        if (_mainConnectionIsCreatedLocal)
                        {
                            // Open connection.
                            _mainConnection.Open();
                        }
                        else
                        {
                            if (_mainConnectionProvider.IsTransactionPending)
                            {
                                cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                            }
                        }

                        // Execute query.
                        adapter.Fill(toReturn);
                        _errorCode = (SqlInt32)cmdToExecute.Parameters["@iErrorCode"].Value;

                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            throw new Exception("Stored Procedure 'spFiltrosPrinPostFact' reported the ErrorCode: " + _errorCode);
                        }

                        return toReturn;
                    }
                    catch (Exception ex)
                    {
                        // some error occured. Bubble it to caller and encapsulate Exception object
                        throw new Exception("Facturas::FiltrosMesaControl::Error occured.", ex);
                    }
                    finally
                    {
                        if (_mainConnectionIsCreatedLocal)
                        {
                            // Close connection.
                            _mainConnection.Close();
                        }
                        cmdToExecute.Dispose();
                    }
                }

                public DataTable filtrosEmbarques(string strTableName)
                {
                    SqlCommand cmdToExecute = new SqlCommand();
                    cmdToExecute.CommandText = "dbo.[spFiltrosEmbarques]";
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    DataTable toReturn = new DataTable(strTableName);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

                    // Use base class' connection object
                    cmdToExecute.Connection = _mainConnection;

                    try
                    {
                        cmdToExecute.Parameters.Add(new SqlParameter("@iFacturasId", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, HojaViaje));
                        cmdToExecute.Parameters.Add(new SqlParameter("@iFacturas_DocNum", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FactSAP));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sFacturas_CardCode", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, NumCli));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sPedido", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Pedido));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sVendedor", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Vendedor));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sFiltro", SqlDbType.NVarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Estatus));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sFactElect", SqlDbType.NVarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FactElect));
                        cmdToExecute.Parameters.Add(new SqlParameter("@sProdSiNo", SqlDbType.NVarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ProduccionSiNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _errorCode));

                        if (_mainConnectionIsCreatedLocal)
                        {
                            // Open connection.
                            _mainConnection.Open();
                        }
                        else
                        {
                            if (_mainConnectionProvider.IsTransactionPending)
                            {
                                cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                            }
                        }

                        // Execute query.
                        adapter.Fill(toReturn);
                        _errorCode = (SqlInt32)cmdToExecute.Parameters["@iErrorCode"].Value;

                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            throw new Exception("Stored Procedure 'spFiltrosEmbarques' reported the ErrorCode: " + _errorCode);
                        }

                        return toReturn;
                    }
                    catch (Exception ex)
                    {
                        // some error occured. Bubble it to caller and encapsulate Exception object
                        throw new Exception("Facturas::filtrosEmbarques::Error occured.", ex);
                    }
                    finally
                    {
                        if (_mainConnectionIsCreatedLocal)
                        {
                            // Close connection.
                            _mainConnection.Close();
                        }
                        cmdToExecute.Dispose();
                    }
                }
           #endregion

        #endregion

        public Busquedas()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public DataTable FiltrosMesaControl()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_post_Facturas_SelectEstatusOK]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Facturas");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {

                cmdToExecute.Parameters.Add(new SqlParameter("@iFacturasId", SqlDbType.NVarChar, 15, ParameterDirection.Input,false, 0, 0, "", DataRowVersion.Proposed, _facturasId));
                cmdToExecute.Parameters.Add(new SqlParameter("@iFacturas_DocEntry", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _facturas_DocEntry));
                cmdToExecute.Parameters.Add(new SqlParameter("@sFacturas_CardCode", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _facturas_CardCode));
                cmdToExecute.Parameters.Add(new SqlParameter("@sPedido", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _pedido));
                cmdToExecute.Parameters.Add(new SqlParameter("@sVendedor", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _vendedor));
                cmdToExecute.Parameters.Add(new SqlParameter("@sFiltro", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _filtro));
                cmdToExecute.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _errorCode));

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                // Execute query.
                adapter.Fill(toReturn);
                _errorCode = (SqlInt32)cmdToExecute.Parameters["@iErrorCode"].Value;

                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_post_Facturas_SelectEstatusOK' reported the ErrorCode: " + _errorCode);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("Facturas::Insert::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
            }
        }

        #region Class Property Declarations
        public SqlString Filtro
        {
            get { return _filtro; }
            set { _filtro = value; }
        }

        public SqlString Vendedor
        {
            get { return _vendedor; }
            set { _vendedor = value; }
        }


        public SqlString Facturas_CardCode
        {
            get { return _facturas_CardCode; }
            set { _facturas_CardCode = value; }
        }

        public SqlString Facturas_DocEntry
        {
            get { return _facturas_DocEntry; }
            set { _facturas_DocEntry = value; }
        }

        public SqlString FacturasId
        {
            get { return _facturasId; }
            set { _facturasId = value; }
        }
        #endregion
    }
}