﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.wy
{
    public class HouseInfoDB
    {
        DBTool db = new DBTool("");
        public DataSet GetHouseInfo(string FWMC,string LSFGS,string FWSX,int limit, int page)
        {
            string sql = "select {0} from wy_houseinfo a" +
                " left join tax_dictionary b on a.LSFGS=b.Code AND b.ParentCode='LSFGS'" +
                " left join tax_dictionary c on a.JGLX=c.Code AND c.ParentCode='JGLX'" +
                " WHERE a.IS_DELETE=0";
            if (!string.IsNullOrEmpty(FWMC))
            {
                sql += " AND FWMC LIKE '%" + FWMC + "%'";
            }
            if (!string.IsNullOrEmpty(LSFGS))
            {
                sql += " AND LSFGS= '" + LSFGS + "'";
            }
            if (!string.IsNullOrEmpty(FWSX))
            {
                sql += " AND FWSX= '" + FWSX + "'";
            }
            sql += "{1}";
            string DataSql = string.Format(sql, "a.*,b.Name AS LS,c.Name AS JG", " ORDER BY FWBH OFFSET " + ((page - 1) * limit) + " rows fetch next " + limit + " rows only");
            string CountSql = string.Format(sql, "count(*) AS TOTAL", "");
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                {"DataSql",DataSql },
                {"CountSql",CountSql}
            };
            return db.GetDataSet(d);

        }
        public string CreateHouseInfo(Dictionary<string,object> d)
        {
            string sql = "INSERT INTO wy_houseinfo (FWID,FWSX,FWBH,FWMC,JZMJ,LSFGS,ZLWZ,JGLX,ZCYZ,SSQY,PMT,WATER_NUMBER,ELE_NUMBER,CJR,CJSJ,ZFK,IS_DELETE)" +
                " VALUES(";
            sql += GetSqlStr(Guid.NewGuid());
            sql += GetSqlStr(0,1);
            sql += GetSqlStr(d["FWBH"]);
            sql += GetSqlStr(d["FWMC"]);
            sql += GetSqlStr(d["JZMJ"],1);
            sql += GetSqlStr(d["LSFGS"]);
            sql += GetSqlStr(d["ZLWZ"]);
            sql += GetSqlStr(d["JGLX"]);
            sql += GetSqlStr(d["ZCYZ"],1);
            sql += GetSqlStr(d["SSQY"]);
            sql += GetSqlStr(d["newFilePath"]);
            sql += GetSqlStr(d["WATER_NUMBER"]);
            sql += GetSqlStr(d["ELE_NUMBER"]);
            sql += GetSqlStr(d["userId"]);
            sql += GetSqlStr(DateTime.Now);
            sql += GetSqlStr(d["ZFK"],1);
            sql += GetSqlStr(0,1);
            return db.ExecutByStringResult(sql.TrimEnd(',')+")");
        }

        public string UpdateHouseInfo(Dictionary<string,object> d)
        {
            string sql = "UPDATE wy_houseinfo SET FWSX=" + GetSqlStr(d["FWSX"]);
            sql += "FWBH=" + GetSqlStr(d["FWBH"]);
            sql += "FWMC=" + GetSqlStr(d["FWMC"]);
            sql += "JZMJ=" + GetSqlStr(d["JZMJ"],1);
            sql += "LSFGS=" + GetSqlStr(d["LSFGS"]);
            sql += "ZLWZ=" + GetSqlStr(d["ZLWZ"]);
            sql += "JGLX=" + GetSqlStr(d["JGLX"]);
            sql += "ZCYZ=" + GetSqlStr(d["ZCYZ"],1);
            sql += "SSQY=" + GetSqlStr(d["SSQY"]);
            sql += "PMT=" + GetSqlStr(d["newFilePath"]);
            sql += "WATER_NUMBER=" + GetSqlStr(d["WATER_NUMBER"]);
            sql += "ELE_NUMBER=" + GetSqlStr(d["ELE_NUMBER"]);
            sql += "BJR=" + GetSqlStr(d["BJR"]);
            sql += "BJSJ=" + GetSqlStr(DateTime.Now);
            sql += "ZFK=" + GetSqlStr(d["ZFK"],1);
            sql = sql.TrimEnd(',');
            sql += " WHERE FWID='" + d["FWID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string DeleteHouseInfo(string FWID)
        {
            string sql = "UPDATE wy_houseinfo set IS_DELETE=1 where FWID='" + FWID + "'";
            return db.ExecutByStringResult(sql);
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