using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Osler.DataLayer
{
    [Serializable]
    public class DataConnection
    {
        private String ConString = "Server=104.211.189.58;Database=infosys_tne;Uid=root;Pwd=pAsfbQ!MBU6tf2ZP;";

        public DataConnection()
        { }

        public DataConnection(String strCon)
        { }

        public List<ResultEntity> GetSpenOverTimeData(String strTimePeriod, String strTimePeriod1, String strTimePeriod2)
        {
            MySqlConnection con = new MySqlConnection();
            con = new MySqlConnection(ConString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("GetSpendOverTimeData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriod", strTimePeriod);
            cmd.Parameters.AddWithValue("@Year1", strTimePeriod1);
            cmd.Parameters.AddWithValue("@Year2", strTimePeriod2);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<ResultEntity> lResult = new List<ResultEntity>();
            lResult = (from DataRow dr in dt.Rows
                       select new ResultEntity()
                       {
                           TimePeriodType = dr["TimePeriod"].ToString(),
                           TimePeriod = dr["Year"].ToString(),
                           Amount = dr["Amount"].ToString()
                       }).ToList();

            return lResult;
        }

        public void GetEmployeeBandInsight(String strFinYear, String strPUCode)
        {
            MySqlConnection con = new MySqlConnection();
            con = new MySqlConnection(ConString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("EmployeeBandInsight", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pFinYear", strFinYear);
            cmd.Parameters.AddWithValue("@pPUCode", strPUCode);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<BUInsightEmployeeBandEntity> lResult = new List<BUInsightEmployeeBandEntity>();
            lResult = (from DataRow dr in dt.Rows
                       select new BUInsightEmployeeBandEntity()
                       {
                           PB2to5 = dr["TimePeriod"].ToString(),
                           PB6 = dr["Year"].ToString(),
                           PB7to9 = dr["Amount"].ToString(),
                           PBAVPnAbove = dr["Amount"].ToString()

                       }).ToList();

            //return lResult;
        }
    }
}