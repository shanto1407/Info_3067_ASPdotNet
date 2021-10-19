using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Casestudy.DAL
{
    public class DataUtility
    {
        private AppDbContext _db;
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }
        public bool loadProductFromWebToDb(string stringJson)
        {
            bool categoriesLoaded = false;
            bool menuItemsLoaded = false;
            try
            {
                // an element that is typed as dynamic is assumed to support any operation
                dynamic objectJson = JsonSerializer.Deserialize<Object>(stringJson);
                categoriesLoaded = loadBrands(objectJson);
                menuItemsLoaded = loadProduts(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return categoriesLoaded && menuItemsLoaded;
        }

        private bool loadBrands(dynamic jsonObjectArray )
        {
            bool loadedProdects = false;
            try
            {
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();
             List<String> allBrands = new List<string>();

                foreach(JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if(element.TryGetProperty("Brand", out JsonElement brandJson ))
                    {
                        allBrands.Add(brandJson.GetString());
                    }
                }
                IEnumerable<String> brands = allBrands.Distinct<String>();
                foreach(string bname in brands)
                {
                    Brand b = new Brand();
                    b.Name = bname;
                    _db.Brands.Add(b);
                    _db.SaveChanges();
                }
                loadedProdects = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }

            return loadedProdects;
        }
        private bool loadProduts(dynamic jsonObjectArray)
        {
            bool loadProducts = false;
            try
            {
                List<Brand> brands = _db.Brands.ToList();
                //Clear out old
                _db.Products.RemoveRange(_db.Products);
                _db.SaveChanges();

                foreach(JsonElement element in jsonObjectArray.EnumerateArray())
               {
                    Product product = new Product();
                    product.Id = element.GetProperty("Id").GetString();
                    product.ProductName = element.GetProperty("ProductName").GetString();
                    product.GraphicName = element.GetProperty("GraphicName").GetString();
                    product.CostPrice = Convert.ToDecimal(element.GetProperty("CostPrice").GetString());
                    product.MSRP = Convert.ToDecimal(element.GetProperty("MSRP").GetString());
                    product.QtyOnHand = Convert.ToInt32(element.GetProperty("QtyOnHand").GetString());
                    product.QtyOnBackOrder = Convert.ToInt32(element.GetProperty("QtyonONBackOrder").GetString());
                    product.Description = element.GetProperty("Description").GetString();
                    string b = element.GetProperty("Brand").GetString();

                    //adding a FK here
                    foreach(Brand br in brands)
                    {
                        if(br.Name== b)
                        {
                            product.Brand = br;
                            break;
                        }

                    }
                    _db.Products.Add(product);
                    _db.SaveChanges();
                    loadProducts = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }


            return loadProducts;
        }
    }
}
