using NewsWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWeb.Models.Repository
{
    public class Rep_News
    {
        DataBaseNews db = new DataBaseNews();
        Rep_Setting rsetting = new Rep_Setting();
        public List<Tbl_News> GetNews()
        {
            try
            {
                //List<Tbl_News> qgetNews = (from a in db.Tbl_News
                //                           where a.Type.Equals("News")
                //                           select a).OrderByDescending(s => s.Id).Skip(0).Take(rsetting.Tools().CountNewsInPage).ToList();
                List<Tbl_News> qgetNews = (from a in db.Tbl_News select a).OrderByDescending(s => s.Id).Select(a => a).ToList();

                return qgetNews;
            }
            catch
            {

                return null;
            }
        }


        public IEnumerable<Tbl_News> GetSpNews()
        {
            try
            {
                IEnumerable<Tbl_News> qgetNews = (from a in db.Tbl_News
                                           where a.Type.Equals("Sp")
                                           select a).OrderByDescending(s => s.Id).Skip(0).Take(rsetting.Tools().CountPopular);

                return qgetNews;
            }
            catch
            {

                return null;
            }
        }


        public List<Tbl_News> GetSpMemoNews()
        {
            try
            {
                List<Tbl_News> qgetNews = (from a in db.Tbl_News
                                           where a.Type.Equals("Memo")
                                           select a).OrderByDescending(s => s.Id).Skip(0).Take(rsetting.Tools().CountOfMemo).ToList();

                return qgetNews;
            }
            catch
            {

                return null;
            }
        }


        public List<Tbl_News> GetNewsByVisit()
        {

            try
            {
                var q = db.Tbl_News.Select(a => a).OrderByDescending(a => a.Visit).Skip(0).Take(10).ToList();

                return q;
            }
            catch
            {

                return null;
            }

        }

        public List<Tbl_News> GetNewsByLike()
        {

            try
            {
                var q = (from a in db.Tbl_News
                         orderby (a.Like - a.DisLike) descending
                         select a).Skip(0).Take(4);

                return q.ToList();
            }
            catch
            {

                return null;
            }

        }

        public List<Tbl_News> GetNewsNewer()
        {

            try
            {
                var q = (from a in db.Tbl_News
                         select a).OrderByDescending(s => s.Id).Skip(0).Take(10);

                return q.ToList();
            }
            catch
            {

                return null;
            }

        }


        public List<Tbl_News> GetNewsNewerSlide()
        {

            try
            {
                var q = (from a in db.Tbl_News
                         select a).OrderByDescending(s => s.Id).Skip(0).Take(5);

                return q.ToList();
            }
            catch
            {

                return null;
            }

        }

        public Tbl_News getDetails(int Id)
        {
            try
            {
                var q = (from a in db.Tbl_News
                         where a.Id.Equals(Id)
                         select a).FirstOrDefault();

                return q;
            }
            catch
            {

                return null;
            }
        }

        public int Like(int Id, string Ip)
        {


            try
            {
                var q = (from a in db.Tbl_News
                         where a.Id.Equals(Id)
                         select a).SingleOrDefault();
                var q2 = from a in db.Tbl_News_Likes
                        where a.NewsId.Equals(Id) && a.UserIp.Equals(Ip)
                        select a;

                var q3 = (from a in db.Tbl_News_Likes
                         where a.NewsId.Equals(Id) && a.UserIp.Equals(Ip) && a.Dislike.Equals(true)
                         select a).SingleOrDefault();


                if (q2.Count()==0)
                {
                    q.Like++;
                    db.Tbl_News.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {
                        Tbl_News_Likes nl = new Tbl_News_Likes();
                        nl.NewsId = Id;
                        nl.UserIp = Ip;
                        nl.Like = true;
                        nl.Dislike = false;

                        db.Tbl_News_Likes.Add(nl);

                        if (db.SaveChanges() > 0)
                        {
                            return q.Like;
                        }
                        else
                        {
                            return q.Like--;
                        }

                    }
                    else
                    {
                        return q.Like--;
                    }
                }
                    //
                if (q3!=null)
                {
                    q.Like++;
                    q.DisLike--;
                    db.Tbl_News.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {
                        q3.NewsId = Id;
                        q3.UserIp = Ip;
                        q3.Like = true;
                        q3.Dislike = false;

                        db.Tbl_News_Likes.Attach(q3);
                        db.Entry(q3).State = System.Data.Entity.EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {
                            return q.Like;
                        }
                        else
                        {
                            return q.Like--;
                        }

                    }
                    else
                    {
                        return q.Like--;
                    }
                }
                    //
                else
                {
                    return q.Like;
                }

            }
            catch
            {
                return -1;
            }
        }

        public int DisLike(int Id, string Ip)
        {


            try
            {
                var q = (from a in db.Tbl_News
                         where a.Id.Equals(Id)
                         select a).SingleOrDefault();
                var q2 = from a in db.Tbl_News_Likes
                         where a.NewsId.Equals(Id) && a.UserIp.Equals(Ip)
                         select a;

                var q3 = (from a in db.Tbl_News_Likes
                         where a.NewsId.Equals(Id) && a.UserIp.Equals(Ip) && a.Like.Equals(true)
                         select a).SingleOrDefault();


                if (q2.Count() == 0)
                {
                    q.DisLike++;
                    db.Tbl_News.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {
                        Tbl_News_Likes nl = new Tbl_News_Likes();
                        nl.NewsId = Id;
                        nl.UserIp = Ip;
                        nl.Dislike = true;
                        nl.Like = false;

                        db.Tbl_News_Likes.Add(nl);

                        if (db.SaveChanges() > 0)
                        {
                            return q.DisLike;
                        }
                        else
                        {
                            return q.DisLike--;
                        }

                    }
                    else
                    {
                        return q.Like--;
                    }
                }
                //
                if (q3!=null)
                {
                    q.DisLike++;
                    q.Like--;
                    db.Tbl_News.Attach(q);
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {
                        q3.NewsId = Id;
                        q3.UserIp = Ip;
                        q3.Dislike = true;
                        q3.Like = false;

                        db.Tbl_News_Likes.Attach(q3);
                        db.Entry(q3).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            return q.DisLike;
                        }
                        else
                        {
                            return q.DisLike--;
                        }

                    }
                    else
                    {
                        return q.DisLike--;
                    }
                }
                //
                else
                {
                    return q.DisLike;
                }

            }
            catch
            {
                return -1;
            }
        }

        public bool UniqueUserLike(int Id, string Ip)
        {
            try
            {
                var q = from a in db.Tbl_News_Likes
                        where a.NewsId.Equals(Id) && a.UserIp.Equals(Ip) && a.Like.Equals(true)
                        select a;
                if (q.Count() == 0)
                    return true;
                else
                    return false;
            }
            catch
            {

                return false;
            }
        }

        public void SetVisit(int Id)
        {
            var q = (from a in db.Tbl_News
                     where a.Id.Equals(Id)
                     select a).SingleOrDefault();
            q.Visit++;
            db.Tbl_News.Attach(q);
            db.Entry(q).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

    }
}