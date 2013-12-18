using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ElectricityWcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IElectricityService”。
    [ServiceContract]
    public interface IElectricityService
    {
        [OperationContract]
        int AddLogUser(LogUser Record);
        [OperationContract]
        int AddConfigLineInformation(ConfigLineInformation Record);
        [OperationContract]
        int AddConfigStationInformation(ConfigStationInformation Record);
        [OperationContract]
        int AddRuntimeLineData(RuntimeLineData Record);
        [OperationContract]
        int AddRuntimeStationData(RuntimeStationData Record);

        [OperationContract]
        LogUser FindLogUser(int ID);
        [OperationContract]
        ConfigLineInformation FindConfigLineInformation(int ID);
        [OperationContract]
        ConfigStationInformation FindConfigStationInformation(int ID);
        [OperationContract]
        RuntimeLineData FindRuntimeLineData(int ID);
        [OperationContract]
        RuntimeStationData FindRuntimeStationData(int ID);
        [OperationContract]
        void StationAlert(int ID);
    }
    [DataContract]
    public class LogUser
    {
        int _ID = 0;
        DateTime _Time =DateTime.Now;
        int _UserID = 0;
        string _Action = "";
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
        [DataMember]
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        [DataMember]
        public string Action
        {
            get { return _Action; }
            set { _Action = value; }
        }
    }
    [DataContract]
    public class ConfigLineInformation
    {
        int _ID = 0;
        string _LineName = "";
        int _StationID_Start = 0;
        int _StationID_End = 0;
        double _VoltageLevel;
        double _RatedCurrent;
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public string LineName
        {
            get { return _LineName; }
            set { _LineName = value; }
        }
        [DataMember]
        public int StationID_Start
        {
            get { return _StationID_Start; }
            set { _StationID_Start = value; }
        }
        [DataMember]
        public int StationID_End
        {
            get { return _StationID_End; }
            set { _StationID_End = value; }
        }
        [DataMember]
        public double VoltageLevel
        {
            get { return _VoltageLevel; }
            set { _VoltageLevel = value; }
        }
        [DataMember]
        public double RatedCurrent
        {
            get { return _RatedCurrent; }
            set { _RatedCurrent = value; }
        }
    }
    [DataContract]
    public class ConfigStationInformation
    {
        int _ID = 0;
        string _StationName = "";
        double _Longitude = 0;
        double _Latitude = 0;
        DateTime _BuildTime;
        double _VoltageLevel;
        double _InstallCapacity;
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public string StationName
        {
            get { return _StationName; }
            set { _StationName = value; }
        }
        [DataMember]
        public double Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }
        [DataMember]
        public double Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }
        [DataMember]
        public DateTime BuildTime
        {
            get { return _BuildTime; }
            set { _BuildTime = value; }
        }
        [DataMember]
        public double VoltageLevel
        {
            get { return _VoltageLevel; }
            set { _VoltageLevel = value; }
        }
        [DataMember]
        public double InstallCapacity
        {
            get { return _InstallCapacity; }
            set { _InstallCapacity = value; }
        }
    }
    [DataContract]
    public class RuntimeLineData
    {
        int _ID = 0;
        int _LineID = 0;
        double _LoadQuantity = 0;
        DateTime _Time = DateTime.Now;
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public int LineID
        {
            get { return _LineID; }
            set { _LineID = value; }
        }
        [DataMember]
        public double LoadQuantity
        {
            get { return _LoadQuantity; }
            set { _LoadQuantity = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
    }
    [DataContract]
    public class RuntimeStationData
    {
        int _ID = 0;
        int _StationID = 0;
        double _ActivePower = 0;
        double _ReactivePower = 0;
        DateTime _Time = DateTime.Now;
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public int StationID
        {
            get { return _StationID; }
            set { _StationID = value; }
        }
        [DataMember]
        public double ActivePower
        {
            get { return _ActivePower; }
            set { _ActivePower = value; }
        }
        public double ReactivePower
        {
            get { return _ReactivePower; }
            set { _ReactivePower = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
    }
}
