using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using webBanHang.Models;

namespace MultiShop.Controllers
{
    
    public class HomeController : Controller
    {
        MultiShopDbContext db = new MultiShopDbContext();
        public ActionResult Index()
        {
            var model = db.Categories
                .Where(c => c.Products.Count >= 4)
                .OrderBy(c => Guid.NewGuid()).ToList();
                
           
            return View(model);
        }

        public ActionResult Search()
        {
            var name = Request["term"];

            var data = db.Products
                .Where(p => p.Name.Contains(name))
                .Select(p => p.Name).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult Category()
        {
            var model = db.Categories;
            return PartialView("_Category",model);
        }

        public ActionResult Special()
        {
            var model = db.Products.Where(p=>p.Special==true).Take(5);
            return PartialView("_Special", model);
        }

        public ActionResult Saleoff()
        {
            var model = db.Products.Where(p => p.Discount>0).Take(1);
            return PartialView("_SaleOff", model);
        }

        //public ActionResult Create()
        //{
        //    if (Session["idUser"] == null)
        //    {

        //        return RedirectToAction("Login");

        //    }
        //    else
        //    {
        //        Sach sach = new Sach();
        //        sach.listchude = db.ChuDes.ToList();
        //        return View(sach);
        //    }

        //}

    //    POST: Create/Create
       //[HttpPost]

       // public ActionResult Create(Sach sach)
       // {
       //     sach.ViewNum = 0;
       //     db.Saches.Add(sach);
       //     db.SaveChanges();

       //     if (sach.ImageUpload != null || sach.ImageUpload.ContentLength > 0)
       //     {
       //         string fileName = Path.GetFileNameWithoutExtension(sach.ImageUpload.FileName);
       //         string exension = Path.GetExtension(sach.ImageUpload.FileName);
       //         fileName = fileName + exension;
       //         sach.AnhBia = "~/images/" + fileName;
       //         sach.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/images/"), fileName));
       //         db.SaveChanges();
       //     }


        //    return RedirectToAction("Index");


        //}




        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();
        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        [HttpGet]
            public ActionResult GetUsers()
            {
                return View();
            }
       
        }
    }
