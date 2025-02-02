using NewsWeb.Models.Domain;
using NewsWeb.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc;
using PagedList;
using CaptchaMvc.HtmlHelpers;
namespace NewsWeb.Controllers
{
    public class HomeController : Controller
    {
        Rep_News rNews = new Rep_News();
        Rep_Comment rcomment = new Rep_Comment();
        DataBaseNews db = new DataBaseNews();
        // GET: /Home/
        public ActionResult Index(int? page)
        {
            var q = rNews.GetNews();
            var pageNumber = page ?? 1;
            if (q != null)
            {
                var onePageOfpost = q.ToPagedList(pageNumber, 7);
                ViewBag.OnePageOfProducts = onePageOfpost;
                return View(onePageOfpost.ToList());
            }
            return View(q);
        }

        public ActionResult News(int Id)
        {
            rNews.SetVisit(Id);
            return View(rNews.getDetails(Id));
        }

        public ActionResult NewsLike(int id)
        {
            //Like Clike = new Like();

            //Clike.Likes = rNews.Like(id, Request.UserHostAddress.ToString());


            //var q = (from a in db.Tbl_News where a.Id.Equals(id) select a).SingleOrDefault();
            //Clike.Dislikes = q.DisLike;
            //return Json(Clike);

            //int like = rNews.Like(id, Request.UserHostAddress.ToString());
            //return Json(like);
            rNews.Like(id, Request.UserHostAddress.ToString());
            var q = (from a in db.Tbl_News where a.Id.Equals(id) select a).SingleOrDefault();
            int[] i = new int[2];
            i[0] = q.Like;
            i[1] = q.DisLike;

            return Json(i);

            //return RedirectToAction("News", rNews.getDetails(id));
        }

        public ActionResult NewsDisLike(int id)
        {
            rNews.DisLike(id, Request.UserHostAddress.ToString());
            var q = (from a in db.Tbl_News where a.Id.Equals(id) select a).SingleOrDefault();
            int[] i = new int[2];
            i[0] = q.Like;
            i[1] = q.DisLike;

            return Json(i);
        }

        public ActionResult CommentLike(int Id)
        {
            int like = rcomment.Like(Id);
            return Json(like);
        }

        public ActionResult CommentDisLike(int Id)
        {
            int dislike = rcomment.DisLike(Id);
            return Json(dislike);
        }

        public ActionResult InsertComment(Tbl_Comment TblComment)
        {
            try
            {
                TblComment.Confirm = false;
                TblComment.Dislike = 0;
                TblComment.Like = 0;
                TblComment.ParentId = 1;
                TblComment.Read = false;
                TblComment.Ip = Request.UserHostAddress;
                TblComment.Date = DateTime.Now.Date;
                int Id = TblComment.NewsId;
                db.Tbl_Comment.Add(TblComment);
                db.SaveChanges();

                //var q = from a in db.Tbl_Comment
                //        where a.NewsId.Equals(Id)
                //        select a;
                return View("News", rNews.getDetails(TblComment.NewsId));

            }
            catch
            {

                return Content("Have an Error");
            }
        }

        public ActionResult AnsComment(int NewsId, int ParentId)
        {
            Tbl_Comment t = new Tbl_Comment();
            t.NewsId = NewsId;
            t.ParentId = ParentId;
            return PartialView("P_AnsComment", t);
        }

        public ActionResult InsertAnsComment(Tbl_Comment TblComment)
        {
            try
            {
                TblComment.Confirm = false;
                TblComment.Dislike = 0;
                TblComment.Like = 0;
                TblComment.Read = false;
                TblComment.Ip = Request.UserHostAddress;
                TblComment.Date = DateTime.Now.Date;

                db.Tbl_Comment.Add(TblComment);
                db.SaveChanges();
                return View("News", rNews.getDetails(TblComment.NewsId));

            }
            catch
            {

                return Content("Have an Error");
            }
        }

        public ActionResult Category(int Id, int? page)
        {
            try
            {
                var q = (from a in db.Tbl_Category
                         join b in db.Tbl_R_News_Cat on a.Id equals b.CatID
                         join c in db.Tbl_News on b.NewsID equals c.Id
                         where a.Id.Equals(Id)
                         select c).ToList();
                //
                var pageNumber = page ?? 1;
                var onePageOfpost = q.ToPagedList(pageNumber, 2);
                ViewBag.OnePageOfProducts = onePageOfpost;
                //
                return View("Index", onePageOfpost.ToList());
            }
            catch
            {
                return Content("Have an error )-: !");

            }
        }

