
using System.Data;
using TaskManagementSystem.Models.General;
using Microsoft.Data.SqlClient;

namespace TaskManagementSystem.Repository
{
    public class TaskMasterRepository: ITaskMasterRepository
    {
        public RetDataSet GetItemTask()
        {
            RetDataSet ret = new RetDataSet();

            try
            {
                using (SqlConnection conn = new SqlConnection(GeneralOperations.ConnectionString))
                {
                    conn.Open();

                    string SQL = "SELECT Title, Description, Status, CreatedAt FROM dbo.Tasks";

                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            DA.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    Dictionary<string, object> rowData = new Dictionary<string, object>();
                                    foreach (DataColumn col in dt.Columns)
                                    {
                                        rowData[col.ColumnName] = row[col];
                                    }
                                    ret.Data.Add(rowData);
                                }

                                ret.Status = true;
                                ret.ErrorDescription = string.Empty;
                            }
                            else
                            {
                                ret.Status = false;
                                ret.ErrorDescription = "No records found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret.Data = null;
                ret.ErrorDescription = ex.Message;
                ret.Status = false;
            }

            return ret;
        }
        public RetDataSet GetItemTaskByID(int Id)
        {
            RetDataSet ret = new RetDataSet();

            try
            {
                using (SqlConnection conn = new SqlConnection(GeneralOperations.ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM Tasks WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", Id); // Corrected from 'id' to 'Id'

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    Dictionary<string, object> rowData = new Dictionary<string, object>();
                                    foreach (DataColumn col in dt.Columns)
                                    {
                                        rowData[col.ColumnName] = row[col];
                                    }
                                    ret.Data.Add(rowData);
                                }

                                ret.Status = true;
                                ret.ErrorDescription = string.Empty;
                            }
                            else
                            {
                                ret.Status = false;
                                ret.ErrorDescription = "No records found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret.Data = null;
                ret.ErrorDescription = ex.Message;
                ret.Status = false;
            }

            return ret;
        }


        public RetDBAction TaskMasterSave(TaskItem task)
        {
            RetDBAction ret = new RetDBAction();
            string SQL = string.Empty;

            try
            {

                GeneralOperations ObjCon = new GeneralOperations();
                using (SqlConnection Conn = new SqlConnection(GeneralOperations.ConnectionString))
                {
                    Conn.Open();

                    SQL = "SELECT Title FROM dbo.Tasks WHERE Title=@Title";

                    SqlDataReader rdr = null;
                    using (SqlCommand CMD = new SqlCommand(SQL, Conn))
                    {
                        CMD.Parameters.AddWithValue("@Title", task.Title);

                        rdr = CMD.ExecuteReader();
                        if (rdr.Read())
                        {
                            rdr.Close();
                            rdr.Dispose();
                            throw new Exception("Title already exists");
                        }
                        else
                        {
                            rdr.Close();
                            rdr.Dispose();
                        }
                    }

                    SQL = string.Empty;
                    SQL = "INSERT INTO dbo.Tasks (Title, Description, Status, CreatedAt) VALUES (@Title, @Description, @Status, @CreatedAt)";

                    using (SqlCommand cmd = new SqlCommand(SQL, Conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", task.Title);
                        cmd.Parameters.AddWithValue("@Description", task.Description);
                        cmd.Parameters.AddWithValue("@Status", task.Status);
                        cmd.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);

                        cmd.ExecuteNonQuery();
                    }
                    ret.Status = true;
                    ret.ErrorDescription = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.ErrorDescription = ex.Message;
                ret.Status = false;
                ret.RetValue = -1;
            }

            return ret;
        }



        public RetDBAction TaskMasterUpdate(TaskItem task)
        {
            RetDBAction ret = new RetDBAction();
            string SQL = string.Empty;

            try
            {

                GeneralOperations ObjCon = new GeneralOperations();
                using (SqlConnection Conn = new SqlConnection(GeneralOperations.ConnectionString))
                {
                    Conn.Open();

                    SQL = "SELECT * FROM dbo.Tasks WHERE Title=@Title AND Id <> @Id";

                    SqlDataReader rdr = null;
                    using (SqlCommand CMD = new SqlCommand(SQL, Conn))
                    {
                        CMD.Parameters.AddWithValue("@Title", task.Title);
                        CMD.Parameters.AddWithValue("@Id", task.Id);

                        rdr = CMD.ExecuteReader();
                        if (rdr.Read())
                        {
                            rdr.Close();
                            rdr.Dispose();
                            throw new Exception("Tasks already exists");
                        }
                        else
                        {
                            rdr.Close();
                            rdr.Dispose();
                        }
                    }

                    SQL = string.Empty;
                    SQL = " UPDATE dbo.Tasks SET Title = @Title, Description = @Description, Status = @Status WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(SQL, Conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", task.Id);
                        cmd.Parameters.AddWithValue("@Title", task.Title);
                        cmd.Parameters.AddWithValue("@Description", task.Description);
                        cmd.Parameters.AddWithValue("@Status", task.Status);

                        cmd.ExecuteNonQuery();
                    }
                    ret.Status = true;
                    ret.ErrorDescription = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.ErrorDescription = ex.Message;
                ret.Status = false;
                ret.RetValue = -1;
            }

            return ret;
        }

        public RetDBAction TaskMasterPartialUpdate(int taskId, Dictionary<string, object> fieldsToUpdate)
        {
            RetDBAction ret = new RetDBAction();

            try
            {
                if (fieldsToUpdate == null || fieldsToUpdate.Count == 0)
                {
                    throw new Exception("No fields provided to update.");
                }

                using (SqlConnection conn = new SqlConnection(GeneralOperations.ConnectionString))
                {
                    conn.Open();

                    // Prevent duplicate task title (optional logic)
                    if (fieldsToUpdate.ContainsKey("Title"))
                    {
                        string checkSQL = "SELECT * FROM dbo.Tasks WHERE Title = @Title AND Id <> @Id";
                        using (SqlCommand checkCmd = new SqlCommand(checkSQL, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@Title", fieldsToUpdate["Title"]);
                            checkCmd.Parameters.AddWithValue("@Id", taskId);

                            using (SqlDataReader rdr = checkCmd.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    throw new Exception("Task with the same title already exists.");
                                }
                            }
                        }
                    }

                    // Build dynamic UPDATE SQL
                    List<string> setClauses = new List<string>();
                    SqlCommand updateCmd = new SqlCommand();
                    updateCmd.Connection = conn;

                    foreach (var field in fieldsToUpdate)
                    {
                        string columnName = field.Key;
                        string parameterName = "@" + columnName;

                        setClauses.Add($"{columnName} = {parameterName}");
                        updateCmd.Parameters.AddWithValue(parameterName, field.Value ?? DBNull.Value);
                    }

                    string setClause = string.Join(", ", setClauses);
                    updateCmd.CommandText = $"UPDATE dbo.Tasks SET {setClause} WHERE Id = @Id";
                    updateCmd.Parameters.AddWithValue("@Id", taskId);

                    updateCmd.ExecuteNonQuery();

                    ret.Status = true;
                    ret.ErrorDescription = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.Status = false;
                ret.ErrorDescription = ex.Message;
                ret.RetValue = -1;
            }

            return ret;
        }

        public RetDBAction TaskMasterDelete(TaskItem task)
        {
            RetDBAction ret = new RetDBAction();

            try
            {

                GeneralOperations ObjCon = new GeneralOperations();

                using (SqlConnection Conn = new SqlConnection(GeneralOperations.ConnectionString))
                {
                    Conn.Open();
                    string SQL = "DELETE FROM Tasks WHERE Id = @Id";
                    using (SqlCommand CMD = new SqlCommand(SQL, Conn))
                    {
                        CMD.Parameters.AddWithValue("@Id", task.Id);

                        ret.RetValue = CMD.ExecuteNonQuery();
                    }
                    ret.Status = true;
                    ret.ErrorDescription = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.ErrorDescription = ex.Message;
                ret.Status = false;
                ret.RetValue = -1;
            }

            return ret;
        }

    }
}
