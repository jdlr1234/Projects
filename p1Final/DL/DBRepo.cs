using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
namespace DL;

public class DBRepo : IRepo
{
    private string _connectionString;
    public DBRepo(string connectionString) {
        _connectionString = connectionString;
    }
    /*
    All Adding to DB 
    */
    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="customerToAdd">Takes in a customer object to create</param>
    public void AddCustomer(Customer customerToAdd)
    {
        DataSet restoSet = new DataSet();
        string selectCmd = "SELECT * FROM Customer WHERE Id = -1";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            using(SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCmd, connection))
            {
                dataAdapter.Fill(restoSet, "Customer");

                DataTable restoTable = restoSet.Tables["Customer"];
                DataRow newRow = restoTable.NewRow();

                newRow["Username"] = customerToAdd.UserName;
                newRow["Password"] = customerToAdd.Password ?? "";
                
                restoTable.Rows.Add(newRow);

                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);

                dataAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
  
                dataAdapter.Update(restoTable);
            }
        }
    }

    public void AddProduct(Product productToAdd)
    {
        DataSet restoSet = new DataSet();
        string selectCmd = "SELECT * FROM Product WHERE Id = -1";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            using(SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCmd, connection))
            {
                dataAdapter.Fill(restoSet, "Product");

                DataTable restoTable = restoSet.Tables["Product"];
                DataRow newRow = restoTable.NewRow();

                newRow["ProductName"] = productToAdd.ProductName;
                newRow["Price"] = productToAdd.Price;
                newRow["Brand"] = productToAdd.Brand;
                newRow["Inventory"] = productToAdd.Inventory;
                newRow["StoreID"]=productToAdd.storeID;
                
                restoTable.Rows.Add(newRow);

                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);

                dataAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
  
                dataAdapter.Update(restoTable);
            }
        }
    }
    public void AddStore(Store storeToAdd)
    {
        DataSet restoSet = new DataSet();
        string selectCmd = "SELECT * FROM Store WHERE Id = -1";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            using(SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCmd, connection))
            {
                dataAdapter.Fill(restoSet, "Store");

                DataTable restoTable = restoSet.Tables["Store"];
                DataRow newRow = restoTable.NewRow();

                newRow["Name"] = storeToAdd.Name;
                newRow["City"] = storeToAdd.City ?? "";
                newRow["State"] = storeToAdd.State;
                
                restoTable.Rows.Add(newRow);

                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);

                dataAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
  
                dataAdapter.Update(restoTable);
            }
        }
    }
    /// <summary>
    /// Adds StoreId, StoreName, ProductId, ProductName, quantity, price, user ID, and time to Order
    /// </summary>
    /// <param name="storeId">Takes in store ID int</param>
    /// <param name="productId">Takes in product ID int</param>
    /// <param name="storeName">Takes in store name string</param>
    /// <param name="productName">Takes in product name string</param>
    /// <param name="quantity">Takes in quantity int</param>
    /// <param name="price">Takes in price decimal</param>
    /// <param name="userId">Takes in user ID int</param>
    /// <param name="time">Takes in DateTime object</param>
    public void AddOrder(int storeId, int productId, string storeName, string productName, int quantity, int price, int userId, DateTime time)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlCmd = "INSERT INTO Orders (StoreId, StoreName, ProductId, ProductName, Quantity, TotalPrice, UserId, Time) VALUES (@stoId, @stoname, @prodId, @prodname, @quantity, @totalprice, @userId, @time)";
            using(SqlCommand cmd = new SqlCommand(sqlCmd, connection))
            {
                SqlParameter param = new SqlParameter("@stoId", storeId);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@stoName", storeName);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@prodId", productId);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@prodName", productName);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@quantity", quantity);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@totalprice", price*quantity);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@userId", userId);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@time", DateTime.Now);
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    /// <summary>
    /// Used to make changed in the inventory amount of each product
    /// </summary>
    /// <param name="productId">Used to select which product to update</param>
    /// <param name="newQuantity">The updated amount</param>
    public void UpdateInventory(int productId, int newQuantity)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlCmd = "UPDATE Product SET Inventory = @Quantity WHERE Id = @Id";
            using(SqlCommand cmd = new SqlCommand(sqlCmd, connection))
            {
                SqlParameter param = new SqlParameter("@Quantity", newQuantity);
                cmd.Parameters.Add(param);
                param = new SqlParameter("@Id", productId);
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    /// <summary>
    /// Updates any inventory based on how low inventory is
    /// </summary>
    public void ReplenishInventory()
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlCmd = "UPDATE Product SET Inventory = (Inventory) + 10 WHERE Inventory<10";
            SqlCommand cmd = new SqlCommand(sqlCmd, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    /// <summary>
    /// Retrieves all the stores in the Store table
    /// </summary>
    /// <returns>A list of all the stores</returns>
    public List<Store> GetAllStores()
    {
        List<Store> allStores = new List<Store>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string stoSelect = "Select * From Store";
        DataSet StoreSet = new DataSet();
        using SqlDataAdapter stoAdapter = new SqlDataAdapter(stoSelect, connection);
        stoAdapter.Fill(StoreSet, "Store");
        DataTable? StoreTable = StoreSet.Tables["Store"];
        if(StoreTable != null)
        {
            foreach(DataRow row in StoreTable.Rows)
            {
                Store sto = new Store(row);
                allStores.Add(sto);
            }
        }
        return allStores;
    }

    /// <summary>
    /// Gets everything from Product table
    /// </summary>
    /// <returns>Everything from product table</returns>
    public List<Product> GetAllProducts()
    {
        List<Product> allProducts = new List<Product>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string prodSelect = "Select * From Product";
        DataSet ProductSet = new DataSet();
        using SqlDataAdapter prodAdapter = new SqlDataAdapter(prodSelect, connection);
        prodAdapter.Fill(ProductSet, "Product");
        DataTable? ProductTable = ProductSet.Tables["Product"];
        if(ProductTable != null)
        {
            foreach(DataRow row in ProductTable.Rows)
            {
                Product prod = new Product(row);
                allProducts.Add(prod);
            }
        }
        return allProducts;
    }

    /// <summary>
    /// Retrieves everything from Orders table based on StoreID
    /// </summary>
    /// <param name="storeId">Uses this to filter table by chosen store ID</param>
    /// <returns>Orders for the store chosen</returns>
    public List<Order> GetAllOrders(int Id)
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"Select * From Orders WHERE UserId = {Id}";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if(OrderTable != null)
        {
            foreach(DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }
    public List<Order> GetAllOrders()
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"Select * From Orders";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if (OrderTable != null)
        {
            foreach (DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }


    /// Gets user orders sorted by d

    /// <summary>
    /// Gets user orders sorted by date from new to old
    /// </summary>
    /// <param name="storeId">Takes the user ID to search for orders</param>
    /// <returns>A list of orders for user</returns>
    public List<Order> GetAllOrdersDate(int Id)
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"Select * From Orders WHERE UserId = {Id} ORDER BY Time DESC";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if(OrderTable != null)
        {
            foreach(DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }

    /// <summary>
    /// Gets user orders sorted by price from high to low
    /// </summary>
    /// <param name="storeId">Takes the user ID to search for orders</param>
    /// <returns>A list of orders for user</returns>
    public List<Order> GetAllOrdersPrice(int Id)
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"Select * From Orders WHERE UserId = {Id} ORDER BY TotalPrice DESC";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if(OrderTable != null)
        {
            foreach(DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }

    /// <summary>
    /// Gets all specified storefront orders
    /// </summary>
    /// <param name="storeId">Takes the store ID to search for orders</param>
    /// <returns>A list of orders for specified store</returns>
    public List<Order> StoreOrders(int storeId)
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"SELECT * FROM Orders WHERE StoreId = {storeId}";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if(OrderTable!= null)
        {
            foreach(DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }

        /// <summary>
    /// Uses query statement to get specific user
    /// </summary>
    /// <param name="customer">Takes in Customer object</param>
    /// <returns>Returns same Customer but from DB if successful</returns>
    public Customer Login(Customer customer)
    {
        string searchQuery = $"SELECT * FROM Customer WHERE Username='{customer.UserName}'";
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(searchQuery, connection);
        connection.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        Customer acc = new Customer();
        if(reader.Read())
        {
            acc.Id = reader.GetInt32(0);
            acc.UserName = reader.GetString(1);
            acc.Password = reader.GetString(2);
        }
        return acc;
    }

    /// <summary>
    /// Gets specified storefront orders sorted by date from old to new
    /// </summary>
    /// <param name="storeId">Takes the store ID to search for orders</param>
    /// <returns>A list of orders for specified store</returns>
    public List<Order> GetAllOrdersStoreDate(int storeId)
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"SELECT * FROM Orders WHERE StoreId = {storeId} ORDER BY Time ASC";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if(OrderTable!= null)
        {
            foreach(DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }

    /// <summary>
    /// Gets specified storefront orders sorted by price from high to low
    /// </summary>
    /// <param name="storeId">Takes the store ID to search for orders</param>
    /// <returns>A list of orders for specified store</returns>
    public List<Order> GetAllOrdersStorePrice(int storeId)
    {
        List<Order> allOrders = new List<Order>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"SELECT * FROM Orders WHERE StoreId = {storeId} ORDER BY TotalPrice DESC";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if(OrderTable!= null)
        {
            foreach(DataRow row in OrderTable.Rows)
            {
                Order ord = new Order(row);
                allOrders.Add(ord);
            }
        }
        return allOrders;
    }

    /// <summary>
    /// Search for the Username for exact match of name
    /// </summary>
    /// <param name="customer">Customer object to search for dup</param>
    /// <returns>bool: true if there is duplicate, false if not</returns>
    public bool IsDuplicate(Customer customer)
    {
        string searchQuery = $"SELECT * FROM Customer WHERE Username='{customer.UserName}'";
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(searchQuery, connection);
        connection.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if(reader.HasRows)
        {
            return true;
        }
        return false;
    }

    public Store GetStoreByID(int id)
    {
        List<Store> allStores = GetAllStores();
        foreach (Store store in allStores)
        {
            if (store.Id == id)
            {
                return store;
            }
        }
        return new Store();

    }

    public void DeleteProduct(int productID)
    {
        //Establishing new connection
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        //Our insert command to add a product
        string sqlCmd = "DELETE FROM Product WHERE Id=@prodID";
        using SqlCommand cmdDeleteProduct = new SqlCommand(sqlCmd, connection);
        {
            SqlParameter param = new SqlParameter("@prodID", productID);
            cmdDeleteProduct.Parameters.Add(param);
            cmdDeleteProduct.ExecuteNonQuery();

        }

        connection.Close();
    }

    public List<Product> GetAllProducts(int storeId)
    {
        List<Product> allProducts = GetAllProducts();
        List<Product> products = new List<Product>();

        for(int i = 0; i < allProducts.Count; i++)
        {
            if (allProducts[i].storeID == storeId)
            {
                products.Add(allProducts[i]);
            }
        }
        return products;

    }

    public Order GetOrderByOrderID(int orderID)
    {
        List<Order> allOrders = GetAllOrders();
        foreach (Order order in allOrders)
        {
            if (order.Id == orderID)
            {
                return order;
            }
        }
        return new Order();
    }

    public List<Customer> GetAllCustomers()
    {
        List<Customer> allCustomers = new List<Customer>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        string ordSelect = $"Select * From Customer";
        DataSet OrderSet = new DataSet();
        using SqlDataAdapter ordAdapter = new SqlDataAdapter(ordSelect, connection);
        ordAdapter.Fill(OrderSet, "Order");
        DataTable? OrderTable = OrderSet.Tables["Order"];
        if (OrderTable != null)
        {
            foreach (DataRow row in OrderTable.Rows)
            {
                Customer cust = new Customer(row);
                allCustomers.Add(cust);
            }
        }
        return allCustomers;
    }
}