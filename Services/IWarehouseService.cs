using System;
using System.Threading.Tasks;
using cw5.Models;

namespace cw5.services{
    public interface IWarehouseService{

        public Task<bool> DoesProductExist(int productId);
        public Task<bool> DoesWarehouseExist(int warehouseId);
        public Task<int> GetTheValidOrderId(Order order);
        public Task<bool> CompeleteTheOrder(Order order);
        public Task<double> GetTheProductPrice(int productId);
        public Task<int> StoredProcedure(Order order);
    }
}