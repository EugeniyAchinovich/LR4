using System;
using System.Data.SqlClient;

namespace AccessLayer
{
    public class AccessLayer { }

    public interface IReadableProperties
    {
        void GetPersonName(string connectionString, int id);
        void GetPersonPhone(string connectionString, int id);
        void GetCreditCard(string connectionString, int id);
        void GetProductionProduct(string connectionString, int id);
        void GetProductionProductInventory(string connectionString, int id);
	}
}