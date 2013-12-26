using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ElectricityWcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ElectricityService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 ElectricityService.svc 或 ElectricityService.svc.cs，然后开始调试。
    public class ElectricityService : IElectricityService
    {
        public string DMain()
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.DMain();
        }
        public int AddLogUser(LogUser Record)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.AddLogUser(Record);
        }
        public int AddConfigLineInformation(ConfigLineInformation Record)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.AddConfigLineInformation(Record);
        }
        public int AddConfigStationInformation(ConfigStationInformation Record)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.AddStationLineInformation(Record);
        }
        public int AddRuntimeLineData(RuntimeLineData Record)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.AddRuntimeLineData(Record);
        }
        public int AddRuntimeStationData(RuntimeStationData Record)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.AddRuntimeStationData(Record);
        }
        public int AddForecastDayStationData(ForecastDayStationData Record)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.AddForecastDayStationData(Record);
        }
        public LogUser FindLogUser(int ID)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.FindLogUser(ID);
        }
        public ConfigLineInformation FindConfigLineInformation(int ID)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.FindConfigLineInformation(ID);
        }
        public ConfigStationInformation FindConfigStationInformation(int ID)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.FindConfigStationInformation(ID);
        }
        public ConfigStationInformation FindConfigStationInformationByStationName(string StationName)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.FindConfigStationInformationByStationName(StationName);
        }
        public RuntimeLineData FindRuntimeLineData(int ID) 
        { 
            DatabaseConnector dc = new DatabaseConnector();
            return dc.FindRuntimeLineData(ID);
        }
        public RuntimeStationData FindRuntimeStationData(int ID)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.FindRuntimeStationData(ID);
        }
        public List<ConfigStationInformation> SelectConfigStationInformation()
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.SelectConfigStationInformation();
        }
        public List<RuntimeStationData> SelectRuntimeStationData(int StationID, DateTime TargetDate)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.SelectRuntimeStationData(StationID,TargetDate);
        }
        public List<ForecastDayStationData> SelectForecastDayStationData(int StationID, DateTime TargetDate,int Forecast)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.SelectForecastDayStationData(StationID, TargetDate,Forecast);
        }
        public List<RuntimeStationData> UpdateRuntimeStationData(int StartID, int StationID)
        {
            DatabaseConnector dc = new DatabaseConnector();
            return dc.UpdateRuntimeStationData(StartID,StationID);
        }
        public void StationAlert(int ID)
        {
        }
        public void Forecast(int StationID, DateTime TargetDate)
        {
            Controller ctrl = new Controller();
            ctrl.Forecast(StationID, TargetDate);
        }
    }
}
