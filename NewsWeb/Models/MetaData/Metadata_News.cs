using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWeb.Models.Domain
{
    internal class Metadata_News
    { 
        public int Id { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; }   

        [DisplayName("روتیتر")]
        public string HeadTitle { get; set; }

        [DisplayName("چکیده")]
        public string Text { get; set; }

        
        [DisplayName("متن خبر")]
        [AllowHtml]
        public string FullText { get; set; }

        [DisplayName("عکس")]
        public string Imge { get; set; }

        [DisplayName("بازدید")]
        public long Visit { get; set; }

        [DisplayName("پسندیدن")]
        public int Like { get; set; }
        [DisplayName("نپسندیدن")]
        public int DisLike { get; set; }
        [DisplayName("انتشار")]
        public System.DateTime Date { get; set; }
        [DisplayName("نوع خبر")]
        public string Type { get; set; }
    }

    [MetadataType(typeof(Metadata_News))]
    public partial class Tbl_News
    {

    }
}