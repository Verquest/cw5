using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using cw5.Models;

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

        public async Task<bool> DoesWarehouseExist(int warehouseId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @1";
                command.Parameters.AddWithValue("@1", warehouseId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }


        public async Task<double> GetTheProductPrice(int productId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT Price FROM product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", productId);

                await connection.OpenAsync();

                var price = double.Parse((await command.ExecuteScalarAsync()).ToString());
                return price;
            }
        }

        public async Task<int> GetTheValidOrderId(Order order)
        {
            DateTime createdAt;
            int orderId;
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT CreatedAt, IdOrder FROM Product_Warehouse WHERE IdProduct = @1 AND IdWarehouse = @2";
                command.Parameters.AddWithValue("@1", order.IdProduct);
                command.Parameters.AddWithValue("@2", order.IdWarehouse);

                await connection.OpenAsync();
                var dataReader = await command.ExecuteReaderAsync();
                createdAt = DateTime.Parse(dataReader["CreatedAt"].ToString());
                orderId = int.Parse(dataReader["orderId"].ToString());
            
                return createdAt < DateTime.Now ? orderId: -1;
            }
        }

        public Task<int> StoredProcedure(Order order)
        {
            throw new NotImplementedException();
        }

    }
}