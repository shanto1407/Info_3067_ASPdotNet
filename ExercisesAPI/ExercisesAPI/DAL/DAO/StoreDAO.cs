using ExercisesAPI.DAL.DomainClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExercisesAPI.DAL.DAO
{
    public class StoreDAO
    {
        private AppDbContext _db;
        public StoreDAO(AppDbContext context)
        {
            _db = context;
        }
        public bool LoadCSVFromFile(string path)
        {
            bool addWorked = false;
            try
            {
                // clear out the old rows
                _db.Stores.RemoveRange(_db.Stores);
                _db.SaveChanges();
                var csv = new List<string[]>();
                var csvFile = path + "\\exercisesStoreRaw.csv";
                var lines = System.IO.File.ReadAllLines(csvFile);
                foreach (string line in lines)
                    csv.Add(line.Split(',')); // populate store with csv
                foreach (string[] rawdata in csv)
                {
                    Store aStore = new Store();
                    aStore.Longitude = Convert.ToDouble(rawdata[0]);
                    aStore.Latitude = Convert.ToDouble(rawdata[1]);
                    aStore.Street = rawdata[2];
                    aStore.City = rawdata[3];
                    aStore.Region = rawdata[4];
                    _db.Stores.Add(aStore);
                    _db.SaveChanges();
                }
                addWorked = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return addWorked;

        }
        public List<Store> GetThreeClosestStores(float? lat, float? lng)
        {
            List<Store> storeDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lngParam = new SqlParameter("@lng", lng);
                var query = _db.Stores.FromSqlRaw("dbo.pGetThreeClosestStores @lat, @lng", latParam,
                lngParam);
                storeDetails = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return storeDetails;
        }

    }
}