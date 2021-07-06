# MvcRedirect

#      
        public IActionResult Action1()
        {
            var valueFromField = "Cool value";
            return this.RedirectTo<HomeController>(a => a.Index(valueFromField, "Cool value 2"));
        }
        
        public IActionResult Index(string title, string title1)
        {
            ViewBag.Title = title;
            ViewBag.Title1 = title1;
            return View();
        }
