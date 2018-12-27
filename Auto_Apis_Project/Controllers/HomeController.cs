using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auto_Apis_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public void CreateFile()
        {
            var _letVal = Request.Form["letVal"];
            var _actionUrl = Request.Form["actionUrl"];
            //"process.env.VUE_APP_Config_login"
            Tool_Class.OutputFile(_letVal, _actionUrl);
            string script = "<script> alert('生成成功');location.href='/'</script>";
            Response.Write(script);
        }
    }
}