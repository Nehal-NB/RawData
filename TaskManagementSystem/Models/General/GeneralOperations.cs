
using System.Data.SqlClient;

public class GeneralOperations
{
    private static string conString = "";

    public static string ConnectionString
    {
        get
        {
            if (conString == "")
            {
                SqlConnection con;
                var configuration = GetConfiguration();
                con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
                ConnectionString = con.ConnectionString;
            }
            return conString;


            IConfiguration GetConfiguration()
            {

                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build();
            }
        }
        set
        {
            conString = value;
        }

    }

}
