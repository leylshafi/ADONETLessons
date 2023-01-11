using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ADONETLesson1;

class Application
{
    SqlConnection? conn=null;
    public Application()
    {
        string connectionString = "Data Source=LEILASHAFI;Initial Catalog=Library;Integrated Security=True;";
        conn=new SqlConnection(connectionString);

    }


    public void InsertQuery()
    {
        try
        {
            conn?.Open();
            string insertString = "INSERT INTO Authors(Id,FirstName,LastName) VALUES(17,'Leyla','Shafi')";
            using SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();

            Console.WriteLine("\nDONE. Press enter. ");
            Console.ReadKey();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally {
            conn?.Close();  
        }
    }
    public void SelectQuery()
    {
        SqlDataReader? reader = null;



        try
        {
            conn?.Open();



            using SqlCommand cmd = new SqlCommand("SELECT * FROM Authors", conn);
            reader = cmd.ExecuteReader();




            while (reader.Read())
            {
                Console.WriteLine(reader[0] + "  " + reader[1] + "  " + reader[2]);
                // Console.WriteLine(reader["Id"] + " " + reader["FirstName"] + " " + reader["LastName"]);
            }




            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            conn?.Close();
            reader?.Close();
        }
    }
    public void ReadData()
    {
        SqlDataReader? reader = null;



        try
        {
            conn?.Open();



            using SqlCommand cmd = new SqlCommand("SELECT * FROM Authors", conn);
            reader = cmd.ExecuteReader();



            int line = 0;



            while (reader.Read())
            {
                if (line == 0)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write(reader.GetName(i) + "  ");



                    Console.WriteLine();
                }



                line++;



                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write(reader[i] + "  ");




                Console.WriteLine();
            }





            Console.WriteLine("\nHandled records: " + line.ToString());
            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            conn?.Close();
            reader?.Close();
        }
    }
    // Batch processing of queries
    public void MultiQueries()
    {
        SqlDataReader? reader = null;



        try
        {
            conn?.Open();



            using SqlCommand cmd = new SqlCommand("SELECT * FROM Authors; SELECT * FROM Books", conn);
            reader = cmd.ExecuteReader();




            int line = 0;



            do
            {
                while (reader.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write(reader.GetName(i) + "  ");
                        Console.WriteLine();
                    }



                    line++;




                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + "   ");
                    }
                    Console.WriteLine();




                }




                Console.WriteLine("\nHandled records: " + line.ToString());
                line = 0;
            } while (reader.NextResult());



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            conn?.Close();
            reader?.Close();
        }




        Console.WriteLine("\nDone. Press enter.");
        Console.ReadLine();
    }
    // Parameterized queries in DbCommand
    public void ParameterizedQueries()
    {
        SqlDataReader? reader = null;



        try
        {
            conn?.Open();



            using SqlCommand cmd = new SqlCommand("SELECT * FROM Authors WHERE FirstName=@p1", conn);






            //// way 1
            // SqlParameter param1 = new SqlParameter();
            // param1.ParameterName = "@p1";
            // param1.SqlDbType = SqlDbType.NVarChar;
            // param1.Value = "Kenan";
            // cmd.Parameters.Add(param1);





            //// way 2
            // cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = "Kenan";





            //// way 3
            cmd.Parameters.AddWithValue("@p1", "Leyla");



            reader = cmd.ExecuteReader();




            while (reader.Read())
            {
                Console.WriteLine(reader[0] + " " + reader[1] + " " + reader[2]);
            }



        }
        finally
        {
            conn?.Close();
            reader?.Close();
        }
    }
    // Stored procedures in DbCommand
    public void ExecStoredProcedure()
    {
        try
        {
            conn?.Open();
            using SqlCommand cmd = new SqlCommand("usp_getBooksNumber", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter inputParam = new SqlParameter("@AuthorId", SqlDbType.Int);
            inputParam.Value = 5;
            cmd.Parameters.Add(inputParam);
            SqlParameter outputParam = new SqlParameter("@BookCount", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();
            Console.WriteLine(cmd.Parameters["@BookCount"].Value);
        }
        finally
        {
            conn?.Close();
        }
    }

    public void Task1()
    {
        SqlDataReader? reader = null;
        try
        {
            conn?.Open();
            using SqlCommand cmd = new SqlCommand("SELECT SUM(Pages)SumPages FROM Books; SELECT SUM(Quantity)SumQuantity FROM Books", conn);
            reader = cmd.ExecuteReader();

            int line = 0;
            do
            {
                while (reader.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write(reader.GetName(i) + "  ");
                        Console.WriteLine();
                    }
                    line++;

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + "   ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\nHandled records: " + line.ToString());
                line = 0;
            } while (reader.NextResult());

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            conn?.Close();
            reader?.Close();
        }
        Console.WriteLine("\nDone. Press enter.");
        Console.ReadLine();
    }
    public void Task2()
    {
        SqlDataReader? reader = null;
        try
        {
            conn?.Open();



            using SqlCommand cmd = new SqlCommand("SELECT SUM(Quantity)SumQuantity FROM Books", conn);
            reader = cmd.ExecuteReader();
            int line = 0;



            while (reader.Read())
            {
                if (line == 0)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write(reader.GetName(i) + "  ");
                    Console.WriteLine();
                }
                line++;
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write(reader[i] + "  ");

                Console.WriteLine();
            }


            Console.WriteLine("\nHandled records: " + line.ToString());
            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            conn?.Close();
            reader?.Close();
        }
    }
}



class Program
{
    static void Main()
    {
        Application app = new();
        // app.InsertQuery();
        // app.SelectQuery();
        // app.ReadData(); 
        // app.MultiQueries();
        // app.ParameterizedQueries();
        app.Task1();
       // app.Task2();
    }
}
