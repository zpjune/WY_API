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
        public Dictionary<string,object> GetShopInfo(string ZHXM, string IS_PASS, string FWSX, string FWID,int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetShopInfo(ZHXM, IS_PASS,FWSX, FWID);
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

        public Dictionary<string, object> GetShopInfoDetail(string CZ_SHID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetShopInfoDetail(CZ_SHID);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功！";
                    r["code"] = 2000;
                    r["items"] = dt;
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
            catch (Exception e)
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
        public Dictionary<string,object> CreateShopInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateShopInfo(d);
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
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> UpdateShopInfo(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.UpdateShopInfo(d);
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
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> PassInfo(string CZ_SHID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.PassInfo(CZ_SHID);
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
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> UnpassInfo(string CZ_SHID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.UnpassInfo(CZ_SHID);
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
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> EndLease(string FWID,string CZ_SHID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            bool IsArrears = false;
            try
            {
                DataTable InfoDic = db.GetShopCostInfo(FWID);
                if (InfoDic.Rows.Count > 0)
                {
                    foreach (DataRow dr in InfoDic.Rows)
                    {
                        if (dr["JFLX"].ToString() == "0")
                        {
                            if (!(Convert.ToDateTime(dr["YXQZ"]) > DateTime.Now))
                            {
                                IsArrears = true;
                                break;
                            }
                        }
                    }
                    if (!IsArrears)
                    {
                        string b = db.EndLease(FWID, CZ_SHID);
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
                    else
                    {
                        r["message"] = "还有欠费信息！";
                        r["code"] = -1;
                    }
                    
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> GetShopUserInfo(string FWBH, string ZHXM, string SFZH, string SHOPBH, int SHOP_STATUS,int limit,int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetShopUserInfo(FWBH, ZHXM, SFZH, SHOPBH, SHOP_STATUS);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但是没有数据,租户可能已经解除了物业关系";
                    r["items"] = dt;
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }


        public Dictionary<string, object> GetShopDetailUserInfo(string CZ_SHID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetShopDetailUserInfo(CZ_SHID);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功,但是没有数据";
                    r["items"] = dt;
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> SecondHand(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.SecondHand(d);
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
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> ExportShopInfo(string FWSX)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.ExportShopInfo(FWSX);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功！";
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;

                }
                else
                {
                    r["message"] = "成功，但是没有数据";
                    r["code"] = 2001;
                    r["items"] = new DataTable();
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
    }
}
