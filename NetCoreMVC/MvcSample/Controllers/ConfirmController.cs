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
    public class ConfirmController : Controller
    {
        private readonly String selectUserInfoGetQuery = "SELECT * FROM t_user_info WHERE user_id = @userId";
        private readonly String selectUserInfoGetQuery1 = "SELECT address_id FROM t_user_info WHERE user_id = @userId";
        private readonly String selecMAddressGetQuery = "SELECT * FROM m_address WHERE address_id = @addressId";
        private readonly String selectAddressAllQuery = "SELECT address_name FROM m_address";

        [HttpGet("Confirm/{id}")]
        public IActionResult Index(string id)
        {
            Console.WriteLine("確認画面");
            Console.WriteLine(id);
            TUserInfoModel tUserInfoModel;
            MAddressModel mAddress;
            try{
                //string conn = @"Server=localhost;Port=5433;User Id=postgres;Password=postgres;Database=postgres";
                string conn = DbModule.dbConnection;
                using (NpgsqlConnection connection = new NpgsqlConnection(conn))
                {
                    connection.Open();
                    // Dapperがカラム名のアンダースコアを無視する設定
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    // MAddressというテーブルのレコードを取得する
                    var MAddressModels = connection.Query<string>(selectAddressAllQuery);
                    ViewBag.addressItems = MAddressModels;

                    // r_user_infoというテーブルから値を取得する
                    tUserInfoModel = connection.Query<TUserInfoModel>(selectUserInfoGetQuery,new{userId = id}).First();
                    mAddress = connection.Query<MAddressModel>(selecMAddressGetQuery,new{addressId = tUserInfoModel.addressId}).First();

                    // t_user_infoにinsertする
                    Console.WriteLine(mAddress.addressName);
                    ViewBag.address = mAddress.addressName;
                    ViewData["address"] = mAddress.addressName;
                    ViewData["date"] = tUserInfoModel.birthday;

                    //DBとの接続終了する
                    connection.Close();
                }
            }catch(Exception e){
                Console.WriteLine(e);
                ViewData["address"] = "";
                ViewData["date"] = "";
            }
            ViewData["loginId"] = id;
            return View("Index");
        }
    }
}
