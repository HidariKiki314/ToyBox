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
using MvcSample.Config;

namespace MvcSample.Controllers
{
    public class RegisterController : Controller
    {
        private readonly String selectUserInfoGetBoolQuery = "SELECT delete_flag FROM t_user_info WHERE user_id = @userId";
        private readonly String insertQuery = "insert into t_user_info (user_id,address_id,birthday,delete_flag) values (@user_id,@address_id,@birthday,@delete_flag)";
        private readonly String selectAddressAllQuery = "SELECT address_name FROM m_address";
        private readonly String selectAddressIdGetQuery = "SELECT address_id FROM m_address WHERE address_name = @address_name";
        private readonly String updateTUserInfoQuery = "UPDATE t_user_info SET address_id = @addressId, birthday = @birthday,delete_flag = @deleteFlag WHERE user_id = @userId";

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Register/{id}")]
        public IActionResult Index(string id)
        {
            Console.WriteLine("Registerページ表示");
            // addressのリスト取得
            //string conn = @"Server=localhost;Port=5433;User Id=postgres;Password=postgres;Database=postgres";
            string conn = DbModule.dbConnection;
            using (NpgsqlConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();
                //MAddressというテーブルのレコードを取得
                var MAddressModels = connection.Query<string>(selectAddressAllQuery);
                ViewBag.addressItems = MAddressModels;
            }
            ViewData["loginId"] = id;
            return View("Index");
        }

        /***
         * t_user_infoに登録する
        */
        [HttpPost]
        public bool Register(string userId, string address, string birthday)
        {
            bool returnResut = false;
            try{
                //string conn = @"Server=localhost;Port=5433;User Id=postgres;Password=postgres;Database=postgres";
                string conn = DbModule.dbConnection;
                using (NpgsqlConnection connection = new NpgsqlConnection(conn))
                {
                    connection.Open();
                    //Dapperがカラム名のアンダースコアを無視する設定
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                    // t_user_infoに登録されているかを確認する
                    bool? tUserInfoDeletFlag = connection.Query<bool?>(selectUserInfoGetBoolQuery,new{userId = userId}).SingleOrDefault();
                    // m_addressテーブルからaddressIdを取得する
                    int mAddressId = connection.Query<int>(selectAddressIdGetQuery,new{address_name = address}).First();

                    //delete_flag確認
                    if(tUserInfoDeletFlag == false){
                        returnResut = true;
                        connection.Close();
                        return returnResut;
                    }
                    if(tUserInfoDeletFlag == true){
                        //t_user_infoをupdateする
                        connection.Execute(updateTUserInfoQuery,new{ userId = userId, addressId = mAddressId, birthday = birthday, deleteFlag = false});
                        returnResut = true;
                        connection.Close();
                        return returnResut;
                    }

                    // t_user_infoにinsertする
                    connection.Execute(insertQuery,new{ user_id = userId, address_id = mAddressId, birthday = birthday, delete_flag = false});
                    returnResut = true;
                    connection.Close();
                }
            }catch(Exception e){
                Console.WriteLine(e);
            }
            return returnResut;
        }
    }
}
