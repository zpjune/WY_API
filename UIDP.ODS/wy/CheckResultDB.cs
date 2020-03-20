using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.wy
{
    public class CheckResultDB
    {
        DBTool db = new DBTool("");

        public DataTable GetCheckResult(string year,string FWBH,string RWMC)
        {
            string sql = "select a.*,e.FWBH,f.ZHXM from wy_check_result a" +
                " join wy_check_task b on a.TASK_ID=b.TASK_ID AND b.IS_DELETE=0" +
                " join wy_checkPlan_detail c on b.PLAN_DETAIL_ID=c.PLAN_DETAIL_ID AND c.IS_DELETE=0" +
                " join wy_checkPlan d on c.PLAN_ID=d.PLAN_ID AND d.IS_DELETE=0" +
                " join wy_houseinfo e on a.FWID=e.FWID AND e.IS_DELETE=0" +
                " join wy_shopinfo f on e.CZ_SHID=f.CZ_SHID AND f.IS_DELETE=0" +
                " where a.IS_DELETE=0 AND d.JHND='"+year+"'";
            if (!string.IsNullOrEmpty(FWBH))
            {
                sql += " AND e.FWBH='" + FWBH + "'";
            }
            if (!string.IsNullOrEmpty(RWMC))
            {
                sql += " AND b.RWMC like'%" + RWMC + "%'";
            }
            return db.GetDataTable(sql);
        }

       public DataTable GetTaskProcessInfo(string year,string RWMC,string RWBH)
        {
            string sql= "select *,(t.total-t.complete) AS incomplete from" +
                "(select a.RWBH,a.RWMC,a.RWKSSJ,a.RWJSSJ,a.RWFW,d.Name," +
                "(SELECT COUNT(*)AS TOTAL FROM wy_houseinfo where a.RWFW=SSQY)AS total," +
                "(select COUNT(distinct FWID) FROM wy_check_result  where a.TASK_ID=TASK_ID) AS complete" +
                " FROM wy_check_task a" +
                " join wy_checkPlan_detail b on a.PLAN_DETAIL_ID=b.PLAN_DETAIL_ID AND b.IS_DELETE=0 " +
                " join wy_checkPlan c on b.PLAN_ID=c.PLAN_ID AND c.IS_DELETE=0" +
                " left join tax_dictionary d on a.RWFW=d.Code AND d.ParentCode='SSQY'" +
                " where c.JHND='" + year + "'";
            if (!string.IsNullOrEmpty(RWMC))
            {
                sql += " AND  a.RWMC='" + RWMC + "'";
            }
            if (!string.IsNullOrEmpty(RWBH))
            {
                sql += " AND  a.RWBH='" + RWBH + "'";
            }
            sql += " GROUP BY a.RWBH,a.RWMC,a.RWKSSJ,a.RWJSSJ,a.RWFW,a.TASK_ID,d.Name)t";
            return db.GetDataTable(sql);
        }
    }
}
