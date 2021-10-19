using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Casestudy.DAL;
using Casestudy.Helpers;
using Microsoft.AspNetCore.Authorization;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using CaseStudyAPI.DAL.DomainClasses;

namespace Casestudy.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here
        {
            _ctx = context;
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<string> Index(OrderHelper helper)
        {
            string retVal = "";
            try
            {
                CustomerDAO cDao = new CustomerDAO(_ctx);
                Customer trayOwner = cDao.GetByEmail(helper.email);
                OrderDAO oDao = new OrderDAO(_ctx);
                int orderId = oDao.AddOrder(trayOwner.Id, helper.selections);
                if (orderId > 0)
                {
                    retVal = "Order " + orderId + " saved!";
                }
                else
                {
                    retVal = "Order not Saved";
                }
            }catch(Exception ex)
            {
                retVal = "Order Not Saved!" + ex.Message;
            }
            return retVal;
        }

        [HttpGet("{email}")]
        public ActionResult<List<Order>> List(string email)
        {
            List<Order> orders = new List<Order>();
            CustomerDAO cDao = new CustomerDAO(_ctx);
            Customer orderOwner = cDao.GetByEmail(email);
            OrderDAO oDao = new OrderDAO(_ctx);
            orders = oDao.GetAll(orderOwner.Id);
            return orders;
        }
        [HttpGet("{orderid}/{email}")]
        public ActionResult<List<OrderDetailsHelper>> GetOrderDetails(int orderid, string email)
        {
            OrderDAO dao = new OrderDAO(_ctx);
            return dao.GetOrderDetails(orderid, email);
        }


    }
}
