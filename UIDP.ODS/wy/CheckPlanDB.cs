using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.wy
{
    public class CheckPlanDB
    {
        DBTool db = new DBTool("");

        public DataTable GetCheckPlan(string JHMC,string JHND)
        {
            string sql = " SELECT * from wy_checkPlan where IS_DELETE=0";
            if (!string.IsNullOrEmpty(JHMC))
            {
                sql += " and JHMC='" + JHMC + "'";
            }
            if (!string.IsNullOrEmpty(JHND))
            {
                sql += " and JHND=YEAR('" + JHND + "')";
            }
            return db.GetDataTable(sql);
        }

        public DataTable GetCheckPlanDetail(string PLAN_ID)
        {
            return db.GetDataTable("SELECT * FROM wy_checkPlan_detail WHERE PLAN_ID='" + PLAN_ID + "'AND IS_DELETE=0");
        }

        public string CreateCheckPlan(Dictionary<string,object> d)
        {
            List<string> SqlList = new List<string>();
            string PLAN_ID = Guid.NewGuid().ToString();
            string CheckPlanSql = "INSERT INTO wy_checkPlan (PLAN_ID,JHND,JHMC,JHSM,JHSJ,REMARK,CJR,CJSJ,IS_DELETE)values(";
            CheckPlanSql += GetSqlStr(PLAN_ID);
            CheckPlanSql += "YEAR('" + d["JHND"] + "'),";
            CheckPlanSql += GetSqlStr(d["JHMC"]);
            CheckPlanSql += GetSqlStr(d["JHSM"]);
            CheckPlanSql += GetSqlStr(d["JHSJ"]);
            CheckPlanSql += GetSqlStr(d["REMARK"]);
            CheckPlanSql += GetSqlStr(d["userId"]);
            CheckPlanSql += GetSqlStr(DateTime.Now);
            CheckPlanSql += GetSqlStr(0,1);
            CheckPlanSql = CheckPlanSql.TrimEnd(',') + ")";
            SqlList.Add(CheckPlanSql);
            foreach(Dictionary<string,object> CheckPlanDetail in JArray.FromObject(d["CheckPlanDetail"]).ToObject<List<Dictionary<string, object>>>())
            {
                if (CheckPlanDetail.ContainsValue(""))
                {
                    return "您输入的详细计划内有空值！请仔细修改后再提交表单";
                }
                else
                {
                    string DetailSql = "INSERT INTO wy_checkPlan_detail(PLAN_DETAIL_ID,PLAN_ID,JCQY,JCNR,JCLX,PCCS,CJR,CJSJ,IS_DELETE)VALUES(";
                    DetailSql += GetSqlStr(Guid.NewGuid());
                    DetailSql += GetSqlStr(PLAN_ID);
                    DetailSql += GetSqlStr(CheckPlanDetail["JCQY"]);
                    DetailSql += GetSqlStr(CheckPlanDetail["JCNR"]);
                    DetailSql += GetSqlStr(CheckPlanDetail["JCLX"]);
                    DetailSql += GetSqlStr(CheckPlanDetail["PCCS"], 1);
                    DetailSql += GetSqlStr(d["userId"]);
                    DetailSql += GetSqlStr(DateTime.Now);
                    DetailSql += GetSqlStr(0, 1);
                    DetailSql = DetailSql.TrimEnd(',') + ")";
                    SqlList.Add(DetailSql);
                }
                
            }
            return db.Executs(SqlList);
        }


        public string UpdateCheckPlan(Dictionary<string,object> d)
        {
            List<string> SqlList = new List<string>();
            string CheckPlanSql = "UPDATE wy_checkPlan SET JHND="+"YEAR('" + d["JHND"] + "'),";
            CheckPlanSql += "JHMC=" + GetSqlStr(d["JHMC"]);
            CheckPlanSql += "JHSM=" + GetSqlStr(d["JHSM"]);
            CheckPlanSql += "JHSJ=" + GetSqlStr(d["JHSJ"]);
            CheckPlanSql += "REMARK=" + GetSqlStr(d["REMARK"]);
            CheckPlanSql += "BJR=" + GetSqlStr(d["userId"]);
            CheckPlanSql += "BJSJ=" + GetSqlStr(DateTime.Now);
            CheckPlanSql += "IS_DELETE=" + GetSqlStr(0,1);
            CheckPlanSql = CheckPlanSql.TrimEnd(',');
            CheckPlanSql += " WHERE PLAN_ID='" + d["PLAN_ID"] + "'";
            SqlList.Add(CheckPlanSql);
            foreach (Dictionary<string, object> CheckPlanDetail in JArray.FromObject(d["CheckPlanDetail"]).ToObject<List<Dictionary<string, object>>>())
            {
                if(!CheckPlanDetail.ContainsKey("PLAN_DETAIL_ID"))
                {
                    if (CheckPlanDetail.ContainsValue(""))
                    {
                        return "您输入的详细计划内有空值！请仔细修改后再提交表单";
                    }
                    else
                    {
                        string DetailSql = "INSERT INTO wy_checkPlan_detail(PLAN_DETAIL_ID,PLAN_ID,JCQY,JCNR,JCLX,PCCS,CJR,CJSJ,IS_DELETE)VALUES(";
                        DetailSql += GetSqlStr(Guid.NewGuid());
                        DetailSql += GetSqlStr(d["PLAN_ID"]);
                        DetailSql += GetSqlStr(CheckPlanDetail["JCQY"]);
                        DetailSql += GetSqlStr(CheckPlanDetail["JCNR"]);
                        DetailSql += GetSqlStr(CheckPlanDetail["JCLX"]);
                        DetailSql += GetSqlStr(CheckPlanDetail["PCCS"], 1);
                        DetailSql += GetSqlStr(d["userId"]);
                        DetailSql += GetSqlStr(DateTime.Now);
                        DetailSql += GetSqlStr(0, 1);
                        DetailSql = DetailSql.TrimEnd(',') + ")";
                        SqlList.Add(DetailSql);
                    }                       
                }
                else
                {
                    if (CheckPlanDetail.ContainsValue(""))
                    {
                        return "您输入的详细计划内有空值！请仔细修改后再提交表单";
                    }
                    else
                    {
                        if (CheckPlanDetail["IS_DELETE"].ToString() == "0")
                        {
                            string DetailSql = "UPDATE wy_checkPlan_detail SET JCQY=" + GetSqlStr(CheckPlanDetail["JCQY"]);
                            DetailSql += " JCNR=" + GetSqlStr(CheckPlanDetail["JCNR"]);
                            DetailSql += " JCLX=" + GetSqlStr(CheckPlanDetail["JCLX"]);
                            DetailSql += " PCCS=" + GetSqlStr(CheckPlanDetail["PCCS"], 1);
                            DetailSql += " BJR=" + GetSqlStr(d["userId"]);
                            DetailSql += " BJSJ=" + GetSqlStr(DateTime.Now);
                            DetailSql = DetailSql.TrimEnd(',') + " where PLAN_DETAIL_ID='" + CheckPlanDetail["PLAN_DETAIL_ID"] + "'";
                            SqlList.Add(DetailSql);
                        }
                        else
                        {
                            string DetailSql = "UPDATE wy_checkPlan_detail SET IS_DELETE=1 " +
                                " where PLAN_DETAIL_ID='" + CheckPlanDetail["PLAN_DETAIL_ID"] + "'";
                            SqlList.Add(DetailSql);
                        }
                        
                    }
                        
                } 
            }
            return db.Executs(SqlList);
        }

        public string DeleteCheckPlanDetail(string PLAN_DETAIL_ID)
        {
            return db.ExecutByStringResult("UPDATE wy_checkPlan_detail SET IS_DELETE=1 WHERE PLAN_DETAIL_ID='" + PLAN_DETAIL_ID + "'");
        }

        public string DeleteCheckPlan(string PLAN_ID)
        {
            return db.Executs(new List<string>()
            {
                { "update wy_checkPlan set IS_DELETE=1 WHERE PLAN_ID='" + PLAN_ID + "'"},
                { "update wy_checkPlan_detail set IS_DELETE=1 WHERE PLAN_ID='" + PLAN_ID + "'"}
            });
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
