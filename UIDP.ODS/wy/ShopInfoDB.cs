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
        public DataTable GetShopInfo(string ZHXM,string FWSX,string FWID)
        {
            string sql = "select b.* from wy_houseinfo a left join wy_shopinfo b ON a.CZ_SHID=b.CZ_SHID" +
                "where a.FWSX="+FWSX+ " AND a.IS_DELETE=0 AND b.IS_DELETE=0";
            if (!string.IsNullOrEmpty(ZHXM))
            {
                sql += " ZHXM='" + ZHXM + "'";
            }
            if (!string.IsNullOrEmpty(FWID))
            {
                sql += " FWID='" + FWID + "'";
            }
            //sql += " ORDER BY SHOP_ID OFFSET" + ((page - 1) * limit) + " rows fetch next " + limit + " rows only";
            return db.GetDataTable(sql);
        }

        public string DeleteShopInfo(string CZ_SHID,string FWID)
        {
            string ShopSql = "UPDATE wy_shopinfo set IS_DELETE=1 where CZ_SHID='" + CZ_SHID + "'";
            string HouseSql = " UPDATE wy_houseinfo SET CZ_SHID=NULL,FWSX=0 WHERE FWID='" + FWID + "'";
            List<string> list = new List<string>()
            {
                {ShopSql },
                {HouseSql }
            };
            return db.Executs(list); ;
        }

        public string CreateShopInfo(Dictionary<string, object> d)
        {
            List<string> list = new List<string>();
            string SUBLET_ID = Guid.NewGuid().ToString();
            string CZ_SHID = Guid.NewGuid().ToString();
            if (d["IS_SUBLET"].ToString() == "1")
            {
                string SubletInsertSql= "INSERT INTO wy_shopinfo(CZ_SHID,SHOP_ID,JYNR,ZHXM,ZHLX,ZHXB,SFZH,MOBILE_PHONE,IS_SUBLET,SUBLET_ID,TELEPHONE,E_MAIL,IS_PASS,CJR,CJSJ" +
                ", IS_DELETE,SHOP_NAME,HOUSE_ID)VALUES('";
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
                SubletInsertSql += GetSqlStr(SUBLET_ID);
            }
            string shopInfoInsertSql = "INSERT INTO wy_shopinfo(CZ_SHID,SHOP_ID,JYNR,ZHXM,ZHLX,ZHXB,SFZH,MOBILE_PHONE,IS_SUBLET,SUBLET_ID,TELEPHONE,E_MAIL,IS_PASS,CJR,CJSJ" +
                ", IS_DELETE,SHOP_NAME,HOUSE_ID)VALUES('";
            string HouseInfoUpdateSql = "UPDATE wy_houseinfo SET CZ_SHID=" +
                " FWSX=";
            list.Add(shopInfoInsertSql);
            list.Add(HouseInfoUpdateSql);
            return db.Executs(list);
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
