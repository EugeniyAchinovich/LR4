using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessLayer;
using System.Data.SqlClient;

namespace DB_Table
{
    public class Person : IReadable
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public Person(string connectionString, int id)
        {
            GetPersonName(connectionString, id);
            GetPersonPhone(connectionString, id);
        }

        public Person() { }

        public void GetPersonName(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand commands = new SqlCommand("GetNameByIDF", connection);

            commands.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter beidParam = new SqlParameter
            {
                ParameterName = "@beid",
                Value = id
            };

            commands.Parameters.Add(beidParam);
            var reader = commands.ExecuteReader();
            while (reader.Read())
            {
                this.Name = reader.GetString(0) + reader.GetString(1);
            }
        }

        public void GetPersonPhone(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand commands = new SqlCommand("GetPhoneByIDF", connection);

            commands.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter beidParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = beid
            };

            commands.Parameters.Add(beidParam);
            var reader = commands.ExecuteReader();
            while (reader.Read())
            {
                this.Phone = reader.GetString(0);
            }
        }
    }

    public class Production : IReadable
    {
        int Quantity { get; set; }
        string Name { get; set; }
        public Production(string connectionString, int id)
        {
            GetProductionProductInventory(connectionString, id);
            GetProductionProduct(connectionString, id);
        }
        public Production() { }
        public void GetProductionProductInventory(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand commands = new SqlCommand("GetProductInventoryByID", connection);
            commands.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter pidParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = id
            };

            commands.Parameters.Add(pidParam);
            var reader = commands.ExecuteReader();
            while (reader.Read())
            {
                this.Quantity = reader.GetInt32(0);
            }
        }
        public void GetProductionProduct(string connectionString, int pid)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand commands = new SqlCommand("GetProductNameByID", connection);
            commands.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter pidParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = pid
            };

            commands.Parameters.Add(pidParam);
            var reader = commands.ExecuteReader();
            while (reader.Read())
            {
                this.Name = reader.GetString(0);
            }
        }
    }

    public class Sales : IReadable
    {
        public string CreditCardNumber { get; set; }
        public Sales() { }
        public Sales(string connectionString, int id)
        {
            GetCreditCard(connectionString, id);
        }
        public void GetCreditCard(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand commands = new SqlCommand("GetCreditCardByID", connection);
            commands.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = id
            };

            commands.Parameters.Add(idParam);
            var reader = commands.ExecuteReader();
            while (reader.Read())
            {
                this.CreditCardNumber = reader.GetString(0);
            }
        }
    }
}