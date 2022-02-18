using CustomExceptions;
using System.Text.RegularExpressions;
using System.Data;

namespace Models;

public class Product {
    public Product()
    {
        this.Orders = new List<Order>();
    }
    public Product(string name)
    {
        this.Orders = new List<Order>();
        this._name = name;
    }

    public Product(DataRow row)
    {
        this.Id = (int) row["Id"];
        this.ProductName = row["ProductName"].ToString() ?? "";
        this.Price = (int) row["Price"];
        this.Brand = row["Brand"].ToString() ?? "";
        this.Inventory = (int) row["Inventory"];
        this.storeID = (int) row["StoreID"];
    }

    public int Id { get; set; }

    private string _name;
    public string ProductName {
        get => _name;
        set
        {
            Regex pattern = new Regex("^[a-zA-Z0-9 !?']+$");
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new InputInvalidException("Name can't be empty");
            }
            else if(!pattern.IsMatch(value))
            {
                throw new InputInvalidException("Restaurant name can only have alphanumeric characters, white space, !, ?, and '.");
            }
            else
            {
                this._name = value;
            }
        } 
    }

    public int Price { get; set; }
    public int storeID {get;set;}
    public string Brand { get; set; }
    public int Inventory { get; set; }
    public List<Order> Orders { get; set; }

    public override string ToString()
    {
        return $"Reference #: {this.Id} \nProduct: {this.ProductName}  \nPrice: {this.Price} \nInventory: {this.Inventory} \nBrand: {this.Brand}\n";
    }

    public void ToDataRow(ref DataRow row)
    {
        row["ProductName"] = this.ProductName;
        row["Brand"] = this.Brand;
        row["Price"] = this.Price;
        row["Inventory"] = this.Inventory;
    }
}