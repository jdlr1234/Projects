namespace BL;

public interface IBL
{
    List<Store> GetAllStores();

    List<Product> GetAllProducts();

    List<Order> GetAllOrders(int Id);
    List<Order> GetAllOrders();

    List<Order> GetAllOrdersDate(int Id); 

    List<Order> GetAllOrdersPrice(int Id);

    List<Order> StoreOrders(int storeId);

    List<Order> GetAllOrdersStoreDate(int storeId);

    List<Order> GetAllOrdersStorePrice(int storeId);

    void AddCustomer(Customer customerToAdd);
    void AddStore(Store storeToAdd);
    List<Customer> GetAllCustomers();
    void AddProduct(Product productToAdd);
    void DeleteProduct(int productId);

    void AddOrder(int storeId, int productId, string storeName, string productName,  int quantity, int price, int userId, DateTime time);

    void UpdateInventory(int productId, int newQuantity); 

    void ReplenishInventory();

    Customer Login(Customer existingCustomer);
    Order GetOrderByOrderID(int orderID);
    Store GetStoreByID(int id);
    List<Product> GetAllProducts(int storeID);
}