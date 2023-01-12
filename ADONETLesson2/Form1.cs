using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ADONETLesson2
{
    public partial class Form1 : Form
    {
        SqlConnection? conn = null;
        SqlDataReader? reader = null;
        DataTable? table = null;
        SqlDataAdapter? adapter = null;
        DataSet? dataSet = null;
        SqlCommandBuilder? cmdBuilder = null;
        public Form1()
        {
            InitializeComponent();

            conn= new SqlConnection("Data Source=LEILASHAFI;Initial Catalog=Library;Integrated Security=True;");


        }

        private void WorkingWithDataTable()
        {
            DataTable table = new DataTable();




            //DataColumn column = new DataColumn()
            //{
            //    AllowDBNull = false,
            //    DataType = typeof(int),
            //    DefaultValue = 0,
            //    ColumnName = "Id"
            //};





            table.Columns.Add("Id");
            table.Columns.Add("FirstName");
            table.Columns.Add("LastName");




            table.Rows.Add(1, "Isa", "Məmmədli");
            table.Rows.Add(2, "Ali", "Şamilzadə");




            // DataRow row = table.NewRow();




            dataGridView1.DataSource = table;
        }
        // Connected Mode
        private void btnExec_Click(object sender, EventArgs e)
        {
            // WorkingWithDataTable();





            try
            {
                // SELECT * FROM Authors
                using var comm = new SqlCommand(txtCommand.Text, conn);



                conn?.Open();




                table = new DataTable();
                reader = comm.ExecuteReader();




                bool isColumnName = true;



                do
                {
                    while (reader.Read())
                    {



                        if (isColumnName)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                table.Columns.Add(reader.GetName(i));



                            isColumnName = false;
                        }




                        DataRow row = table.NewRow();



                        for (int i = 0; i < reader.FieldCount; i++)
                            row[i] = reader[i];



                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());




                dataGridView1.DataSource = table;
            }



            catch
            {
                MessageBox.Show("Probably wrong request syntax");
            }
            finally
            {
                // Close the connection
                conn?.Close();
                reader?.Close();
            }
        }
        // Disconnected Mode
        // 1) DataSet
        // 2) DbDataAdapter
        private void btnFill_Click(object sender, EventArgs e)
        {



            //adapter = new SqlDataAdapter("SELECT * FROM Books; SELECT * FROM Authors", conn);




            ////// Open()
            ////// SqlCommand
            ////// SqlDataReader
            ////// Close()




            //// table = new DataTable();
            //dataSet = new DataSet();




            ////// SqlDataAdapter
            //////      Fill()
            //////      Update()



            //adapter.Fill(dataSet, "Books");





            //// dataGridView1.DataSource = dataSet.Tables[0];
            //// dataGridView1.DataSource = dataSet.Tables[1];
            //// dataGridView1.DataSource = dataSet.Tables["table"];
            ////dataGridView1.DataSource = dataSet.Tables["table1"];
            //dataGridView1.DataSource = dataSet.Tables["Books1"];




            //MessageBox.Show(dataSet.Tables["Books1"]?.Rows[17][1].ToString());





            ////// DataSet vs SqlDataReader
            //ExampleSqlCommandBuilder();
            CustomUpdateCommand();
        }
        private void ExampleSqlCommandBuilder()
        {
            string selectSQL = "SELECT * FROM Authors;";



            adapter = new SqlDataAdapter(selectSQL, conn);




            cmdBuilder = new SqlCommandBuilder(adapter);
            //// cmdBuilder.RefreshSchema();



            dataSet = new DataSet();
            adapter.Fill(dataSet, "myTable");
            // adapter.Fill(dataSet, 2, 10, "myTable");




            dataGridView1.DataSource = dataSet.Tables[0];




            Debug.WriteLine(cmdBuilder.GetInsertCommand().CommandText);
            Debug.WriteLine(cmdBuilder.GetUpdateCommand().CommandText);
            Debug.WriteLine(cmdBuilder.GetDeleteCommand().CommandText);
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataSet is not null)
                adapter?.Update(dataSet, "myTable");
        }
        private void CustomUpdateCommand()
        {
            string selectSQL = "SELECT * FROM Books";
            adapter = new SqlDataAdapter(selectSQL, conn);




            dataSet = new DataSet();
            adapter.Fill(dataSet, "myTable");





            //// Way 1
            // SqlCommand updateCommand = new SqlCommand("UPDATE Books SET Pages=@pPages WHERE Id=@pId", conn);





            // Way 2
            SqlCommand updateCommand = new SqlCommand()
            {
                CommandText = "usp_UpdateBooks",
                Connection = conn,
                CommandType = CommandType.StoredProcedure,
            };






            updateCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
            updateCommand.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
            updateCommand.Parameters["@pId"].SourceColumn = "Id";




            updateCommand.Parameters.Add(new SqlParameter("@pPages", SqlDbType.Int));
            updateCommand.Parameters["@pPages"].SourceVersion = DataRowVersion.Current;
            updateCommand.Parameters["@pPages"].SourceColumn = "Pages";




            adapter.UpdateCommand = updateCommand;




            dataGridView1.DataSource = dataSet.Tables[0];



            MessageBox.Show(adapter.UpdateCommand.CommandText);
        }

    }
}