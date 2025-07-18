using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace P0006.Metodos
{
    public class Conexion
    {
        // Correct default connection string format
        public static string db = @"Data Source=DESKTOP-AHEDRQP\SQLEXPRESS;Initial Catalog=DBMVC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        // Use Web.config if available, else fallback to default
        public static string Bd = ConfigurationManager.ConnectionStrings["cnnDbString"]?.ConnectionString ?? db;
    }

    public class Connection
    {
        public static string GetConnectionString()
        {
            return Conexion.Bd;
        }
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString ?? Conexion.Bd;
        }
    }
}