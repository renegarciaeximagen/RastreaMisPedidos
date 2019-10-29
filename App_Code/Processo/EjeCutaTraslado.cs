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
using Componente.ConexionIP2;
using System.Data.SqlTypes;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de EjeCutaTraslado
/// </summary>
public class EjeCutaTraslado : DBInteractionBase
{
    #region Class Member Declarations
    private SqlInt32 _cantidad, _partida;
    private SqlString _item, _almacenOrigen, _almacenDestino, _factura;
    private SqlDateTime _docdate, _docDueDate;
    #endregion

    public EjeCutaTraslado()
    {
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public override bool Insert()
    {
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.CommandText = "dbo.[Traslados_pruebas]";
        cmdToExecute.CommandType = CommandType.StoredProcedure;

        // Use base class' connection object
        cmdToExecute.Connection = _mainConnection;

        try
        {
            cmdToExecute.Parameters.Add(new SqlParameter("@DocDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _docdate));
            cmdToExecute.Parameters.Add(new SqlParameter("@DocDueDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _docDueDate));
            cmdToExecute.Parameters.Add(new SqlParameter("@ITEM", SqlDbType.VarChar, 11, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _item));
            cmdToExecute.Parameters.Add(new SqlParameter("@Cantidad", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _cantidad));
            cmdToExecute.Parameters.Add(new SqlParameter("@AlmacenO", SqlDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _almacenOrigen));
            cmdToExecute.Parameters.Add(new SqlParameter("@AlmacenD", SqlDbType.VarChar, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _almacenDestino));
            cmdToExecute.Parameters.Add(new SqlParameter("@PARTIDA", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _partida));
            cmdToExecute.Parameters.Add(new SqlParameter("@Factura", SqlDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _factura));



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


            if (_errorCode != (int)LLBLError.AllOk)
            {
                // Throw error.
                throw new Exception("Stored Procedure 'sp_post_TrasMan_Insert' reported the ErrorCode: " + _errorCode);
            }

            return true;
        }
        catch (Exception ex)
        {
            // some error occured. Bubble it to caller and encapsulate Exception object
            throw new Exception("TrasMan::Insert::Error occured.", ex);
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

    public override DataTable SelectOne()
    {
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.CommandText = "dbo.[sp_ConsultaAlmcen]";
        cmdToExecute.CommandType = CommandType.StoredProcedure;
        DataTable toReturn = new DataTable("ConsultaAlmcen");
        SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

        // Use base class' connection object
        cmdToExecute.Connection = _mainConnection;

        try
        {
            cmdToExecute.Parameters.Add(new SqlParameter("@ItemCode", SqlDbType.VarChar, 11, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _item));
            cmdToExecute.Parameters.Add(new SqlParameter("@WhsCode", SqlDbType.VarChar, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _almacenOrigen));         
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
                throw new Exception("Stored Procedure 'sp_post_CatStatusAlmaKN_SelectOne' reported the ErrorCode: " + _errorCode);
            }

        
            return toReturn;
        }
        catch (Exception ex)
        {
            // some error occured. Bubble it to caller and encapsulate Exception object
            throw new Exception("CatStatusAlmaKN::SelectOne::Error occured.", ex);
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
    public SqlString Factura
    {
        get { return _factura; }
        set { _factura = value; }
    }

    public SqlInt32 Partida
    {
        get { return _partida; }
        set { _partida = value; }
    }
    public SqlString AlmacenDestino
    {
        get { return _almacenDestino; }
        set { _almacenDestino = value; }
    }


    public SqlString AlmacenOrigen
    {
        get { return _almacenOrigen; }
        set { _almacenOrigen = value; }
    }
    public SqlInt32 Cantidad
    {
        get { return _cantidad; }
        set { _cantidad = value; }
    }
    public SqlString Item
    {
        get { return _item; }
        set { _item = value; }
    }


    public SqlDateTime DocDueDate
    {
        get { return _docDueDate; }
        set { _docDueDate = value; }
    }
    public SqlDateTime Docdate
    {
        get { return _docdate; }
        set { _docdate = value; }
    }
    #endregion
}
