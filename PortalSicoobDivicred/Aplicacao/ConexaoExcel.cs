using System.Data;
using System.Data.OleDb;

namespace PortalSicoobDivicred.Aplicacao
{
    internal class ConexaoExcel
    {
        public DataTable ImportarExcel(string caminhoPlanilha, string count)
        {
            //DataSet ds = new DataSet();
            var tabela0 = new DataTable();
            //--------------------------------------------------------------------------------
            var strConexao0 =
                string.Format(
                    "Provider=Microsoft.Ace.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=no;IMEX=1\"",
                    caminhoPlanilha);
            var conn0 = new OleDbConnection(strConexao0);
            conn0.Open();
            var dt0 = conn0.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});

            if (dt0 != null)
                foreach (DataRow row in dt0.Rows)
                {
                    var sheet = row["TABLE_NAME"].ToString();
                    dt0.TableName = row["TABLE_NAME"] + "Tabela" + count;
                    var cmd0 = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn0);
                    cmd0.CommandType = CommandType.Text;

                    tabela0 = new DataTable(sheet);
                    tabela0.TableName = "Tabela" + count;
                    //ds.Tables.Add(tabela0);
                    new OleDbDataAdapter(cmd0).Fill(tabela0);
                    break;
                }

            conn0.Close();
            return tabela0;
        }
    }
}