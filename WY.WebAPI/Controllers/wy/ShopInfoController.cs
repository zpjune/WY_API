﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule.wy;

namespace WY.WebAPI.Controllers.wy
{
    [Produces("application/json")]
    [Route("ShopInfo")]
    public class ShopInfoController : Controller
    {
        ShopInfoModule SM = new ShopInfoModule();
        /// <summary>
        /// 查询列表中的商铺信息
        /// </summary>
        /// <param name="ZHXM"></param>
        /// <param name="FWSX"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetShopInfo")]
        public IActionResult GetShopInfo(string ZHXM, string IS_PASS, int FWSX, string FWID, int page, int limit) => Ok(SM.GetShopInfo(ZHXM, IS_PASS,FWSX,FWID, page, limit));
        /// <summary>
        /// 获取商户详情 包括房屋信息，商户信息，租赁信息，物业信息等
        /// </summary>
        /// <param name="FWID"></param>
        /// <returns></returns>
        [HttpGet("GetShopInfoDetail")]
        public IActionResult GetShopInfoDetail(string CZ_SHID) => Ok(SM.GetShopInfoDetail(CZ_SHID));
        /// <summary>
        /// 删除列表中的商户信息
        /// </summary>
        /// <param name="CZ_SHID"></param>
        /// <param name="FWID"></param>
        /// <returns></returns>
        [HttpGet("DeleteShopInfo")]
        public IActionResult DeleteShopInfo(string CZ_SHID, string FWID) => Ok(SM.DeleteShopInfo(CZ_SHID, FWID));
        /// <summary>
        /// 创建商户信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("CreateShopInfo")]
        public IActionResult CreateShopInfo([FromBody]JObject value) => Ok(SM.CreateShopInfo(value.ToObject<Dictionary<string,object>>()));

        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("UpdateShopInfo")]
        public IActionResult UpdateShopInfo([FromBody]JObject value) => Ok(SM.UpdateShopInfo(value.ToObject<Dictionary<string, object>>()));
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="CZ_SHID">商户ID</param>
        /// <returns></returns>
        [HttpGet("PassInfo")]
        public IActionResult PassInfo(string CZ_SHID) => Ok(SM.PassInfo(CZ_SHID));
        /// <summary>
        /// 不通过审核
        /// </summary>
        /// <param name="CZ_SHID"></param>
        /// <returns></returns>
        [HttpGet("UnpassInfo")]
        public IActionResult UnpassInfo(string CZ_SHID) => Ok(SM.UnpassInfo(CZ_SHID));

        /// <summary>
        /// 终止租赁
        /// </summary>
        /// <param name="FWID"></param>
        /// <param name="CZ_SHID"></param>
        /// <returns></returns>
        [HttpGet("EndLease")]
        public IActionResult EndLease(string FWID,string CZ_SHID) => Ok(SM.EndLease(FWID,CZ_SHID));




    }
}