using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.wy;
using UIDP.UTILITY;

namespace UIDP.BIZModule.wy
{
    public class ShopInfoModule
    {
        ShopInfoDB db = new ShopInfoDB();
        public Dictionary<string,object> GetShopInfo(string ZHXM, string FWSX,string FWID,int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetShopInfo(ZHXM, FWSX,FWID);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功！";
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt,page,limit);
                    r["total"] = dt.Rows.Count;

                }
                else
                {
                    r["message"] = "成功，但是没有数据";
                    r["code"] = 2000;
                    r["items"] = new DataTable();
                    r["total"] = 0;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> DeleteShopInfo(string CZ_SHID,string FWID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DeleteShopInfo(CZ_SHID,FWID);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = b;
                    r["code"] = -1;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
    }
}
