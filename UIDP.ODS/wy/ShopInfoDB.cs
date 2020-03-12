using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.wy
{
    public class ShopInfoDB
    {
        DBTool db = new DBTool("");
        public DataTable GetShopInfo(string ZHXM,string IS_PASS,int FWSX,string CZ_SHID)
        {
            string sql = "select a.FWID,a.FWBH,a.FWMC,b.*,c.Name,a.FWID AS OLDID from wy_houseinfo a  " +
                " join wy_shopinfo b ON a.CZ_SHID=b.CZ_SHID" +
                " left join tax_dictionary c on a.LSFGS=c.Code and c.ParentCode='LSFGS'" +
                " where a.IS_DELETE=0 AND b.IS_DELETE=0 AND a.FWSX="+FWSX+"";
            if (!string.IsNullOrWhiteSpace(IS_PASS))
            {
                sql += " AND IS_PASS='" + IS_PASS + "'";
            }
            if (!string.IsNullOrEmpty(ZHXM))
            {
                sql += " AND ZHXM='" + ZHXM + "'";
            }
            if (!string.IsNullOrEmpty(CZ_SHID))
            {
                sql += " AND b.CZ_SHID='" + CZ_SHID + "'";
            }
            //sql += " ORDER BY SHOP_ID OFFSET" + ((page - 1) * limit) + " rows fetch next " + limit + " rows only";
            return db.GetDataTable(sql);
        }

        public DataTable GetShopInfoDetail(string CZ_SHID)
        {
            string sql = "select a.FWID,a.FWBH,a.FWMC,a.JZMJ,a.ZLWZ,b.*,c.Name,a.FWID AS OLDID,d.LEASE_ID,d.ZLKSSJ,d.ZLZZSJ,d.ZLZE,d.ZLYJ,d.ZLYS,d.ZJJFFS,e.FEE_ID,e.WYJFFS,e.WYJZSJ,e.WYJZ" +
                " from wy_houseinfo a " +
                " join wy_shopinfo b ON a.CZ_SHID=b.CZ_SHID" +
                " left join tax_dictionary c on a.LSFGS=c.Code and c.ParentCode='LSFGS'" +
                " left join wy_Leasinginfo d on b.LEASE_ID=d.LEASE_ID and d.IS_DELETE=0" +
                " left join wy_RopertyCosts e on b.FEE_ID=e.FEE_ID AND e.IS_DELETE=0" +
                " where a.IS_DELETE=0 AND b.IS_DELETE=0 AND b.CZ_SHID='" + CZ_SHID + "'";
            return db.GetDataTable(sql);
        }

        public string DeleteShopInfo(string CZ_SHID,string FWID)
        {
            string ShopSql = " UPDATE wy_shopinfo set IS_DELETE=1 where CZ_SHID='" + CZ_SHID + "'";
            string HouseSql = " UPDATE wy_houseinfo SET CZ_SHID=NULL,FWSX=0 WHERE FWID='" + FWID + "'";
            string FeeSql = "UPDATE wy_RopertyCosts SET IS_DELETE=1 WHERE FEE_ID in(SELECT FEE_ID FROM wy_shopinfo WHERE CZ_SHID='" + CZ_SHID + "')";
            string LeaseSql = " UPDATE wy_Leasinginfo SET IS_DELETE=1 WHERE LEASE_ID in (SELECT LEASE_ID FROM wy_shopinfo WHERE CZ_SHID='" + CZ_SHID + "')";
            List<string> list = new List<string>()
            {
                {ShopSql },
                {HouseSql },
                { FeeSql},
                { LeaseSql}
            };
            return db.Executs(list); ;
        }

        public string CreateShopInfo(Dictionary<string, object> d)
        {
            List<string> list = new List<string>();
            string CZ_SHID = Guid.NewGuid().ToString();//商户唯一ID
            string FEE_ID = Guid.NewGuid().ToString();//物业费ID
            string LEASE_ID = Guid.NewGuid().ToString();//租赁信息ID
            string SUBLET_ID = Guid.NewGuid().ToString();//转租用户的ID
            DateTime DateTime = DateTime.Now;//获取时间
            string SuletSql = string.Empty;
            //租赁信息语句
            string LeaseSql = "INSERT INTO wy_Leasinginfo(LEASE_ID,ZLKSSJ,ZLZZSJ,ZLZE,ZLYJ,ZLYS,ZJJFFS,CJR,CJSJ,IS_DELETE)VALUES(";
            LeaseSql += GetSqlStr(LEASE_ID);
            LeaseSql += GetSqlStr(d["ZLKSSJ"]);
            LeaseSql += GetSqlStr(d["ZLZZSJ"]);
            LeaseSql += GetSqlStr(d["ZLZE"],1);
            LeaseSql += GetSqlStr(d["ZLYJ"],1);
            LeaseSql += GetSqlStr(d["ZLYS"],1);
            LeaseSql += GetSqlStr(d["ZJJFFS"],1);
            LeaseSql += GetSqlStr(d["userId"]);
            LeaseSql += GetSqlStr(DateTime);
            LeaseSql += GetSqlStr(0,1);
            LeaseSql = LeaseSql.TrimEnd(',')+")";
            list.Add(LeaseSql);
            //物业费信息语句
            string FeeSql = "INSERT INTO wy_RopertyCosts (FEE_ID,WYJFFS,WYJZSJ,WYJZ,IS_DELETE)VALUES(";
            FeeSql += GetSqlStr(FEE_ID);
            FeeSql += GetSqlStr(d["WYJFFS"]);
            FeeSql += GetSqlStr(d["WYJZSJ"]);
            FeeSql += GetSqlStr(d["WYJZ"],1);
            FeeSql += GetSqlStr(0,1);
            FeeSql = FeeSql.TrimEnd(',') + ")";
            list.Add(FeeSql);
            if (d["IS_SUBLET"].ToString() == "1")
            {
                SuletSql= "INSERT INTO wy_shopinfo(CZ_SHID,JYNR,ZHXM,ZHXB,SFZH,MOBILE_PHONE,TELEPHONE,E_MAIL," +
                "IS_PASS,CJR,CJSJ,SHOP_NAME,SHOPBH,ZHLX,LEASE_ID,FEE_ID,IS_DELETE)";
                SuletSql += GetSqlStr(SUBLET_ID);
                SuletSql += GetSqlStr(d["JYNR"]);
                SuletSql += GetSqlStr(d["ZHXM"]);
                SuletSql += GetSqlStr(d["ZHXB"], 1);
                SuletSql += GetSqlStr(d["SFZH"]);
                SuletSql += GetSqlStr(d["MOBILE_PHONE"]);;
                SuletSql += GetSqlStr(d["TELEPHONE"]);
                SuletSql += GetSqlStr(d["E_MAIL"]);
                SuletSql += GetSqlStr(0, 1);
                SuletSql += GetSqlStr(d["userId"]);
                SuletSql += GetSqlStr(d["JYNR"]);
                SuletSql += GetSqlStr(DateTime);
                SuletSql += GetSqlStr(d["SHOP_NAME"]);
                SuletSql += GetSqlStr(d["SHOPBH"]);
                SuletSql += GetSqlStr(d["ZHLX"], 1);
                SuletSql += GetSqlStr(LEASE_ID);
                SuletSql += GetSqlStr(FEE_ID);
                SuletSql += GetSqlStr(0, 1);
                SuletSql = SuletSql.TrimEnd(',') + ")";
                list.Add(SuletSql);
            }
            //租户信息插入语句
            string ShopInfoSql = "INSERT INTO wy_shopinfo(CZ_SHID,JYNR,ZHXM,ZHXB,SFZH,MOBILE_PHONE,IS_SUBLET,SUBLET_ID,TELEPHONE,E_MAIL," +
                "IS_PASS,CJR,CJSJ,SHOP_NAME,SHOPBH,ZHLX,LEASE_ID,FEE_ID,IS_DELETE)values(";
            ShopInfoSql += GetSqlStr(CZ_SHID);
            ShopInfoSql += GetSqlStr(d["JYNR"]);
            ShopInfoSql += GetSqlStr(d["ZHXM"]);
            ShopInfoSql += GetSqlStr(d["ZHXB"],1);
            ShopInfoSql += GetSqlStr(d["SFZH"]);
            ShopInfoSql += GetSqlStr(d["MOBILE_PHONE"]);
            ShopInfoSql += GetSqlStr(d["IS_SUBLET"]);
            if (d["IS_SUBLET"].ToString() == "1")
            {
                ShopInfoSql += GetSqlStr(SUBLET_ID);
            }
            else
            {
                ShopInfoSql += GetSqlStr("");
            }
            ShopInfoSql += GetSqlStr(d["TELEPHONE"]);
            ShopInfoSql += GetSqlStr(d["E_MAIL"]);
            ShopInfoSql += GetSqlStr(0,1);
            ShopInfoSql += GetSqlStr(d["userId"]);
            ShopInfoSql += GetSqlStr(DateTime);
            ShopInfoSql += GetSqlStr(d["SHOP_NAME"]);
            ShopInfoSql += GetSqlStr(d["SHOPBH"]);
            ShopInfoSql += GetSqlStr(d["ZHLX"],1);
            ShopInfoSql += GetSqlStr(LEASE_ID);
            ShopInfoSql += GetSqlStr(FEE_ID);
            ShopInfoSql += GetSqlStr(0,1);
            ShopInfoSql = ShopInfoSql.TrimEnd(',') + ")";
            list.Add(ShopInfoSql);
            //房屋信息更新语句
            string HouseUpdateSql = "UPDATE wy_houseinfo set FWSX=" + d["userType"] + ",CZ_SHID='" + CZ_SHID + "' WHERE FWID='" + d["FWID"] + "'";
            list.Add(HouseUpdateSql);
            return db.Executs(list);

        }

        public string UpdateShopInfo(Dictionary<string, object> d)
        {
            List<string> list = new List<string>();
            if (d["FWID"].ToString() != d["OLDID"].ToString())
            {
                //回滚旧房屋状态Sql
                string RollBackSql = "update wy_houseinfo set FWSX=0,CZ_SHID=null where FWID='" + d["OLDID"] + "'";
                //更新新房屋的状态
                string UpdateHouseSql = "update wy_houseinfo set FWSX=" + d["userType"] + ",CZ_SHID='" + d["CZ_SHID"] + "' WHERE FWID='" + d["FWID"] + "'";
                list.Add(RollBackSql);
                list.Add(UpdateHouseSql);
            }
            //修改租赁信息
            string LeaseSql = "UPDATE wy_Leasinginfo SET ZLKSSJ=" + GetSqlStr(d["ZLKSSJ"]);
            LeaseSql += "ZLZZSJ="+GetSqlStr(d["ZLZZSJ"]);
            LeaseSql += "ZLZE="+GetSqlStr(d["ZLZE"], 1);
            LeaseSql += "ZLYJ="+GetSqlStr(d["ZLYJ"], 1);
            LeaseSql += "ZLYS="+GetSqlStr(d["ZLYS"], 1);
            LeaseSql += "ZJJFFS="+GetSqlStr(d["ZJJFFS"], 1);
            LeaseSql += "BJR="+GetSqlStr(d["userId"]);
            LeaseSql += "BJSJ="+GetSqlStr(DateTime.Now);
            LeaseSql = LeaseSql.TrimEnd(',') + " WHERE LEASE_ID='"+d["LEASE_ID"]+"'";
            list.Add(LeaseSql);
            
            if (d["IS_SUBLET"].ToString() == "1")
            {
                string SuletSql = "INSERT INTO wy_shopinfo(CZ_SHID,JYNR,ZHXM,ZHXB,SFZH,MOBILE_PHONE,TELEPHONE,E_MAIL," +
                "IS_PASS,CJR,CJSJ,SHOP_NAME,SHOPBH,ZHLX,LEASE_ID,FEE_ID,IS_DELETE)";
                //SuletSql += GetSqlStr(SUBLET_ID);
                SuletSql += GetSqlStr(d["JYNR"]);
                SuletSql += GetSqlStr(d["ZHXM"]);
                SuletSql += GetSqlStr(d["ZHXB"], 1);
                SuletSql += GetSqlStr(d["SFZH"]);
                SuletSql += GetSqlStr(d["MOBILE_PHONE"]); ;
                SuletSql += GetSqlStr(d["TELEPHONE"]);
                SuletSql += GetSqlStr(d["E_MAIL"]);
                SuletSql += GetSqlStr(0, 1);
                SuletSql += GetSqlStr(d["userId"]);
                SuletSql += GetSqlStr(d["JYNR"]);
                //SuletSql += GetSqlStr(DateTime);
                SuletSql += GetSqlStr(d["SHOP_NAME"]);
                SuletSql += GetSqlStr(d["SHOPBH"]);
                SuletSql += GetSqlStr(d["ZHLX"], 1);
                //SuletSql += GetSqlStr(LEASE_ID);
                //SuletSql += GetSqlStr(FEE_ID);
                SuletSql += GetSqlStr(0, 1);
                SuletSql = SuletSql.TrimEnd(',') + ")";
                list.Add(SuletSql);
            }
            //修改商户信息
            string ShopInfoSql = "UPDATE wy_shopinfo SET JYNR=" + GetSqlStr(d["JYNR"]);
            ShopInfoSql += "ZHXM=" + GetSqlStr(d["ZHXM"]);
            ShopInfoSql += "ZHXB=" + GetSqlStr(d["ZHXB"], 1);
            ShopInfoSql += "SFZH=" + GetSqlStr(d["SFZH"]);
            ShopInfoSql += "MOBILE_PHONE=" + GetSqlStr(d["MOBILE_PHONE"]);
            ShopInfoSql += "IS_SUBLET=" + GetSqlStr(d["IS_SUBLET"]);
            ShopInfoSql += "TELEPHONE=" + GetSqlStr(d["TELEPHONE"]);
            ShopInfoSql += "E_MAIL=" + GetSqlStr(d["E_MAIL"]);
            ShopInfoSql += "BJR=" + GetSqlStr(d["userId"]);
            ShopInfoSql += "BJSJ=" + GetSqlStr(DateTime.Now);
            ShopInfoSql += "SHOP_NAME=" + GetSqlStr(d["SHOP_NAME"]);
            ShopInfoSql += "SHOPBH=" + GetSqlStr(d["SHOPBH"]);
            ShopInfoSql += "ZHLX=" + GetSqlStr(d["ZHLX"], 1);
            ShopInfoSql = ShopInfoSql.TrimEnd(',') + " WHERE CZ_SHID='" + d["CZ_SHID"] + "'";
            list.Add(ShopInfoSql);
            //修改物业信息
            string FeeSql = "UPDATE wy_RopertyCosts SET WYJFFS="+GetSqlStr(d["WYJFFS"]);          
            FeeSql += "WYJZSJ="+GetSqlStr(d["WYJZSJ"]);
            FeeSql += "WYJZ="+GetSqlStr(d["WYJZ"], 1);
            FeeSql = FeeSql.TrimEnd(',') + " WHERE FEE_ID='"+d["FEE_ID"]+"'";
            list.Add(FeeSql);
            return db.Executs(list);
        }
        public string PassInfo(string CZ_SHID)
        {
            string sql = " UPDATE wy_shopinfo set IS_PASS=1 where CZ_SHID='" + CZ_SHID + "'";
            return db.ExecutByStringResult(sql);
        }
        public string UnpassInfo(string CZ_SHID)
        {
            string sql = " UPDATE wy_shopinfo set IS_PASS=0 where CZ_SHID='" + CZ_SHID + "'";
            return db.ExecutByStringResult(sql);
        }
        public string EndLease(string FWID, string CZ_SHID)
        {
            string HouseSql = "UPDATE wy_houseinfo SET FWSX=0,CZ_SHID=NULL WHERE FWID='" + FWID + "'";
            string FeeSql = "UPDATE wy_RopertyCosts SET IS_DELETE=1 WHERE FEE_ID=(SELECT FEE_ID FROM wy_shopinfo WHERE CZ_SHID='" + CZ_SHID + "')";
            string LeaseSql = " update wy_Leasinginfo SET IS_DELETE=1 WHERE LEASE_ID=(select LEASE_ID from wy_houseinfo WHERE CZ_SHID='" + CZ_SHID + "')";
            List<string> list = new List<string>()
            {
                {HouseSql },
                {FeeSql },
                {LeaseSql }
            };
            return db.Executs(list);
            //return db.ExecutByStringResult(sql);
        }

        public string GetSqlStr(object t, int type = 0)
        {
            if (t == null || t.ToString() == "")
            {
                return "null,";

            }
            else
            {
                if (type == 0)
                {
                    return "'" + t + "',";
                }
                else
                {
                    return t + ",";
                }
            }
        }
    }
}
