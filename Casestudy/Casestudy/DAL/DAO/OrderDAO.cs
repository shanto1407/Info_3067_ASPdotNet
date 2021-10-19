using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Identity;

namespace Casestudy.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Order> GetAll(int id)
        {
            return _db.Orders.Where(order => order.UserId == id).ToList<Order>();
        }
        public int AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = userid;
                        order.OrderDate = System.DateTime.Now;
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            order.OrderAmount += selection.item.MSRP;
                        }
                        _db.Orders.Add(order);
                        _db.SaveChanges();
                        // then add each item to the trayitems table
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            OrderLineItem oItem = new OrderLineItem();

                            if (selection.Qty <= selection.item.QtyOnHand)
                            {
                                selection.item.QtyOnHand -= selection.Qty;
                                oItem.QtySold = selection.Qty;
                                oItem.QtyOrdered = selection.Qty;
                                oItem.QtyBackOrdered = 0;
                            }
                            else
                            {
                                selection.item.QtyOnHand = 0;
                                oItem.QtyBackOrdered = selection.Qty - selection.item.QtyOnHand;
                                oItem.QtySold = selection.Qty- oItem.QtyBackOrdered;
                                oItem.QtyOrdered = oItem.QtyBackOrdered;
                            }
                            oItem.ProductId = selection.item.Id;
                            oItem.SellingPrice = selection.item.MSRP;
                            oItem.OrderId = order.Id;
                            _db.OrderLineItems.Add(oItem);
                            _db.SaveChanges();
                        }
                        // test trans by uncommenting out these 3 lines
                        //int x = 1;
                        //int y = 0;
                        //x = x / y;
                        _trans.Commit();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return orderId;
        }
        public List<OrderDetailsHelper>GetOrderDetails(int oid,string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(customer => customer.Email == email);

            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();

            //LINQ way of doing INNER JOINS
            var results = from o in _db.Orders
                          join oli in _db.OrderLineItems on o.Id equals oli.OrderId
                          join p in _db.Products on oli.ProductId equals p.Id
                          where (o.UserId == customer.Id && o.Id == oid)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              UserId = customer.Id,
                              QtyOrder = oli.QtyOrdered,
                              QtyBackOrder = oli.QtyBackOrdered,
                              QtySold = oli.QtySold,
                              OrderAmount = o.OrderAmount,
                              ProductName = p.ProductName,
                              Description=p.Description,
                              ProductPrice = p.MSRP,
                              OrderLineItemId = oli.Id,


                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")                             
                          };
            allDetails = results.ToList<OrderDetailsHelper>();
            return allDetails;
        }
    }
}
