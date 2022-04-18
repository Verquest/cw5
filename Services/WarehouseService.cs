using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using cw5.Models;

namespace cw5.services{
    public class WarehouseService : IWarehouseService
    {
        private readonly string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
        public async Task<int> CompeleteTheOrder(Order order)
        {
           using (var connection = new SqlConnection(connectionString))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "UPDATE \"Order\" SET fulfilledAt = @1 WHERE idOrder = @2";
                command.Parameters.AddWithValue("@1", DateTime.Now);
                command.Parameters.AddWithValue("@2", order.Id);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteNonQueryAsync();

                //product price
                command.Parameters.Clear();
                command.CommandText = "SELECT Price FROM Product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", order.IdProduct);
                var price = Decimal.ToDouble((Decimal)await command.ExecuteScalarAsync());

                //inserting the order
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO Product_Warehouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
                command.Parameters.AddWithValue("@IdWarehouse", order.IdWarehouse);
                command.Parameters.AddWithValue("@IdProduct", order.IdProduct);
                command.Parameters.AddWithValue("@IdOrder", order.Id);
                command.Parameters.AddWithValue("@Amount", order.Amount);
                command.Parameters.AddWithValue("@Price", order.Amount * price);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                //getting the inserted order id
                command.Parameters.Clear();
                command.CommandText = "SELECT IdOrder FROM Product_Warehouse WHERE IdProduct = @1 AND IdWarehouse = @2 AND Amount = @3";
                command.Parameters.AddWithValue("@1", order.IdProduct);
                command.Parameters.AddWithValue("@2", order.IdWarehouse);
                command.Parameters.AddWithValue("@3", order.Amount);
                var id = (int)await command.ExecuteScalarAsync();
                return id;
            }
        }

        public async Task<bool> DoesProductExist(int productId)
        {
            using (var connection = new SqlConnection(connectionString))
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
            using (var connection = new SqlConnection(connectionString))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM warehouse WHERE IdWarehouse = @1";
                command.Parameters.AddWithValue("@1", warehouseId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }

        public async Task<bool> HasOrderBeenFulfilled(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @1";
                command.Parameters.AddWithValue("@1", orderId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }


        public async Task<double> GetTheProductPrice(int productId)
        {
            using (var connection = new SqlConnection(connectionString))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT Price FROM product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", productId);

                await connection.OpenAsync();

                var price = (double)await command.ExecuteScalarAsync();
                return price;
            }
        }

        public async Task<int> GetTheValidOrderId(int idProduct, int amount)
        {
            DateTime createdAt;
            using (var connection = new SqlConnection(connectionString))
            using(var command = new SqlCommand()){
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "SELECT IdOrder FROM \"Order\" WHERE IdProduct = @1 AND Amount = @2";
                command.Parameters.AddWithValue("@1", idProduct);
                command.Parameters.AddWithValue("@2", amount);
                int id;
                try{
                    id = (int)await command.ExecuteScalarAsync();
                }catch(NullReferenceException e){
                    return -1;
                }

                command.Parameters.Clear();
                command.CommandText = "SELECT CreatedAt FROM \"Order\" WHERE IdProduct = @1 AND Amount = @2";
                command.Parameters.AddWithValue("@1", idProduct);
                command.Parameters.AddWithValue("@2", amount);
                    
                createdAt = (DateTime)await command.ExecuteScalarAsync();
                
                if(createdAt < DateTime.Now)
                {
                    return id;
                }
            
                return -2;
            }
        }

        public Task<int> StoredProcedure(Order order)
        {
            throw new NotImplementedException();
        }

    }
}