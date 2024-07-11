using CRUD_ADO.NET.Services;
using Chart_of_account.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

public class UserController : Controller
{
    private readonly UserDAL _userDAL;
    private object _userRepository;

    public object FormsAuthentication { get; private set; }
    public UserController(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("cs");
        _userDAL = new UserDAL(connectionString);
    }

    [HttpGet]
    public ActionResult CreateAccount()
    {
        return View();
    }
    [HttpPost]
    public ActionResult CreateAccount(User_Login login)
    {
        if (_userDAL.CreateAccount(login))
        {
            TempData["Message"] = "Account Created Successfully";
            return RedirectToAction("SignIn", "User");
        }
        else
        {
            TempData["InsertMsgee"] = "Username and password already exist";
            return RedirectToAction("CreateAccount");
        }
    }

    [HttpGet]
    public ActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public ActionResult SignIn(User_Login login)
    {
        bool isValidUser = _userDAL.SignIn(login);
        if (isValidUser)
        {
            return RedirectToAction("GetList");
        }
        else
        {
            TempData["InsertMsge"] = "Invalid Username and Password";
            return RedirectToAction("SignIn");
        }
    }
    [HttpGet]
    public ActionResult GetPasswordByEmail()
    {
        return View();
    }
    [HttpPost]
    public ActionResult GetPasswordByEmail(User_Login login)
    {

        bool check = _userDAL.GetPasswordByEmail(login);
        if (check)
        {
            TempData["success"] = "Your email has been delivered with the password.";
            //password sent via email
            return   RedirectToAction("SignIn", "User");
        }
        else
        {
            return RedirectToAction("SignIn", "User");
        }
    }

    [HttpGet]

    public ActionResult CreatetheChart()
    {
        var data = _userDAL.ShowList();
        ViewBag.List = new SelectList(data, "AccountClassId", "AccountClassTitle");
        return View();
    }
    [HttpPost]
    public ActionResult CreatetheChart(Combine_Model model)
    {
        if(_userDAL.CreatetheChart(model))
        {
            return RedirectToAction("GetList");
        }
        else
        {
            TempData["Message"] = "Account Not Save";
            
        }
        return View();
    }
    [HttpGet]

    public IActionResult GetList()
    {
        List<Combine_Model> users = _userDAL.GetList();
        return View(users);
    }
    [HttpGet]

    public ActionResult CreatetheVoucher()
    {
        var voucherData = _userDAL.VoucherList();
        ViewBag.VoucherList = new SelectList(voucherData, "VoucherTypeId", "VoucherTypeTitle");
        var accountData = _userDAL.AccountNameList();
        ViewBag.AccountList = new SelectList(accountData, "AccountClassId", "AccountTitle");

        return View();
    }
    [HttpPost]
    public ActionResult CreatetheVoucher(CombineVoucherModel model)
    {
        if (_userDAL.CreatetheVoucher(model))
        {
            return RedirectToAction("GetListVoucher");
        }
        else
        {
            TempData["Message"] = "Account Not Save";

        }
        return View();
    }
    [HttpGet]

    public IActionResult GetListVoucher()
    {
        List<CombineVoucherModel> users = _userDAL.GetListVoucher();
        return View(users);
    }
}