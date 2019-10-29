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
using Componente.Conexion;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Componente.Procesos
{
    /// <summary>
    /// Purpose: Data Access class for the table 'Buffer'.
    /// </summary>
    /// 
    public class MenuPosfack : DBInteractionBase
    {
        #region Class Member Declarations
        private SqlInt32 _usuarioId;

 
        private SqlMoney _bufferPorcen;
        private SqlString _bufferFamilia;
        #endregion


        /// <summary>
        /// Purpose: Class constructor.
        /// </summary>
        public MenuPosfack()
        {
            // Nothing for now.
        }




        /// <summary>
        /// Purpose: Select method. This method will Select one existing row from the database, based on the Primary Key.
        /// </summary>
        /// <returns>DataTable object if succeeded, otherwise an Exception is thrown. </returns>
        /// <remarks>
        /// Properties needed for this method: 
        /// <UL>
        ///		 <LI>BufferId</LI>
        /// </UL>
        /// Properties set after a succesful call of this method: 
        /// <UL>
        ///		 <LI>ErrorCode</LI>
        ///		 <LI>BufferId</LI>
        ///		 <LI>BufferFamilia</LI>
        ///		 <LI>BufferPorcen</LI>
        /// </UL>
        /// Will fill all properties corresponding with a field in the table with the value of the row selected.
        /// </remarks>
        public override DataTable SelectOne()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[ObtenerOpcionesMenu]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Menu");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@iUsuarioId", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _usuarioId));
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
                    throw new Exception("Stored Procedure 'sp_post_Buffer_SelectOne' reported the ErrorCode: " + _errorCode);
                }
                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("Buffer::SelectOne::Error occured.", ex);
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

       #region Class Property Declarations
 
        public SqlInt32 UsuarioId
        {
            get { return _usuarioId; }
            set { _usuarioId = value; }
        }

        public SqlString BufferFamilia
        {
            get
            {
                return _bufferFamilia;
            }
            set
            {
                SqlString bufferFamiliaTmp = (SqlString)value;
                if (bufferFamiliaTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("BufferFamilia", "BufferFamilia can't be NULL");
                }
                _bufferFamilia = value;
            }
        }


        public SqlMoney BufferPorcen
        {
            get
            {
                return _bufferPorcen;
            }
            set
            {
                SqlMoney bufferPorcenTmp = (SqlMoney)value;
                if (bufferPorcenTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("BufferPorcen", "BufferPorcen can't be NULL");
                }
                _bufferPorcen = value;
            }
        }
        #endregion
    }
}

