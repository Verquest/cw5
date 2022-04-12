using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using Microsoft.Extensions.Configuration;

namespace cw5.services{
    public class WarehouseService : IWarehouseService
    {
        public Task<bool> CompeleteTheOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoesProductExist(int productId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", productId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }

        public Task<bool> DoesWarehouseExist(int warehouseId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", productId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }


        public Task<double> GetTheProductPrice(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTheValidOrderId(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<int> StoredProcedure(Order order)
        {
            throw new NotImplementedException();
        }

    }
}