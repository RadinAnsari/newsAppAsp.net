using NewsWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWeb.Models.Repository
{
    public class Rep_Category
    {
        DataBaseNews db = new DataBaseNews();
        public List<Tbl_Category> get13NewsCat()
        {
            try
            {
                var q = (from a in db.Tbl_Category
                         where a.Id!=1 && a.ParentId.Equals(1)
                         orderby a.Id descending
                         select a).Take(13).ToList();
                return q;
            }
            catch
            {

                return null;
            }
        }

        public List<Tbl_Category> getAllCategory()
        {
            try
            {
                var q = (from a in db.Tbl_Category
                         where a.Id != 1
                         select a).ToList();
                return q;
            }
            catch
            {

                return null;
            }
        }


    }
}