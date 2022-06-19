using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcSample.Models;
using Npgsql;
using Dapper;

namespace MvcSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Home/{id}")]
        public IActionResult Index(string id)
        {
            Console.WriteLine("Homeページ表示");
            Console.WriteLine(id);
            ViewData["loginId"] = id;
            ViewData["address"] = "東京都新宿区";
            ViewData["date"] = "2022-04-12";
            return View("Index");
        }

        [HttpPost]
        public bool Register(string userId,string address, DateTime birthday)
        {
            Console.WriteLine(userId);
            Console.WriteLine(address);
            Console.WriteLine(birthday);
            var returnResut = true;
            string conn = @"Server=localhost;Port=5433;User Id=postgres;Password=postgres;Database=postgres";
            Console.WriteLine("DB接続開始");
            Console.WriteLine(conn);
            using (NpgsqlConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();
                // m_userというテーブルのレコード数を取得
                Console.WriteLine("DB接続開始１");
            }
            return returnResut;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
