using System;
using System.Collections.Generic;
using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using ExercisesAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExercisesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrayController : ControllerBase
    {
        AppDbContext _ctx;
        public TrayController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]
        public ActionResult<string> Index(TrayHelper helper)
        {
            string retVal = "";
            try
            {
                UserDAO uDao = new UserDAO(_ctx);
                User trayOwner = uDao.GetByEmail(helper.email);
                TrayDAO tDao = new TrayDAO(_ctx);
                int trayId = tDao.AddTray(trayOwner.Id, helper.selections);
                if (trayId > 0)
                {
                    retVal = "Tray " + trayId + " saved!";
                }
                else
                {
                    retVal = "Tray not saved";
                }
            }
            catch (Exception ex)
            {
                retVal = "Tray not saved " + ex.Message;
            }
            return retVal;
        }

        [HttpGet("{email}")]
        public ActionResult<List<Tray>> List(string email)
        {
            List<Tray> trays = new List<Tray>();
            UserDAO uDao = new UserDAO(_ctx);
            User trayOwner = uDao.GetByEmail(email);
            TrayDAO tDao = new TrayDAO(_ctx);
            trays = tDao.GetAll(trayOwner.Id);
            return trays;
        }
        [HttpGet("{trayid}/{email}")]
        public ActionResult<List<TrayDetailsHelper>> GetTrayDetails(int trayid, string email)
        {
            TrayDAO dao = new TrayDAO(_ctx);
            return dao.GetTrayDetails(trayid, email);
        }


    }
}
