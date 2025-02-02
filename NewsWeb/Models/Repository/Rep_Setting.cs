using NewsWeb.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWeb.Models.Repository
{
    public class Rep_Setting
    {
        DataBaseNews db = new DataBaseNews(); 


        public Tbl_Setting Tools()
        {

            try
            {
                var qGetSetting = (from a in db.Tbl_Setting
                                  select a).FirstOrDefault();
                return qGetSetting;

            }
            catch
            {

                return null;
            }
        }



    }
}