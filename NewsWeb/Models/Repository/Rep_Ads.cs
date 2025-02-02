using NewsWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWeb.Models.Repository
{
    public class Rep_Ads
    {
        DataBaseNews db=new DataBaseNews();
        public List<Tbl_Adv> getAds()
        {
            DateTime dt=DateTime.Now.Date;
            try
            {
                var q = (from a in db.Tbl_Adv
                         where a.Enable.Equals(true) &&  dt>= a.DateStart && dt<=a.DateEnd
                         orderby a.Id descending
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