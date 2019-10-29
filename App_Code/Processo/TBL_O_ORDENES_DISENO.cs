///////////////////////////////////////////////////////////////////////////
// Description: Data Access class for the table 'TBL_O_ORDENES_DISENO'
// Generated by LLBLGen v1.21.2003.712 Final on: Miércoles, 15 de Julio de 2009, 05:15:41 p.m.
// Because the Base Class already implements IDispose, this class doesn't.
///////////////////////////////////////////////////////////////////////////
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Componente.Conexion;

namespace Componente.Procesos
{
    /// <summary>
    /// Purpose: Data Access class for the table 'TBL_O_ORDENES_DISENO'.
    /// </summary>
    public class TBL_O_ORDENES_DISENO : DBInteractionBase
    {
        #region Class Member Declarations
        private SqlInt32 _id_Mov_Historial, _id_Log, _id_od, _int_Estatus;
        private SqlString _chr_Ruta_Arch_Final, _chr_Id_Cliente, _chr_Clave_Madre_Produc, _chr_Colores;
        #endregion


        /// <summary>
        /// Purpose: Class constructor.
        /// </summary>
        public TBL_O_ORDENES_DISENO()
        {
            // Nothing for now.
        }


        /// <summary>
        /// Purpose: Insert method. This method will insert one new row into the database.
        /// </summary>
        /// <returns>True if succeeded, otherwise an Exception is thrown. </returns>
        /// <remarks>
        /// Properties needed for this method: 
        /// <UL>
        ///		 <LI>Int_Estatus</LI>
        ///		 <LI>Chr_Id_Cliente</LI>
        ///		 <LI>Chr_Clave_Madre_Produc</LI>
        ///		 <LI>Chr_Colores</LI>
        ///		 <LI>Id_Mov_Historial. May be SqlInt32.Null</LI>
        ///		 <LI>Id_Log. May be SqlInt32.Null</LI>
        ///		 <LI>Chr_Ruta_Arch_Final. May be SqlString.Null</LI>
        /// </UL>
        /// Properties set after a succesful call of this method: 
        /// <UL>
        ///		 <LI>Id_od</LI>
        ///		 <LI>ErrorCode</LI>
        /// </UL>
        /// </remarks>
        public override bool Insert()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_NTBL_O_ORDENES_DISENO_Insert]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@iint_Estatus", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _int_Estatus));
                cmdToExecute.Parameters.Add(new SqlParameter("@schr_Id_Cliente", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _chr_Id_Cliente));
                cmdToExecute.Parameters.Add(new SqlParameter("@schr_Clave_Madre_Produc", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _chr_Clave_Madre_Produc));
                cmdToExecute.Parameters.Add(new SqlParameter("@schr_Colores", SqlDbType.NVarChar, 250, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _chr_Colores));
                cmdToExecute.Parameters.Add(new SqlParameter("@iid_Mov_Historial", SqlDbType.Int, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _id_Mov_Historial));
                cmdToExecute.Parameters.Add(new SqlParameter("@iid_Log", SqlDbType.Int, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _id_Log));
                cmdToExecute.Parameters.Add(new SqlParameter("@schr_Ruta_Arch_Final", SqlDbType.NVarChar, 300, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _chr_Ruta_Arch_Final));
                cmdToExecute.Parameters.Add(new SqlParameter("@iid_od", SqlDbType.Int, 4, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _id_od));
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
                _rowsAffected = cmdToExecute.ExecuteNonQuery();
                _id_od = (Int32)cmdToExecute.Parameters["@iid_od"].Value;
                _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_NTBL_O_ORDENES_DISENO_Insert' reported the ErrorCode: " + _errorCode);
                }

                return true;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TBL_O_ORDENES_DISENO::Insert::Error occured.", ex);
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


        /// <summary>
        /// Purpose: Select method for a unique field. This method will Select one row from the database, based on the unique field 'id_od'
        /// </summary>
        /// <returns>DataTable object if succeeded, otherwise an Exception is thrown. </returns>
        /// <remarks>
        /// Properties needed for this method: 
        /// <UL>
        ///		 <LI>Id_od</LI>
        /// </UL>
        /// Properties set after a succesful call of this method: 
        /// <UL>
        ///		 <LI>ErrorCode</LI>
        ///		 <LI>Id_od</LI>
        ///		 <LI>Int_Estatus</LI>
        ///		 <LI>Chr_Id_Cliente</LI>
        ///		 <LI>Chr_Clave_Madre_Produc</LI>
        ///		 <LI>Chr_Colores</LI>
        ///		 <LI>Id_Mov_Historial</LI>
        ///		 <LI>Id_Log</LI>
        ///		 <LI>Chr_Ruta_Arch_Final</LI>
        /// </UL>
        /// Will fill all properties corresponding with a field in the table with the value of the row selected.
        /// </remarks>
        public DataTable SelectOneWid_odLogic()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_NTBL_O_ORDENES_DISENO_SelectOneWid_odLogic]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("TBL_O_ORDENES_DISENO");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@iid_od", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _id_od));
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
                _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_NTBL_O_ORDENES_DISENO_SelectOneWid_odLogic' reported the ErrorCode: " + _errorCode);
                }

                if (toReturn.Rows.Count > 0)
                {
                    _id_od = (Int32)toReturn.Rows[0]["id_od"];
                    _int_Estatus = (Int32)toReturn.Rows[0]["int_Estatus"];
                    _chr_Id_Cliente = (string)toReturn.Rows[0]["chr_Id_Cliente"];
                    _chr_Clave_Madre_Produc = (string)toReturn.Rows[0]["chr_Clave_Madre_Produc"];
                    _chr_Colores = (string)toReturn.Rows[0]["chr_Colores"];
                    _id_Mov_Historial = toReturn.Rows[0]["id_Mov_Historial"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["id_Mov_Historial"];
                    _id_Log = toReturn.Rows[0]["id_Log"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["id_Log"];
                    _chr_Ruta_Arch_Final = toReturn.Rows[0]["chr_Ruta_Arch_Final"] == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["chr_Ruta_Arch_Final"];
                }
                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TBL_O_ORDENES_DISENO::SelectOneWid_odLogic::Error occured.", ex);
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

        
        /// <summary>
        /// Purpose: SelectAll method. This method will Select all rows from the table.
        /// </summary>
        /// <returns>DataTable object if succeeded, otherwise an Exception is thrown. </returns>
        /// <remarks>
        /// Properties set after a succesful call of this method: 
        /// <UL>
        ///		 <LI>ErrorCode</LI>
        /// </UL>
        /// </remarks>
        public override DataTable SelectAll()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_NTBL_O_ORDENES_DISENO_SelectAll]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("TBL_O_ORDENES_DISENO");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
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
                _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_NTBL_O_ORDENES_DISENO_SelectAll' reported the ErrorCode: " + _errorCode);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TBL_O_ORDENES_DISENO::SelectAll::Error occured.", ex);
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
        public  DataTable Select_OD_Aprobadas()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_NTBL_O_ORDENES_DISENO_Aprobadas]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("TBL_O_ORDENES_DISENO");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@schr_Id_Cliente", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _chr_Id_Cliente));
                cmdToExecute.Parameters.Add(new SqlParameter("@schr_Clave_Madre_Produc", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _chr_Clave_Madre_Produc));
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
                    throw new Exception("Stored Procedure 'sp_NTBL_O_ORDENES_DISENO_SelectAll' reported the ErrorCode: " + _errorCode);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TBL_O_ORDENES_DISENO::SelectAll::Error occured.", ex);
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
        public DataTable Select_Ruta_Archi_Od()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_COM_TBL_O_RUTAFINAL_OD_Select_Ruta_ARCH_OD]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("TBL_O_RUTAFINAL_OD");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@iid_od", SqlDbType.Int, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _id_od));
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
                    throw new Exception("Stored Procedure 'sp_OD_TBL_O_RUTAFINAL_OD_Select_Ruta_ARCH_OD' reported the ErrorCode: " + _errorCode);
                }
                if (toReturn.Rows.Count > 0)
                {

               _chr_Ruta_Arch_Final = toReturn.Rows[0]["ruta"] == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["ruta"];
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TBL_O_RUTAFINAL_OD::SelectAll::Error occured.", ex);
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
        public SqlInt32 Id_od
        {
            get
            {
                return _id_od;
            }
            set
            {
                SqlInt32 id_odTmp = (SqlInt32)value;
                if (id_odTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("Id_od", "Id_od can't be NULL");
                }
                _id_od = value;
            }
        }


        public SqlInt32 Int_Estatus
        {
            get
            {
                return _int_Estatus;
            }
            set
            {
                SqlInt32 int_EstatusTmp = (SqlInt32)value;
                if (int_EstatusTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("Int_Estatus", "Int_Estatus can't be NULL");
                }
                _int_Estatus = value;
            }
        }


        public SqlString Chr_Id_Cliente
        {
            get
            {
                return _chr_Id_Cliente;
            }
            set
            {
                SqlString chr_Id_ClienteTmp = (SqlString)value;
                if (chr_Id_ClienteTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("Chr_Id_Cliente", "Chr_Id_Cliente can't be NULL");
                }
                _chr_Id_Cliente = value;
            }
        }


        public SqlString Chr_Clave_Madre_Produc
        {
            get
            {
                return _chr_Clave_Madre_Produc;
            }
            set
            {
                SqlString chr_Clave_Madre_ProducTmp = (SqlString)value;
                if (chr_Clave_Madre_ProducTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("Chr_Clave_Madre_Produc", "Chr_Clave_Madre_Produc can't be NULL");
                }
                _chr_Clave_Madre_Produc = value;
            }
        }


        public SqlString Chr_Colores
        {
            get
            {
                return _chr_Colores;
            }
            set
            {
                SqlString chr_ColoresTmp = (SqlString)value;
                if (chr_ColoresTmp.IsNull)
                {
                    throw new ArgumentOutOfRangeException("Chr_Colores", "Chr_Colores can't be NULL");
                }
                _chr_Colores = value;
            }
        }


        public SqlInt32 Id_Mov_Historial
        {
            get
            {
                return _id_Mov_Historial;
            }
            set
            {
                _id_Mov_Historial = value;
            }
        }


        public SqlInt32 Id_Log
        {
            get
            {
                return _id_Log;
            }
            set
            {
                _id_Log = value;
            }
        }


        public SqlString Chr_Ruta_Arch_Final
        {
            get
            {
                return _chr_Ruta_Arch_Final;
            }
            set
            {
                _chr_Ruta_Arch_Final = value;
            }
        }
        #endregion
    }
}
