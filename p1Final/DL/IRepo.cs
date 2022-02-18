namespace DL;

public interface IRepo
{
    List<Store> GetAllStores();

    List<Product> GetAllProducts();
    List<Product> GetAllProducts(int storeId);

    List<Order> GetAllOrders(int Id);
    List<Order> GetAllOrders();

    List<Order> GetAllOrdersDate(int Id);

    List<Order> GetAllOrdersPrice(int Id);

    List<Order> StoreOrders(int Id);

    List<Order> GetAllOrdersStoreDate(int Id);

    List<Order> GetAllOrdersStorePrice(int Id);

    void AddCustomer(Customer customerToAdd);
    void AddStore(Store storeToAdd);
    void AddProduct(Product productToAdd);

    void AddOrder(int storeId, int productId, string storeName, string productName, int quantity, int price, int userId, DateTime time);

    void UpdateInventory(int productId, int newQuantity);

    void ReplenishInventory();

    Customer Login(Customer existingCustomer);

    bool IsDuplicate(Customer customer);
    Store GetStoreByID(int id);
    void DeleteProduct(int productId);
    Order GetOrderByOrderID(int orderID);
    List<Customer> GetAllCustomers();
}