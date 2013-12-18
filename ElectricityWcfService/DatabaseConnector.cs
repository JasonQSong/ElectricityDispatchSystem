﻿using System;
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
            string connStr = "server=localhost;user=root;database=softwareengineering;port=3306;password=;";
            conn = new MySqlConnection(connStr);
            conn.Open();
        }
        public int AddLogUser(LogUser Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `log_user` (`Time`,`UserID`,`Action`) VALUE ('{0}','{1}','{2}');",
                Record.Time.ToString("yyyy-MM-dd hh:mm:ss"), Record.UserID, Record.Action);
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
                Record.StationName, Record.Longitude, Record.Latitude, Record.BuildTime.ToString("yyyy-MM-dd hh:mm:ss"), Record.VoltageLevel, Record.InstallCapacity);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddRuntimeLineData(RuntimeLineData Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `runtime_line_data` (`LineID`,`LoadQuantity`,`Time`) VALUE ('{0}','{1}','{2}');",
                Record.LineID, Record.LoadQuantity, Record.Time.ToString("yyyy-MM-dd hh:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public int AddRuntimeStationData(RuntimeStationData Record)
        {
            connect();
            string sql = String.Format("INSERT INTO `runtime_station_data` (`StationID`,`ActivePower`,`ReactivePower`,`Time`) VALUE ('{0}','{1}','{2}','{3}');",
                Record.StationID, Record.ActivePower, Record.ReactivePower, Record.Time.ToString("yyyy-MM-dd hh:mm:ss"));
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
                StationName = (string)rdr["LineName"],
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
            string sql = String.Format("SELECT * FROM `config_station_information` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            RuntimeLineData Record = new RuntimeLineData()
            {
                ID = (int)rdr["ID"],
                /*
                LineName = (string)rdr["LineName"],
                StationID_Start = (int)rdr["StationID_Start"],
                StationID_End = (int)rdr["StationID_End"],
                VoltageLevel = double.Parse((string)rdr["VoltageLevel"]),
                RatedCurrent = double.Parse((string)rdr["RatedCurrent"])
                 * */
            };
            rdr.Close();
            return Record;
        }
        public RuntimeStationData FindRuntimeStationData(int ID)
        {
            connect();
            string sql = String.Format("SELECT * FROM `config_station_information` WHERE `ID`={0};",
                ID);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            RuntimeStationData Record = new RuntimeStationData()
            {
                ID = (int)rdr["ID"],
                /*
                LineName = (string)rdr["LineName"],
                StationID_Start = (int)rdr["StationID_Start"],
                StationID_End = (int)rdr["StationID_End"],
                VoltageLevel = double.Parse((string)rdr["VoltageLevel"]),
                RatedCurrent = double.Parse((string)rdr["RatedCurrent"])
                 * */
            };
            rdr.Close();
            return Record;
        }


    }
}