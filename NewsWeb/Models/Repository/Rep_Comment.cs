using NewsWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWeb.Models.Repository
{
    public class Rep_Comment
    {
        DataBaseNews db = new DataBaseNews();
        Rep_Setting rSetting = new Rep_Setting();
        public List<Tbl_Comment> getCommrnts(int NewsID)
        {
            try
            {
                if (rSetting.Tools().ShowConfComm == true)
                {
                    var q = (from a in db.Tbl_Comment
                             where a.NewsId.Equals(NewsID)
                             select a).ToList();
                    return q;
                }

                else
                {

                    var q = (from a in db.Tbl_Comment
                             where a.NewsId.Equals(NewsID) && a.Confirm==true
                             select a).ToList();
                    return q;
                }

            }
            catch
            {

                return null;
            }
        }

        public int Like(int Id)
        {

            var q = (from a in db.Tbl_Comment
                     where a.Id.Equals(Id)
                     select a).SingleOrDefault();
            try
            {

                q.Like++;
                db.Tbl_Comment.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return q.Like;
            }
            catch
            {
                return q.Like--;
            }
        }

        public int DisLike(int Id)
        {

            var q = (from a in db.Tbl_Comment
                     where a.Id.Equals(Id)
                     select a).SingleOrDefault();
            try
            {

                q.Dislike++;
                db.Tbl_Comment.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                if (db.SaveChanges() > 0)
                {
                    return q.Dislike;
                }
                else
                {
                    return q.Dislike--;
                }
            }
            catch
            {
                return q.Dislike;
            }
        }

    }
}