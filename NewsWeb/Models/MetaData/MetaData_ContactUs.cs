using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWeb.Models.Domain
{
    internal class MetaData_ContactUs
    {
        public int Id { get; set; }

        [DisplayName("نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "تعداد کاراکتر بین 3 تا 20 است")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [DisplayName("ایمیل")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",ErrorMessage="قالب ایمیل را به درستی وارد کنید")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "تعداد حروف وارد شده استاندارد نمی باشد")]
        [DisplayName("متن پیام")]
        public string Text { get; set; }
    }

    [MetadataType(typeof(MetaData_ContactUs))]
    public partial class Tbl_ContactUs
    {

    }
}