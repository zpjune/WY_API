using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.wy
{
    public class TaskDB
    {
        DBTool db = new DBTool("");
        public DataTable GetTaskInfo(string RWBH,string RWMC)
        {
            string sql = "select a.*,b.NAME from wy_check_task a" +
                " JOIN v_taskregion b ON a.PLAN_DETAIL_ID=b.PLAN_DETAIL_ID " +
                " where IS_DELETE=0";
            if (!string.IsNullOrEmpty(RWBH))
            {
                sql += " AND RWBH='" + RWBH + "'";
            }
            if (!string.IsNullOrEmpty(RWMC))
            {
                sql += " AND RWMC like'%" + RWMC + "%'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateTask(Dictionary<string,object>d)
        {
            string sql = "INSERT INTO wy_check_task (TASK_ID,PLAN_DETAIL_ID,RWBH,RWMC,RWKSSJ,RWJSSJ,RWNR,RWFW,REMARK,CJR,CJSJ,IS_DELETE,IS_PUSH)VALUES(";
            sql += GetSqlStr(Guid.NewGuid());
            sql += GetSqlStr(d["PLAN_DETAIL_ID"]);
            sql += GetSqlStr(d["RWBH"]);
            sql += GetSqlStr(d["RWMC"]);
            sql += GetSqlStr(d["RWKSSJ"]);
            sql += GetSqlStr(d["RWJSSJ"]);
            sql += GetSqlStr(d["RWNR"]);
            sql += GetSqlStr(d["RWFW"]);
            sql += GetSqlStr(d["REMARK"]);
            sql += GetSqlStr(d["userId"]);
            sql += GetSqlStr(DateTime.Now);
            sql += GetSqlStr(0,1);
            sql += GetSqlStr(0, 1);
            sql = sql.TrimEnd(',') + ")";
            return db.ExecutByStringResult(sql);
        }

        public string UpdateTask(Dictionary<string,object>d)
        {
            string sql = "UPDATE wy_check_task SET PLAN_DETAIL_ID=" + GetSqlStr(d["PLAN_DETAIL_ID"]);
            sql += "RWBH=" + GetSqlStr(d["RWBH"]);
            sql += "RWMC=" + GetSqlStr(d["RWMC"]);
            sql += "RWKSSJ=" + GetSqlStr(d["RWKSSJ"]);
            sql += "RWJSSJ=" + GetSqlStr(d["RWJSSJ"]);
            sql += "RWNR=" + GetSqlStr(d["RWNR"]);
            sql += "RWFW=" + GetSqlStr(d["RWFW"]);
            sql += "REMARK=" + GetSqlStr(d["REMARK"]);
            sql += "BJR=" + GetSqlStr(d["userId"]);
            sql += "BJSJ=" + GetSqlStr(DateTime.Now);
            sql = sql.TrimEnd(',');
            sql += " WHERE TASK_ID='" + d["TASK_ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string DeleteTask(string TASK_ID)
        {
            string sql = "UPDATE wy_check_task SET IS_DELETE=1 WHERE TASK_ID='" + TASK_ID + "'";
            return db.ExecutByStringResult(sql);
        }

        public string PushTask(string TASK_ID)
        {
            string sql = "UPDATE wy_check_task SET IS_PUSH=1 WHERE TASK_ID='" + TASK_ID + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataSet GetPlanCheckAndDetail(string TASK_ID)
        {
            string CheckPlanDetailSql = "select a.*,b.NAME AS ALLPLACENAME,c.NAME AS JCNAME from wy_checkPlan_detail a" +
                " left join V_TaskRegion b on a.PLAN_DETAIL_ID= b.PLAN_DETAIL_ID " +
                " left join wy_task_detail_config c on c.Code=a.JCLX" +
                " where a.PLAN_DETAIL_ID=(select PLAN_DETAIL_ID from wy_check_task where TASK_ID='" + TASK_ID + "')";
            string CheckPlanSql = "select * from wy_checkPlan where PLAN_ID=(" +
                " SELECT PLAN_ID FROM wy_checkPlan_detail WHERE PLAN_DETAIL_ID=(" +
                " SELECT PLAN_DETAIL_ID FROM wy_check_task WHERE TASK_ID='" + TASK_ID + "'))";
            Dictionary<string, string> list = new Dictionary<string, string>()
            {
                {"CheckPlanDetailSql",CheckPlanDetailSql},
                {"CheckPlanSql",CheckPlanSql }
            };
            return db.GetDataSet(list);
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
