using ExercisesAPI.DAL.DomainClasses;
using ExercisesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExercisesAPI.DAL.DAO
{
    public class TrayDAO
    {
        private AppDbContext _db;
        public TrayDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Tray> GetAll(int id)
        {
            return _db.Trays.Where(tray => tray.UserId == id).ToList<Tray>();
        }


        public int AddTray(int userid, TraySelectionHelper[] selections)
        {
            int trayId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Tray tray = new Tray();
                        tray.UserId = userid;
                        tray.DateCreated = System.DateTime.Now;
                        tray.TotalCalories = 0;
                        tray.TotalCholesterol = 0;
                        tray.TotalFat = 0.0M;
                        tray.TotalFibre = 0;
                        tray.TotalSalt = 0;
                        tray.TotalProtein = 0;
                        // calculate the totals and then add the tray row to the table
                        foreach (TraySelectionHelper selection in selections)
                        {
                            tray.TotalCalories += selection.item.Calories * selection.Qty;
                            tray.TotalCholesterol += selection.item.Cholesterol * selection.Qty;
                            tray.TotalFat += Convert.ToDecimal(selection.item.Fat) *
                            selection.Qty;
                            tray.TotalFibre += selection.item.Fibre * selection.Qty;
                            tray.TotalSalt += selection.item.Salt * selection.Qty;
                            tray.TotalProtein += selection.item.Protein * selection.Qty;
                        }
                        _db.Trays.Add(tray);
                        _db.SaveChanges();
                        // then add each item to the trayitems table
                        foreach (TraySelectionHelper selection in selections)
                        {
                            TrayItem tItem = new TrayItem();
                            tItem.Qty = selection.Qty;
                            tItem.MenuItemId = selection.item.Id;
                            tItem.TrayId = tray.Id;
                            _db.TrayItems.Add(tItem);
                            _db.SaveChanges();
                        }
                        // test trans by uncommenting out these 3 lines
                        //int x = 1;
                        //int y = 0;
                        //x = x / y;
                        _trans.Commit();
                        trayId = tray.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return trayId;
        }
        public List<TrayDetailsHelper> GetTrayDetails(int tid, string email)
        {
            User user = _db.Users.FirstOrDefault(user => user.Email == email);
            List<TrayDetailsHelper> allDetails = new List<TrayDetailsHelper>();
            // LINQ way of doing INNER JOINS
            var results = from t in _db.Trays
                          join ti in _db.TrayItems on t.Id equals ti.TrayId
                          join mi in _db.MenuItems on ti.MenuItemId equals mi.Id
                          where (t.UserId == user.Id && t.Id == tid)
                          select new TrayDetailsHelper
                          {
                              TrayId = t.Id,
                              UserId = user.Id,
                              TotalCalories = t.TotalCalories,
                              TotalFat = t.TotalFat,
                              TotalCholesterol = t.TotalCholesterol,
                              TotalFibre = t.TotalFibre,
                              TotalProtein = t.TotalProtein,
                              TotalSalt = t.TotalSalt,
                              Description = mi.Description,
                              MenuItemId = ti.MenuItemId,
                              Qty = ti.Qty,
                              DateCreated = t.DateCreated.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = results.ToList<TrayDetailsHelper>();
            return allDetails;
        }
    }
}
