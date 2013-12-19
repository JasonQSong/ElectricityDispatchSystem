using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ElectricityWcfService
{
    public class DatabaseConnector
    {
        public string DMain()
        {
            string c = "";
            connect();
            c+="Done.";
            return c;
        }
        private static MySqlConnection conn = null;
        private static void connect()
        {
            if (conn != null)
                return;
            string connStr = "server=localhost;user=root;database=electricity;port=3306;password=;charset=utf8";
            conn = new MySqlConnection(connStr);
            conn.Open();
        }
        public int AddLogUser(LogUser Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `log_user` (`Time`,`UserID`,`Action`) VALUE ('{0}','{1}','{2}');",
                Record.Time.ToString("yyyy-MM-dd HH:mm:ss"), Record.UserID, Record.Action);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddConfigLineInformation(ConfigLineInformation Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `config_line_information` (`LineName`,`StationID_Start`,`StationID_End`,`VoltageLevel`,`RatedCurrent`) VALUE ('{0}','{1}','{2}','{3}','{4}');",
                Record.LineName, Record.StationID_Start, Record.StationID_End, Record.VoltageLevel, Record.RatedCurrent);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddStationLineInformation(ConfigStationInformation Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `config_station_information` (`StationName`,`Longitude`,`Latitude`,`BuildTime`,`VoltageLevel`,`InstallCapacity`) VALUE ('{0}','{1}','{2}','{3}','{4}','{5}');",
                Record.StationName, Record.Longitude, Record.Latitude, Record.BuildTime.ToString("yyyy-MM-dd HH:mm:ss"), Record.VoltageLevel, Record.InstallCapacity);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddRuntimeLineData(RuntimeLineData Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `runtime_line_data` (`LineID`,`LoadQuantity`,`Time`) VALUE ('{0}','{1}','{2}');",
                Record.LineID, Record.LoadQuantity, Record.Time.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddRuntimeStationData(RuntimeStationData Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `runtime_station_data` (`StationID`,`ActivePower`,`ReactivePower`,`Time`) VALUE ('{0}','{1}','{2}','{3}');",
                Record.StationID, Record.ActivePower, Record.ReactivePower, Record.Time.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddForecastDayStationData(ForecastDayStationData Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `forecast_day_station_data` (`StationID`,`ForecastType`,`ActivePower`,`ReactivePower`,`Time`) VALUE ('{0}','{1}','{2}','{3}','{4}');",
                Record.StationID,Record.ForecastType, Record.ActivePower, Record.ReactivePower, Record.Time.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public LogUser FindLogUser(int ID)
        {
            connect();
            string sql = String.Format("SELECT * FROM `log_user` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            LogUser Record = new LogUser() { 
                ID = (int)rdr["ID"], 
                Time = (DateTime)rdr["Time"], 
                UserID = (int)rdr["UserID"], 
                Action = (string)rdr["Action"]
            };
            rdr.Close();
            return Record;
        }
        public ConfigLineInformation FindConfigLineInformation(int ID)
        {
            connect();
            string sql = String.Format("SELECT * FROM `config_line_information` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            ConfigLineInformation Record = new ConfigLineInformation() { 
                ID = (int)rdr["ID"],
                LineName=(string)rdr["LineName"],
                StationID_Start = (int)rdr["StationID_Start"],
                StationID_End = (int)rdr["StationID_End"],
                VoltageLevel = double.Parse((string)rdr["VoltageLevel"]),
                RatedCurrent = double.Parse((string)rdr["RatedCurrent"])
            };
            rdr.Close();
            return Record;
        }
        public ConfigStationInformation FindConfigStationInformation(int ID)
        {
            connect();
            string sql = String.Format("SELECT * FROM `config_station_information` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            ConfigStationInformation Record = new ConfigStationInformation()
            {
                ID = (int)rdr["ID"],
                StationName = (string)rdr["StationName"],
                Longitude = double.Parse((string)rdr["Longitude"]),
                Latitude = double.Parse((string)rdr["Latitude"]),
                BuildTime = (DateTime)rdr["BuildTime"],
                VoltageLevel = double.Parse((string)rdr["VoltageLevel"]),
                InstallCapacity = double.Parse((string)rdr["InstallCapacity"])
            };
            rdr.Close();
            return Record;
        }
        public ConfigStationInformation FindConfigStationInformationByStationName(string StationName)
        {
            connect();
            string sql = String.Format("SELECT * FROM `config_station_information` WHERE `StationName`='{0}';",
                StationName);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            ConfigStationInformation Record = new ConfigStationInformation()
            {
                ID = (int)rdr["ID"],
                StationName = (string)rdr["StationName"],
                Longitude = double.Parse((string)rdr["Longitude"]),
                Latitude = double.Parse((string)rdr["Latitude"]),
                BuildTime = (DateTime)rdr["BuildTime"],
                VoltageLevel = double.Parse((string)rdr["VoltageLevel"]),
                InstallCapacity = double.Parse((string)rdr["InstallCapacity"])
            };
            rdr.Close();
            return Record;
        }
        public RuntimeLineData FindRuntimeLineData(int ID)
        {
            connect();
            string sql = String.Format("SELECT * FROM `runtime_line_data` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            RuntimeLineData Record = new RuntimeLineData()
            {
                ID = (int)rdr["ID"],
                LineID = (int)rdr["LineID"],
                LoadQuantity = double.Parse((string)rdr["LoadQuantity"]),
                Time = (DateTime)rdr["Time"]
            };
            rdr.Close();
            return Record;
        }
        public RuntimeStationData FindRuntimeStationData(int ID)
        {
            connect();
            string sql = String.Format("SELECT * FROM `runtime_station_data` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            RuntimeStationData Record = new RuntimeStationData()
            {
                ID = (int)rdr["ID"],
                StationID = (int)rdr["StationID"],
                ActivePower = double.Parse((string)rdr["ActivePower"]),
                ReactivePower = double.Parse((string)rdr["ReactivePower"]),
                Time = (DateTime)rdr["Time"]
            };
            rdr.Close();
            return Record;
        }

        public List<RuntimeStationData> SelectRuntimeStationData(int StationID, DateTime TargetDate)
        {
            connect();
            DateTime TimeStart = TargetDate.Date;
            DateTime TimeEnd = TimeStart + TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1);
            List<RuntimeStationData> DataList = new List<RuntimeStationData>();
            string sql = String.Format("SELECT * FROM `runtime_station_data` WHERE `StationID`={0} AND `Time` BETWEEN '{1}' AND '{2}';",
                StationID, TimeStart.ToString("yyyy-MM-dd HH:mm:ss"), TimeEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                RuntimeStationData Record = new RuntimeStationData()
                {
                    ID = (int)rdr["ID"],
                    StationID = (int)rdr["StationID"],
                    ActivePower = double.Parse((string)rdr["ActivePower"]),
                    ReactivePower = double.Parse((string)rdr["ReactivePower"]),
                    Time = (DateTime)rdr["Time"]
                };
                DataList.Add(Record);
            }
            rdr.Close();
            return DataList;
        }
        public List<ForecastDayStationData> SelectForecastDayStationData(int StationID, DateTime TargetDate,int Forecast)
        {
            connect();
            DateTime TimeStart = TargetDate.Date;
            DateTime TimeEnd = TimeStart + TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1);
            List<ForecastDayStationData> DataList = new List<ForecastDayStationData>();
            string sql = String.Format("SELECT * FROM `forecast_day_station_data` WHERE `StationID`={0} AND `ForecastType`={1} AND `Time` BETWEEN '{2}' AND '{3}';",
                StationID,Forecast, TimeStart.ToString("yyyy-MM-dd HH:mm:ss"), TimeEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ForecastDayStationData Record = new ForecastDayStationData()
                {
                    ID = (int)rdr["ID"],
                    StationID = (int)rdr["StationID"],
                    ForecastType = (int)rdr["ForecastType"],
                    ActivePower = double.Parse((string)rdr["ActivePower"]),
                    ReactivePower = double.Parse((string)rdr["ReactivePower"]),
                    Time = (DateTime)rdr["Time"]
                };
                DataList.Add(Record);
            }
            rdr.Close();
            return DataList;
        }

    }
}