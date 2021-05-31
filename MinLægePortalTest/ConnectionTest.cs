using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace MinLægePortalTest
{
    /// <summary>
    /// Summary description for ConnectionTest
    /// </summary>
    [TestClass]
    public class ConnectionTest
    {
        SqlConnection connection;

        [TestInitialize]
        public void InitialiseBeforeEachMethod()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        [TestMethod]
        public void TestDoWeHaveTheConnectionToDatabase()
        {
            Assert.AreEqual(System.Data.ConnectionState.Open, connection.State);
        }

        [TestCleanup]
        public void CleanUp()
        {
            connection.Dispose();
        }
    }
}
