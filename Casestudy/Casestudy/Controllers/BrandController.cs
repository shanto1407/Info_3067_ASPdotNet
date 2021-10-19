﻿using System.Collections.Generic;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Casestudy.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        public ActionResult<List<Brand>> Index()
        {
            BrandDAO dao = new BrandDAO(_db);
            List<Brand> allBrands = dao.GetAll();
            return allBrands;
        }
    }

}