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

namespace Componente.Procesos
{
    /// <summary>
    /// Descripción breve de LogTransporte
    /// </summary>
    public class LogTransporte : DBInteractionBase
    {
        public LogTransporte()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        #region Propiedades
        private String _numTrasporte, _numPlacas, _strSQL;

        private SqlInt32 _SelloKN, _cantBultos, _facturaId, _logTransId, _facturaIdContruc;

        public SqlInt32 FacturaIdContruc
        {
            get { return _facturaIdContruc; }
            set { _facturaIdContruc = value; }
        }

        public String NumTransporte
        {
            get
            {
                return _numTrasporte;
            }
            set
            {
                _numTrasporte = (String)value;
            }
        }

        public String NumPlacas
        {
            get
            {
                return _numPlacas;
            }
            set
            {
                _numPlacas = (String)value;
            }
        }

        public SqlInt32 SelloKN
        {
            get
            {
                return _SelloKN;
            }
            set
            {
                _SelloKN = (SqlInt32)value;
            }
        }

        public SqlInt32 CantBultos
        {
            get
            {
                return _cantBultos;
            }
            set
            {
                _cantBultos = (SqlInt32)value;
            }
        }

        public SqlInt32 HojaViaje
        {
            get
            {
                return _facturaId;
            }
            set
            {
                _facturaId = (SqlInt32)value;
            }
        }
        #endregion


        #region Metodos

        public override bool Insert()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = _strSQL;
            cmdToExecute.CommandType = CommandType.Text;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
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
                _rowsAffected = cmdToExecute.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("LogTransporte::Insert::Error occured.", ex);
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

        public void strSqlCommand(DataTable dtHojasViaje)
        {
           string strSello = (!SelloKN.IsNull)?SelloKN.ToString():"NULL";

            _strSQL = "DECLARE @UltLogTransId INT \n";
            _strSQL += "INSERT INTO dbo.LogTransporte(NumTransporte,Placas,SelloKN,CantBultos,Fecha) \n" +
                       "VALUES('" + NumTransporte.ToString() + "','" + NumPlacas + "'," + strSello + "," + CantBultos.ToString() + ",GETDATE()) \n";

            _strSQL += "SELECT @UltLogTransId = MAX(LogTransId) FROM dbo.LogTransporte \n";

            foreach (DataRow dr in dtHojasViaje.Rows)
            {
                _strSQL += "INSERT INTO dbo.HojaViajeTransporteRel (FacturasId,LogTransId) \n" +
                           "VALUES (" + dr["FacaturasId"].ToString() + ",@UltLogTransId) \n";
            }
        }
        public override DataTable SelectOne()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_post_TransPorteSelec]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("LogTransporte");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@iFacturasId", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _facturaIdContruc));
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
                    throw new Exception("Stored Procedure 'sp_post_Facturas_SelectOne' reported the ErrorCode: " + _errorCode);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("Facturas::SelectOne::Error occured.", ex);
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
        #endregion
    }
}