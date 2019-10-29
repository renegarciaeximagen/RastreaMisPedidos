using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace Componente.Procesos
{
    public class SeccionMySql
    {
        #region Class Member Declarations
        private string _logineshop, id_customer, _customers;
        #endregion

        public  DataSet SeccionID()
        {

            DataSet dt = new DataSet();


            string Sqlquery = string.Empty;

            //OdbcConnection OdbcConn = new OdbcConnection("DSN=PromoShop;UID=promosho_promosh;PWD=AxJtIb37k97N;");
            OdbcConnection OdbcConn = new OdbcConnection("DSN=promo_eximagen;UID=promo_apps;PWD=R8svMTeMqk5r;");
            OdbcConn.Open();

            Sqlquery = "SELECT whos_online.customer_id ,customers.customers_razSoc,customers.customers_numCliente FROM whos_online ";
            Sqlquery += " INNER JOIN customers ON customers.customers_id= whos_online.customer_id";
            Sqlquery += " WHERE whos_online.session_id='"+ _logineshop +"'";

            OdbcCommand OdbcCom = new OdbcCommand(Sqlquery, OdbcConn);

            OdbcDataReader odbcReade = OdbcCom.ExecuteReader();

            dt = DataSetFromDataReader(odbcReade);
            OdbcConn.Close();

            return dt;



        }

        public DataSet Eliminar_Customers()
        {

            DataSet dt = new DataSet();


            string Sqlquery = string.Empty;

           // OdbcConnection OdbcConn = new OdbcConnection("DSN=PromoShop;UID=promosho_promosh;PWD=AxJtIb37k97N;");
            OdbcConnection OdbcConn = new OdbcConnection("DSN=promo_eximagen;UID=promo_apps;PWD=R8svMTeMqk5r;");
            OdbcConn.Open();

            Sqlquery = "UPDATE customers SET Desactivo = 'E',customers_email_address='" + _customers + "' WHERE customers_id =" + id_customer;


            OdbcCommand OdbcCom = new OdbcCommand(Sqlquery, OdbcConn);

            OdbcCom.ExecuteReader();


            OdbcConn.Close();

            return dt;



        }

        public DataSet DataSetFromDataReader(IDataReader reader)
        {
            DataSet ds = new DataSet();

            // Loop through result sets, creating a DataTable for each
            do
            {
                DataTable schema = reader.GetSchemaTable();
                DataTable table = new DataTable();

                if (schema != null)
                {
                    // Create a DataTable and add it to the DataSet
                    for (int i = 0; i < schema.Rows.Count; i++)
                    {
                        DataRow row = schema.Rows[i];
                        string name = (string)row["ColumnName"];
                        DataColumn column = new DataColumn(name,
                            (Type)row["DataType"]);
                        table.Columns.Add(column);
                    }

                    ds.Tables.Add(table);

                    // Fill the data table
                    while (reader.Read())
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[i] = reader.GetValue(i);
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    DataColumn column = new DataColumn("RowsAffected");
                    table.Columns.Add(column);
                    ds.Tables.Add(table);
                    DataRow row = table.NewRow();
                    row[0] = reader.RecordsAffected;
                    table.Rows.Add(row);
                }
            } while (reader.NextResult());

            return ds;
        }
       
    #region Class Property Declarations
        public string Logineshop
        {
            get { return _logineshop; }
            set { _logineshop = value; }
        }
        public string Id_customer
        {
            get { return id_customer; }
            set { id_customer = value; }
        }
        public string Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }
        #endregion

    }
}