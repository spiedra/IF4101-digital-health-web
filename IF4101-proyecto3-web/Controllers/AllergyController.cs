﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Controllers
{
    public class AllergyController : Controller
    {
        public IActionResult RegisterAllergy()
        {
            return View();
        }
        public IActionResult ManageAllergy()
        {
            return View();
        }
    }
}
