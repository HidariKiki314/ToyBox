using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using MvcSample.Models;
using MvcSample.Config;

namespace sampleMVC01.Controllers
{
    public class LoginController : Controller
    {
        DbModule bdModule = new DbModule();
        public IActionResult Index()
        {
            ViewData["LoginMessage"] = "ログインIDとパスワードを入力して下さい";
            return View();
        }

        [HttpPost]
        public bool Form(string LoginId, string LoginPassword)
        {
            bool returnResut = true;
            Console.WriteLine("ID:"+LoginId+",PASS:"+LoginPassword);
            //string conn = @"Server=localhost;Port=5433;User Id=postgres;Password=postgres;Database=postgres";
            string conn = DbModule.dbConnection;
            Console.WriteLine("DB接続開始");
            Console.WriteLine(conn);
            using (NpgsqlConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();
                // m_userというテーブルのレコード数を取得
                Console.WriteLine("DB接続開始");
                /*var count = connection.Query<int>("SELECT COUNT(*) FROM m_user").First();
                Console.WriteLine(count);*/
                string  query = "SELECT * FROM m_user WHERE id = @id AND password = @password;";
                var count = connection.Query<MUserModel>(query,new{ id = LoginId, password = LoginPassword });
                Console.WriteLine(count);
                foreach(var i in count)
                {
                    Console.WriteLine(i);
                    string asdfghjkl = i.id;
                    string zxcvbnm = i.password;
                    Console.WriteLine(asdfghjkl);
                    Console.WriteLine(zxcvbnm);
                }
                string  query1 = "SELECT id FROM m_user WHERE id = @id AND password = @password;";
                var count1 = connection.Query<string>(query1,new{ id = LoginId, password = LoginPassword }).First();
                Console.WriteLine(count1);
                /*foreach (var p in count)
                {
                    Console.WriteLine($"ID: {p.LoginId}  pass: {p.LoginPassword}");
                }*/
            }
            return returnResut;
        }
    }
}