        public ActionResult Searh(string txtSearch)
        {
            try
            {
                var q = db.Tbl_News.Select(a => a).Where(a => a.Title.Contains(txtSearch));
                return View("Index", q.ToList());
            }
            catch
            {
                return Content("Have an error in Your Search ! )-:");

            }

        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(Tbl_ContactUs Tbl_ContactUs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tbl_ContactUs.Add(Tbl_ContactUs);

                    if (db.SaveChanges() > 0)
                    {
                        ViewBag.style = "color:green";
                        ViewBag.message = "نظر شما با موفقیت ارسال شد";
                        return View();
                    }
                    else
                    {
                        ViewBag.style = "color:red";
                        ViewBag.message = "نظر شما با موفقیت ارسال نشد";
                        return View();
                    }
                }
                else
                {

                    ViewBag.style = "color:red";
                    ViewBag.message = "تمامی فیلد را به صورت صحیح پر نمایید";
                    return View();
                }

            }
            catch
            {
                ViewBag.style = "color:red";
                ViewBag.message = "خطا در ارسال";
                return View();
            }
        }
        ////33
        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult logIn()
        {
            if (Session["UserName"] == null)
                return View();
            else
                return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult logIn(string Txt_UserName, string Txt_Psw)
        {


            if (this.IsCaptchaValid("error"))
            {

                if (Txt_UserName != "" && Txt_Psw != "" && Txt_UserName != null && Txt_Psw != null)
                {
                    var q = (from u in db.Tbl_Users where u.UserName.Equals(Txt_UserName) && u.Psw.Equals(Txt_Psw) select u).SingleOrDefault();
                    if (q != null)
                    {
                        Session["UserName"] = q.UserName;
                        Session["Access"] = q.Access.ToString();
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.style = "color:red";
                        ViewBag.Message = "نام کاربری یا کلمه عبور اشتباه است";
                        return View();
                    }
                }
                else
                {
                    ViewBag.style = "color:red";
                    ViewBag.Message = "نام کاربری یا کلمه عبور اشتباه است";
                    return View();
                }

            }
            else
            {
                ViewBag.style = "color:red";
                ViewBag.Message = "کد تصویر را به درستی وارد کنید";
                return View();
            }
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Tbl_Users TblUser, HttpPostedFileBase PicUser)
        {
            try
            {
                #region registration code
                TblUser.Access = "User";
                TblUser.DateRegistration = DateTime.Now.Date;
                TblUser.Enable = false;
                if (ModelState.IsValid)
                {
                    if (PicUser != null)
                    {
                        if (PicUser.ContentType == "image/jpeg")
                        {
                            if (PicUser.ContentLength <= 500000 && PicUser.ContentLength >= 80000)
                            {
                                string PicName = TblUser.UserName.ToString() + ".jpeg";
                                string path = System.IO.Path.Combine(Server.MapPath(@"~/Content/img/UserPic/"));
                                PicUser.SaveAs(path + PicName);

                                TblUser.Image = PicName;
                                db.Tbl_Users.Add(TblUser);
                                if (db.SaveChanges() > 0)
                                {
                                    ViewBag.style = "color:green";
                                    ViewBag.Message = "ثبت نام با موفقیت صورت گرفت";
                                    return View();
                                }
                                else
                                {
                                    ViewBag.style = "color:red";
                                    ViewBag.Message = "ثبت نام با موفقیت صورت نگرفت";
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.style = "color:red";
                                ViewBag.Message = "حجم تصویر باید بین 100 تا 500 کیلوبایت باشد";
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.style = "color:red";
                            ViewBag.Message = "قالب تصویر باید jpeg  باشد";
                            return View();
                        }
                    }
                    else
                    {
                        TblUser.Image = "Default.jpeg";
                    }
                    return View();
                }
                else
                {
                    ViewBag.style = "color:red";
                    ViewBag.Message = "تمامی فیلد را پر کنید";
                    return View();
                }
                #endregion
            }
            catch
            {
                return Content("Error in connection...please try again.");

            }


        }

        [HttpPost]
        public ActionResult DublicateUserName(string username)
        {


            try
            {
                var q = db.Tbl_Users.Where(a => a.UserName.Equals(username));

                if (q.Count() == 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }

            }
            catch
            {
                return Json(false);

            }

        }

        public ActionResult DublicateEmail(string email)
        {

            try
            {
                var q = db.Tbl_Users.Where(a => a.Email.Equals(email));
                if (q.Count() == 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch
            {

                return Json(false);
            }
        }

        [HttpGet]
        public ActionResult Recovery()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Recovery(string Txt_UserName, string Txt_Email)
        {
            try
            {
                if (Txt_UserName != "" && Txt_Email != "")
                {
                    var q = (from a in db.Tbl_Users
                             where a.UserName.Equals(Txt_UserName) && a.Email.Equals(Txt_Email)
                             select a).SingleOrDefault();
                }
                else
                {
                    ViewBag.style = "Color:red";
                    ViewBag.Message = "خطایی رخ داده است";
                    return View();
                }
            }
            catch
            {

                ViewBag.style = "Color:red";
                ViewBag.Message = "خطایی رخ داده است";
            }
            return View();
        }


    }
}