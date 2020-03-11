using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetShopInfo(string ZHXM,string FWSX,string FWID,int page, int limit) => Ok(SM.GetShopInfo(ZHXM, FWSX,FWID, page, limit));
        /// <summary>
        /// 删除列表中的商户信息
        /// </summary>
        /// <param name="CZ_SHID"></param>
        /// <param name="FWID"></param>
        /// <returns></returns>
        [HttpGet("DeleteShopInfo")]
        public IActionResult DeleteShopInfo(string CZ_SHID, string FWID) => Ok(SM.DeleteShopInfo(CZ_SHID, FWID));
    }
}