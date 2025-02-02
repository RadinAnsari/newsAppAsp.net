using NewsWeb.Models.Domain;
using System;
using System.Collections.Generic;
using NewsWeb.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NewsWeb.Controllers
{
    public class AdminController : Controller
    {
        DataBaseNews db = new DataBaseNews();
        NewsSite.Models.Utility.Utility utility = new NewsSite.Models.Utility.Utility();
        // GET: /Admin/
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {

                return View();
            }
            else
            {
                ViewBag.style = "color:red;";
                ViewBag.Message = "ابتدا باید وارد سایت شوید";
                return RedirectToAction("LogIn", "Home");

            }

        }

        public ActionResult Logout()
        {
            if (Session["UserName"] != null)
            {
                Session["UserName"] = null;
                return RedirectToAction("logIn", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult EditPassword()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }

            else
            { return RedirectToAction("LogIn", "Home"); }
        }

        [HttpPost]
        public ActionResult EditPassword(string Txt_CurrentPsw, string Txt_NewPsw, string Txt_ConfirmNewPsw)
        {

            if (Session["UserName"] != null)
            {
                if (Txt_CurrentPsw == "" || Txt_CurrentPsw == null || Txt_NewPsw == "" || Txt_NewPsw == null || Txt_ConfirmNewPsw == "" || Txt_ConfirmNewPsw == null)
                {
                    ViewBag.style = "color:red;";
                    ViewBag.message = "همه فیلد را پر نمایید";
                    return View();
                }

                if (Txt_NewPsw.Trim() != Txt_ConfirmNewPsw.Trim())
                {
                    ViewBag.style = "color:red;";
                    ViewBag.message = "کلمه عبور جدید را به درستی وارد نمایید";
                    return View();
                }
                string username = Session["UserName"].ToString();
                var q = (from a in db.Tbl_Users where a.UserName.Equals(username) && a.Psw.Equals(Txt_CurrentPsw) select a).SingleOrDefault();
                if (q != null)
                {
                    q.Psw = Txt_NewPsw.Trim();
                    db.Tbl_Users.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() <= 0)
                    {
                        ViewBag.style = "color:red;";
                        ViewBag.message = "کلمه عبور ویرایش نشد";
                        return View();
                    }
                    else
                    {
                        ViewBag.style = "color:green;";
                        ViewBag.message = "کلمه عبور با موفقیت ویرایش شد";
                        return View();
                    }
                }
                else
                {
                    ViewBag.style = "color:red;";
                    ViewBag.message = "کلمه عبور فعلی را به درستی وارد نمایید";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        [HttpGet]
        public ActionResult EditUser(Tbl_Users TblUser)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    string username = Session["UserName"].ToString();
                    var q = (from a in db.Tbl_Users where a.UserName.Equals(username) select a).SingleOrDefault();
                    return View(q);
                }

                else
                { return RedirectToAction("LogIn", "Home"); }
            }
            catch
            {
                return Content("have an error.");

            }

        }

        [HttpPost]
        public ActionResult EditUser(Tbl_Users TblUser, HttpPostedFileBase PicUser)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    string username = Session["UserName"].ToString();
                    var q = (from a in db.Tbl_Users where a.UserName.Equals(username) select a).SingleOrDefault();

                    q.Name = TblUser.Name;
                    q.Ncode = TblUser.Ncode;
                    q.Mobnumber = TblUser.Mobnumber;
                    q.Discription = TblUser.Discription;

                    if (PicUser != null)
                    {
                        if (PicUser.ContentType == "image/jpeg")
                        {
                            if (PicUser.ContentLength <= 800000 && PicUser.ContentLength >= 80000)
                            {
                                string PicName = q.UserName.ToString() + ".jpeg";
                                string path = System.IO.Path.Combine(Server.MapPath(@"~/Content/img/UserPic/"));
                                PicUser.SaveAs(path + PicName);

                                q.Image = PicName;
                            }
                            else
                            {
                                ViewBag.style = "color:red";
                                ViewBag.Message = "حجم تصویر باید بین 80 تا 800 کیلوبایت باشد";
                                return View();
                            }
                        }
                    }


                    db.Tbl_Users.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        ViewBag.style = "color:green";
                        ViewBag.Message = "با موفقیت ویرایش شد";
                        return View();
                    }

                }

                else
                { return RedirectToAction("LogIn", "Home"); }
                return View();
            }
            catch
            {
                return Content("have an error.");

            }

        }

        [HttpGet]
        public ActionResult EditEmail()
        {
            try
            {
                if (Session["UserName"] != null)
                {

                    return View();
                }
                else
                {
                    return RedirectToAction("LogIn", "Home");
                }
            }
            catch
            {
                return Content("خطایی رخ داده است ، لطفا دوباره تلاش کنید");
            }

        }

        [HttpPost]
        public ActionResult EditEmail(string Txt_Email)
        {
            try
            {
                #region inner Code
                if (Session["UserName"] != null)
                {
                    string Username = Session["UserName"].ToString();
                    var q = (from a in db.Tbl_Users
                             where a.UserName.Equals(Username)
                             select a).SingleOrDefault();
                    if (q != null)
                    {
                        if (q.Email.Equals(Txt_Email))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            var q2 = db.Tbl_Users.Select(a => a).Where(a => a.Email.Equals(Txt_Email));
                            if (q2.Count() == 0)
                            {
                                q.Email = Txt_Email;
                                db.Tbl_Users.Attach(q);
                                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                                if (db.SaveChanges() > 0)
                                {
                                    ViewBag.style = "color:green;";
                                    ViewBag.Message = "ایمیل با موفقیت ویرایش شد";
                                    return View();
                                }
                                else
                                {
                                    ViewBag.style = "color:red;";
                                    ViewBag.Message = "ایمیل با موفقیت ویرایش نشد";
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.style = "color:red;";
                                ViewBag.Message = "این ایمیل تکراری است";
                                return View();
                            }
                        }
                    }
                    else
                    {
                        Session["UserName"] = null;
                        return RedirectToAction("LogIn", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("LogIn", "Home");
                }
                #endregion
            }
            catch
            {
                return Content("خطایی رخ داده است ، لطفا دوباره تلاش کنید");
            }

        }

        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ManagePosts()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    if (Session["Access"].ToString().Equals("Admin"))
                    {
                        var q = from a in db.Tbl_News
                                orderby a.Id descending
                                select a;
                        return View(q);
                    }
                    else
                    {
                        return View();
                    }
                }
                return RedirectToAction("LogIn", "Home");
            }
            catch
            {

                return Content("خطا در ارتباط...لطفا دوباره تلاش کنید");
            }
        }

        public ActionResult DeletePost(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {

                    if (Session["Access"].ToString().Equals("Admin"))
                    {
                        var qComment = from a in db.Tbl_Comment
                                       where a.NewsId.Equals(id)
                                       select a;
                        var qRNewsCat = from a in db.Tbl_R_News_Cat
                                        where a.NewsID.Equals(id)
                                        select a;
                        var qNews = (from a in db.Tbl_News
                                     where a.Id.Equals(id)
                                     select a).SingleOrDefault();

                        string path = System.IO.Path.Combine(Server.MapPath("~") + "Content/img/NewsPic/");
                        if (System.IO.File.Exists(path + qNews.Imge))
                        {
                            System.IO.File.Delete(path + qNews.Imge);
                        }
                        db.Tbl_R_News_Cat.RemoveRange(qRNewsCat.AsEnumerable());
                        db.Tbl_Comment.RemoveRange(qComment.AsEnumerable());
                        db.Tbl_News.Remove(qNews);
                        db.SaveChanges();

                        return RedirectToAction("ManagePosts");
                    }

                    else
                    {
                        return Content("Invalid request...Please try again.");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {

                return Content("خطا در ارتباط");
            }

        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    var q = (from a in db.Tbl_News
                             where a.Id.Equals(id)
                             select a).SingleOrDefault();

                    return View(q);
                }
                else
                {
                    return RedirectToAction("LogIn", "Home");
                }
            }

            catch
            {

                return Content("خطا");
            }

        }

        [HttpPost]
        public ActionResult EditPost(Tbl_News tbn, HttpPostedFileBase Pic, int id, int Type = 0)
        {
            try
            {
                if (Session["UserName"]!=null)
                {
                    if (ModelState.IsValid)
                    {
                        if (Type != 0)
                        {
                            var qRetriveNews = (from a in db.Tbl_News
                                                    where a.Id.Equals(id)
                                                    select a).SingleOrDefault();
                            if (Pic != null)
                            {
                                

                                if (Pic.ContentType == "image/jpeg")
                                {
                                    if (Pic.ContentLength >= 80000 && Pic.ContentLength <= 850000)
                                    {
                                       
                                        string namePic = qRetriveNews.Imge;
                                        string Path = System.IO.Path.Combine(Server.MapPath("~"), "Content/img/NewsPic");
                                        Pic.SaveAs(Path + namePic);
                                        qRetriveNews.Imge = namePic;
                                    }
                                    else
                                    {
                                        ViewBag.style = "Color:red";
                                        ViewBag.message = "حجم عکس مورد نظر شما باید بین 80 و 850 کیلوبایت باشد";
                                        return View();
                                    }
                                }
                                else
                                {
                                    ViewBag.style = "Color:red";
                                    ViewBag.message = "پسوند عکس مورد نظر باید jpeg باشذ";
                                    return View();
                                }
                                return View();
                            }
                            else
                            {
                                qRetriveNews.Type = tbn.Type.ToString();
                                qRetriveNews.Text = tbn.Text;
                                qRetriveNews.Title = tbn.Title;
                                qRetriveNews.HeadTitle = tbn.HeadTitle;
                                qRetriveNews.FullText = tbn.FullText;

                                db.Tbl_News.Attach(qRetriveNews);
                                db.Entry(qRetriveNews).State = System.Data.Entity.EntityState.Modified;
                                if(db.SaveChanges()>0)
                                {
                                    ViewBag.style = "Color:green";
                                    ViewBag.message = "با موفقیت ویرایش شد";
                                    return View(tbn);

                                }
                                else
                                {
                                    ViewBag.style = "Color:red;";
                                    ViewBag.Message = "تمامی فیلد را به درستی وارد کنید";
                                    return View();
                                }
                            }
                        }

                        else
                        {
                            ViewBag.style = "Color:red;";
                            ViewBag.Message = "تمامی فیلد را به درستی وارد کنید";
                            return View();
                        }

                    }
                    else
                    {
                        ViewBag.style = "Color:red;";
                        ViewBag.Message = "تمامی فیلد را به درستی وارد کنید";
                        return View();
                    }

                }
                else
                {
                    return RedirectToAction("LogIn", "Home");
                }
            }

            catch
            {

                return Content("خطا");
            }

        }

        [HttpGet]
        public ActionResult NewPost()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "home");
            }
        }

        [HttpPost]
        public ActionResult NewPost(Tbl_News t, HttpPostedFileBase Pic, int Type = 0)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn", "Home");

            //if (Pic == null)
            //{
            //    ViewBag.Style = "color:red;";
            //    ViewBag.Message = "یک عکس را انتخاب نمایید";
            //    return View();
            //}

            if (Type == 0)
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "یکی از دسته هارا انتخاب نمایید";
                return View();
            }

            var q3 = (from a in db.Tbl_Category
                      where a.Id.Equals(Type)
                      select a).SingleOrDefault();
            if (q3 == null)
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "یک دسته ی معتبر را انتخاب نمایید";
                return View();
            }


            if (ModelState.IsValid)
            {
                if (Pic != null)
                {
                    if (Pic.ContentLength >= 0 && Pic.ContentLength <= 850000)
                    {
                        if (Pic.ContentType == "image/jpeg")
                        {
                            Random rnd = new Random();
                            string PicName = (rnd.Next().ToString() + ".jpg");

                            string path = System.IO.Path.Combine(Server.MapPath("~") + "Content/img/NewsPic/");

                            Pic.SaveAs(path + PicName);
                            t.Imge = PicName;


                        }
                        else
                        {
                            ViewBag.Style = "color:red;";
                            ViewBag.Message = "فرمت تصویر باید jpg باشد";
                            return View();
                        }
                    }

                    else
                    {
                        ViewBag.Style = "color:red;";
                        ViewBag.Message = "حجم فایل بیشتر از 850 کیلوبایت است";
                        return View();
                    }
                }

                t.Date = DateTime.Now.Date;
                t.DisLike = 0;
                t.Like = 0;
                t.Visit = 0;

                db.Tbl_News.Add(t);
                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    var q = (from a in db.Tbl_News
                             orderby a.Id descending
                             select a).FirstOrDefault();

                    Tbl_R_News_Cat tcn = new Tbl_R_News_Cat();
                    tcn.NewsID = q.Id;
                    tcn.CatID = Type;
                    db.Tbl_R_News_Cat.Add(tcn);


                    if (db.SaveChanges() > 0)
                    {
                        ViewBag.Style = "color:Green;";
                        ViewBag.Message = "با موفقیت ثبت شد";
                        return View();
                    }
                    else
                    {
                        ViewBag.Style = "color:red;";
                        ViewBag.Message = "متاسفانه ثبت نشد";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Style = "color:red;";
                    ViewBag.Message = "متاسفانه ثبت نشد";
                    return View();
                }
            }
            else
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "تمامی فیلد ها را به درستی پر نمایید";
                return View();
            }

        }

        [HttpGet]
        public ActionResult DetailsPost(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    var q = (from a in db.Tbl_News
                             where a.Id.Equals(id)
                             select a).SingleOrDefault();

                    q.Date = Convert.ToDateTime(utility.ConvertToShamsi(q.Date).ToShortDateString());

                    return View(q);

                }
                else
                {
                    return RedirectToAction("Home", "LogIn");
                }
            }
            catch
            {
                return Content("خطا");
            }

        }
        [HttpGet]
        public ActionResult ManageComment(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    var q = (from a in db.Tbl_Comment
                             where a.NewsId.Equals(id)
                             select a);
                    return View(q);

                }
                else
                {
                    return RedirectToAction("Home", "LogIn");
                }

            }
            catch
            {

                return Content("خطا");
            }


        }

        [HttpGet]
        public ActionResult DetailsComment(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    var q = (from a in db.Tbl_Comment
                             where a.Id.Equals(id)
                             select a).SingleOrDefault();
                    q.Read = true;
                    db.Tbl_Comment.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View(q);

                }
                else
                {
                    return RedirectToAction("Home", "LogIn");
                }

            }
            catch
            {

                return Content("خطا");
            }
        }

        [HttpPost]
        public ActionResult DeleteComment(int id)
        {


            try
            {
                if (Session["UserName"] != null)
                {
                    var q = (from a in db.Tbl_Comment
                             where a.Id.Equals(id)
                             select a).SingleOrDefault();
                    db.Tbl_Comment.Remove(q);
                    db.SaveChanges();
                    return View();

                }
                else
                {
                    return RedirectToAction("Home", "LogIn");
                }

            }
            catch
            {

                return Content("خطا");
            }
        }

        public IQueryable<Tbl_Category> GetListCategoties()
        {

            if (Session["UserName"] == null)
                RedirectToAction("Home", "LogIn");

            if (Session["Access"].ToString() != "Admin")
                RedirectToAction("Home", "Logout");

            var q = (from a in db.Tbl_Category
                     select a);
            return q;
        }

        [HttpGet]
        public ActionResult ManageCategory()
        {
            try
            {
                return View("ManageCategory", GetListCategoties());


            }
            catch
            {
                return Content("خطا");

            }
        }

        public ActionResult DeleteCategory(int id)
        {
            try
            {
                if (Session["UserName"] != null)
                {

                    var q1 = (from a in db.Tbl_Category
                              where a.Id.Equals(id)
                              select a).SingleOrDefault();

                    var chooseParentID = from a in db.Tbl_Category
                                         where a.ParentId.Equals(id)
                                         select a;
                    //effects

                    foreach (var item in chooseParentID)
                    {
                        item.ParentId = 1;
                        db.Tbl_Category.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();


                    db.Tbl_Category.Remove(q1);

                    if (db.SaveChanges() > 0)
                    {
                        ViewBag.style = "Color:Green;";
                        ViewBag.message = "با موفقیت حذف شد";
                        return View("ManageCategory", GetListCategoties());
                    }
                    else
                    {
                        ViewBag.style = "Color:red;";
                        ViewBag.message = "متاسفانه حذف نشد";
                        return View("ManageCategory", GetListCategoties());
                    }
                }
                else
                {
                    return RedirectToAction("Home", "LogIn");
                }


            }
            catch
            {
                return Content("خطا");

            }
        }
        [HttpGet]
        public ActionResult CreatCategory()
        {

            if (Session["UserName"] != null)
            {

                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreatCategory(Tbl_Category Cat, HttpPostedFileBase Pic)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn", "Home");

            if (Cat.Id == 0)
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "یکی از دسته هارا انتخاب نمایید";
                return View();
            }

            var q3 = (from a in db.Tbl_Category
                      where a.Id.Equals(Cat.Id)
                      select a).SingleOrDefault();
            if (q3 == null)
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "یک دسته ی معتبر را انتخاب نمایید";
                return View();
            }

            if (ModelState.IsValid)
            {
                Tbl_Category c = new Tbl_Category();
                if (Pic != null)
                {
                    if (Pic.ContentLength >= 80000 && Pic.ContentLength <= 850000)
                    {
                        if (Pic.ContentType == "image/jpeg")
                        {

                            Random rnd = new Random();
                            string PicName = (rnd.Next().ToString() + ".jpg");

                            string path = System.IO.Path.Combine(Server.MapPath("~") + "Content/img/CatPic/");

                            Pic.SaveAs(path + PicName);
                            c.Image = PicName;

                        }
                        else
                        {
                            ViewBag.Style = "color:red;";
                            ViewBag.Message = "فرمت تصویر باید jpg باشد";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Style = "color:red;";
                        ViewBag.Message = "حجم فایل بیشتر از 512 کیلوبایت است";
                        return View();
                    }
                }

                //insert Category
                c.ParentId = Cat.Id;
                c.Title = Cat.Title;
                db.Tbl_Category.Add(c);
                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Style = "color:green;";
                    ViewBag.Message = "با موفقیت ثبت شد";
                    return View();
                }
                else
                {
                    ViewBag.Style = "color:red;";
                    ViewBag.Message = "متاسفانه ثبت نشد";
                    return View();
                }
                //end

            }
            else
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "تمامی فیلد ها را به درستی پر نمایید";
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditCategory(int Id)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn", "Home");

            try
            {
                var q = (from a in db.Tbl_Category
                         where a.Id.Equals(Id)
                         select a).SingleOrDefault();
                return View(q);

            }
            catch
            {

                return Content("خطا");
            }
        }

        [HttpPost]
        public ActionResult EditCategory(Tbl_Category Cat, HttpPostedFileBase Pic)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn", "Home");


            if (Cat.Id == 0)
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "یکی از دسته هارا انتخاب نمایید";
                return View();
            }

            var q3 = (from a in db.Tbl_Category
                      where a.Id.Equals(Cat.Id)
                      select a).SingleOrDefault();
            if (q3 == null)
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "یک دسته ی معتبر را انتخاب نمایید";
                return View();
            }

            if (Pic != null)
            {
                if (Pic.ContentLength >= 80000 && Pic.ContentLength <= 850000)
                {
                    if (Pic.ContentType == "image/jpeg")
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~") + "Content/img/NewsPic/");
                        Pic.SaveAs(path + Cat.Image);
                    }
                    else
                    {
                        ViewBag.Style = "color:red;";
                        ViewBag.Message = "فرمت تصویر باید jpg باشد";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Style = "color:red;";
                    ViewBag.Message = "حجم فایل بیشتر از 512 کیلوبایت است";
                    return View();
                }
            }

            if (ModelState.IsValid)
            {

                var e = (from a in db.Tbl_Category
                         where a.Id.Equals(Cat.Id)
                         select a).SingleOrDefault();
                
                e.ParentId = Cat.Id;
                e.Title = Cat.Title;
                db.Tbl_Category.Attach(e);
                db.Entry(e).State = System.Data.Entity.EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Style = "color:green;";
                    ViewBag.Message = "با موفقیت ثبت شد";
                    return View();
                }
                else
                {
                    ViewBag.Style = "color:red;";
                    ViewBag.Message = "متاسفانه ثبت نشد";
                    return View();
                }
            }
            else
            {
                ViewBag.Style = "color:red;";
                ViewBag.Message = "تمامی فیلد ها را به درستی پر نمایید";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ManageContacts()
        {
            try
            {
                if (Session["UserName"] == null)
                    return RedirectToAction("LogIn", "Admin");

                var q = from a in db.Tbl_ContactUs select a;
                return View(q);
            }
            catch
            {
                return Content("خطا");
            }
        }

        public ActionResult DetailsContact(int Id)
        {
            try
            {
                if (Session["UserName"] == null)
                    return RedirectToAction("LogIn", "Admin");


                var q = (from a in db.Tbl_ContactUs where a.Id.Equals(Id) select a).SingleOrDefault();
                return View(q);
            }
            catch
            {
                return Content("خطا");
            }
        }

        public ActionResult DeleteContact(int Id)
        {
            try
            {
                if (Session["UserName"] == null)
                    return RedirectToAction("LogIn", "Home");

                var q = (from a in db.Tbl_ContactUs
                         where a.Id.Equals(Id)
                         select a).SingleOrDefault();

                db.Tbl_ContactUs.Remove(q);

                var q2 = from a in db.Tbl_ContactUs
                         orderby a.Id
                         select a;

                if (db.SaveChanges() > 0)
                {

                    ViewBag.message = "با موفقیت ثبت شد";
                    ViewBag.style = "Color:Green;";
                    return View("ManageContacts", q2);
                }
                else
                {
                    ViewBag.message = "متاسفانه با  موفقیت ثبت نشد";
                    ViewBag.style = "Red";
                    return View("ManageContacts", q2);
                }

            }
            catch
            {

                return Content("خطا");
            }
            //try
            //{
            //    if (Session["UserName"] == null)
            //        return RedirectToAction("LogIn", "Admin");

            //    if (!Session["Access"].Equals("Admin"))
            //        return RedirectToAction("Logout", "Admin");

            //    var q = (from a in db.Tbl_ContactUs where a.Id.Equals(Id) select a).SingleOrDefault();
            //    db.Tbl_ContactUs.Remove(q);

            //    db.SaveChanges();
            //    var q2 = from a in db.Tbl_ContactUs
            //             orderby a.Id
            //             select a;

            //    if (Convert.ToBoolean(db.SaveChanges() > 0))
            //    {
            //        ViewBag.style = "Color:Green;";
            //        ViewBag.message = "با موفقیت حذف شد";
            //        return View("ManageContacts", q2);
            //    }
            //    else
            //    {
            //        ViewBag.style = "Color:red;";
            //        ViewBag.message = "متاسفانه حذف نشد";
            //        return View("ManageContacts", q2);
            //    }

            //}
            //catch
            //{
            //    return Content("خطا");
            //}
        }

        [HttpGet]
        public ActionResult Settings()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn", "Home");
            if (Session["Access"].ToString() != "Admin")
                return RedirectToAction("Logout", "Admin");

            var q = (from a in db.Tbl_Setting select a).FirstOrDefault();
            return View(q);
        }

        [HttpPost]
        public ActionResult Settings(Tbl_Setting tbl)
        {
            try
            {
                if (Session["UserName"] == null)
                    return RedirectToAction("LogIn", "Home");
                if (Session["Access"].ToString() != "Admin")
                    return RedirectToAction("Logout", "Admin");

                var q = (from a in db.Tbl_Setting select a).FirstOrDefault();
                q.CountNewer = tbl.CountNewer;
                q.CountNewsInPage = tbl.CountNewsInPage;
                q.CountOfMemo = tbl.CountOfMemo;
                q.CountPopular = tbl.CountPopular;
                q.DisableComment = tbl.DisableComment;
                q.Discription = tbl.Discription;
                q.MaxUploadSize = tbl.MaxUploadSize;
                q.MetaTage = tbl.MetaTage;
                q.ShowConfComm = tbl.ShowConfComm;
                q.Title = tbl.Title;

                db.Tbl_Setting.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    ViewBag.Style = "color:Green;";
                    ViewBag.Message = "با موفقیت ثبت شد";
                    return View(q);
                }
                else
                {
                    ViewBag.Style = "color:red;";
                    ViewBag.Message = "متاسفانه با موفقیت ثبت نشد";
                    return View(q);
                }
            }
            catch
            {
                return Content("خطا");
            }
        }
    }
}

