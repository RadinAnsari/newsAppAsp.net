using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWeb.Models.Domain
{
    public class MetaData_Registration
    {

        internal int Id { get; set; }
        [DisplayName("نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [Remote("DublicateUserName", "Home", ErrorMessage = "نام کاربری تکرای است", HttpMethod = "POST")]
        public string UserName { get; set; }

        [DisplayName("رمز عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [DataType(DataType.Password)]
        public string Psw { get; set; }

        [DisplayName("ایمیل")]
        [Remote("DublicateEmail", "Home", ErrorMessage = "ایمیل تکراری", HttpMethod = "POST")]
        [DataType(DataType.EmailAddress,ErrorMessage="ایمیل را به درستی وارد کنید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        public string Email { get; set; }

        public string Access { get; set; }
        [DisplayName("توضیحات")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "حداکثر 255 کاراکتر را وارد  کنید")]
        public string Discription { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [DisplayName("نام")]
        public string Name { get; set; }
        public string Image { get; set; }
        [DisplayName("کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        public string Ncode { get; set; }
        [DisplayName("شماره تلفن")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نباید خالی باشد")]
        [StringLength(11,MinimumLength=11,ErrorMessage="شماره مبایل را به درستی وارد کنید")]
        public string Mobnumber { get; set; }
        public bool Enable { get; set; }
        public System.DateTime DateRegistration { get; set; }
    }

    [MetadataType(typeof(MetaData_Registration))]
    public partial class Tbl_Users
    { }
}