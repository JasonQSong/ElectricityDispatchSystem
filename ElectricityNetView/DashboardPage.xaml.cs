using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace ElectricityNetView
{
    /// <summary>
    /// DashboardPage.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardPage : Page
    {
        public System.Windows.Threading.DispatcherTimer TimerRefresh;
        public TemplateWebBrowser TemplateWebBrowserChart { get; protected set; }
        public DashboardPage()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            MapCenter = new Point(121, 31);
            TemplateWebBrowserChart = new TemplateWebBrowser(WebBrowserChart);
            BackGroundWorkerDownloadMap.RunWorkerCompleted += BackGroundWorkerDownloadMap_RunWorkerCompleted;
            BackGroundWorkerDownloadMap.DoWork += BackGroundWorkerDownloadMap_DoWork;
            TimerRefresh = new System.Windows.Threading.DispatcherTimer();
            TimerRefresh.Tick += TimerRefresh_Tick;
            TimerRefresh.Interval = TimeSpan.FromSeconds(1);
        }

        Random RandomSeed = new Random();
        void TimerRefresh_Tick(object sender, EventArgs e)
        {
            UpdateAll();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadImage();
            TemplateWebBrowserChart.NavigateToTemplate(@"chart/template.html");
            Login();
            FetchAll();
            TimerRefresh.Start();
        }

        private void ButtonDisplayNonproduct_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("3Dtravel.exe");
        }

        private void TestDegreePerPx(double x, double y)
        {
            //x=121.000172;121.000747;121.0001321;...;121.500348;121.500923;121.5001498;
            //y=31;
            //x=122.530597;122.531172;
            //y=52.983967
            //x=112.014242;112.014817;
            //y=3.852553
            //x=121
            //y=31.000154;31.000162;31.000649;31.000657;31.0001145;...31.400465;31.400472;31.400958;31.400966;31.401451;
            //x=112.014242
            //y=3.852553,3.852664;3.852673;3.853242;
            string basefile = "", file = "";
            string url = String.Format("http://api.map.baidu.com/staticimage?width=30&height=1024&center={0},{1}&zoom=12", x.ToString("0.000000"), y.ToString("0.000000"));
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            basefile = String.Format("{0}-{1}.png", x.ToString("0.000000"), y.ToString("0.000000"));
            myWebClient.DownloadFile(url, basefile);
            int bits = 1000;
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < 10; i++)
                {
                    y += 1.0 / bits;
                    url = String.Format("http://api.map.baidu.com/staticimage?width=30&height=1024&center={0},{1}&zoom=12", x.ToString("0.000000"), y.ToString("0.000000"));
                    myWebClient = new System.Net.WebClient();
                    file = String.Format("{0}-{1}.png", x.ToString("0.000000"), y.ToString("0.000000"));
                    myWebClient.DownloadFile(url, file);
                    if (!CompareFile(basefile, file))
                    {
                        y -= 1.0 / bits;
                        bits *= 10;
                        break;
                    }
                }
            }
        }
        private bool CompareFile(string basefile, string file)
        {
            using (FileStream fs1 = new FileStream(basefile, FileMode.Open), fs2 = new FileStream(file, FileMode.Open))
            {
                if (fs1.Length != fs2.Length)
                    return false;
                else
                {
                    int file1byte, file2byte;
                    do
                    {
                        // 从每一个文件读取一个字节。
                        file1byte = fs1.ReadByte();
                        file2byte = fs2.ReadByte();
                    }
                    while ((file1byte == file2byte) && (file1byte != -1));
                    if (file1byte != file2byte)
                        return false;
                }
                return true;
            }
        }
        public void DownloadMap(string filename)
        {
            BackGroundWorkerDownloadMap.RunWorkerAsync();
        }
        private double _HDegreePerPxBase = 0.000575;
        private double _VDegreePerPxBase = 0.000495;
        private int _MapZoomBase = 12;
        public double HDegreePerPx
        {
            get
            {
                return this._HDegreePerPxBase / Math.Pow(2, (MapZoom - _MapZoomBase));
            }
        }
        public double VDegreePerPx
        {
            get
            {
                return this._VDegreePerPxBase / Math.Pow(2, (MapZoom - _MapZoomBase));
            }
        }
        public Image MapImage { get; set; }
        private Point _MapCenter = new Point(121, 31);
        public Point MapCenter
        {
            get { return this._MapCenter; }
            set
            {
                Point Offset = new Point((this._MapCenter.X - value.X) / HDegreePerPx, (value.Y - this._MapCenter.Y) / VDegreePerPx);
                foreach (UIElement fe in CanvasMap.Children)
                {
                    double Left = Canvas.GetLeft(fe);
                    double Top = Canvas.GetTop(fe);
                    Left += Offset.X;
                    Top += Offset.Y;
                    Canvas.SetLeft(fe, Left);
                    Canvas.SetTop(fe, Top);
                }
                this._MapCenter = value;
                ReloadImage();
            }
        }
        private int _MapZoom = 12;
        public int MapZoom
        {
            get { return this._MapZoom; }
            set
            {
                double scale = Math.Pow(2, value - this._MapZoom);
                ScaleTransform st = CanvasMap.RenderTransform as ScaleTransform;
                st.ScaleX *= scale;
                st.ScaleY *= scale;
                this._MapZoom = value;
                ReloadImage();
            }
        }
        private void ReloadImage()
        {
            string filename = GetFileName(MapCenter.X, MapCenter.Y, MapZoom);
            if (File.Exists(filename))
            {
                try
                {
                    ImageFake.Source = new BitmapImage(new Uri(filename));
                    Canvas.SetLeft(ImageFake, 0);
                    Canvas.SetTop(ImageFake, 0);
                    ScaleTransform st = CanvasMap.RenderTransform as ScaleTransform;
                    st.ScaleX = 1;
                    st.ScaleY = 1;
                    DrawStations();
                }
                catch (Exception) { }
            }
            else
            {
                if (!BackGroundWorkerDownloadMap.IsBusy)
                    BackGroundWorkerDownloadMap.RunWorkerAsync();
            }
        }
        System.ComponentModel.BackgroundWorker BackGroundWorkerDownloadMap = new System.ComponentModel.BackgroundWorker();
        void BackGroundWorkerDownloadMap_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ReloadImage();
        }

        void BackGroundWorkerDownloadMap_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                if (!Directory.Exists("./Map"))
                    System.IO.Directory.CreateDirectory("./Map");
                string filename = GetFileName(MapCenter.X, MapCenter.Y, MapZoom);
                if (File.Exists(filename))
                    break;
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                //myWebClient.Proxy = new System.Net.WebProxy("http://cache.sjtu.edu.cn:8080");
                string api = String.Format(MapAPIFormat, MapCenter.X, MapCenter.Y, MapZoom);
                if (!File.Exists(filename))
                {
                    this.Dispatcher.Invoke(new Action(() => { WriteLine("[BGW_DownloadMap]正在下载地图：{0}", System.IO.Path.GetFileName(filename)); }));
                    myWebClient.DownloadFile(api, filename);
                    this.Dispatcher.Invoke(new Action(() => { WriteLine("[BGW_DownloadMap]地图下载完成：{0}", System.IO.Path.GetFileName(filename)); }));
                }
            }
        }

        String MapAPIFormat = "http://api.map.baidu.com/staticimage?copyright=1&width=1023&height=1023&center={0},{1}&zoom={2}";
        String MapFileFormat = "./Map/{0}.{1}.{2}.png";
        private String GetFileName(double x, double y, int MapZoom)
        {
            return System.IO.Path.GetFullPath(String.Format(MapFileFormat, x.ToString("0.000000"), y.ToString("0.000000"), MapZoom));
        }
        private void ButtonParaLabel_Click(object sender, RoutedEventArgs e)
        {
            double x, y;
            bool bx = double.TryParse(TextBoxLabel.Text.Substring(0, TextBoxLabel.Text.IndexOf(',')), out x);
            bool by = double.TryParse(TextBoxLabel.Text.Substring(TextBoxLabel.Text.IndexOf(',') + 1), out y);
            if (bx && by)
                SetLabel(x, y);
        }
        List<TextBlock> Labels = new List<TextBlock>();
        private void SetLabel(double x, double y)
        {
            int dx = (int)((x - MapCenter.X) / HDegreePerPx);
            int dy = (int)((y - MapCenter.Y) / VDegreePerPx);
            TextBlock tb = new TextBlock();
            tb.Text = x.ToString() + "," + y.ToString();
            CanvasMap.Children.Add(tb);
            MessageBox.Show((x - MapCenter.X).ToString() + " " + dx.ToString());
            Canvas.SetLeft(tb, dx + 512);
            Canvas.SetTop(tb, dy + 512);
        }

        private void ButtonPushChart_Click(object sender, RoutedEventArgs e)
        {
            TemplateWebBrowserChart.JavaScript("pushdata", 0, RandomSeed.Next() % 30);
            TemplateWebBrowserChart.JavaScript("pushdata", 1, RandomSeed.Next() % 30);
            TemplateWebBrowserChart.JavaScript("pushdata", 2, RandomSeed.Next() % 30);
            TemplateWebBrowserChart.JavaScript("pushdata", 3, RandomSeed.Next() % 30);
        }
        Point PointBefore = new Point(0, 0);
        private void CanvasMap_MouseMove(object sender, MouseEventArgs e)
        {
            Point PointNow = e.GetPosition(CanvasMap);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double dx = (PointNow.X - PointBefore.X) * HDegreePerPx;
                double dy = (PointNow.Y - PointBefore.Y) * VDegreePerPx;
                MapCenter = new Point(MapCenter.X - dx, MapCenter.Y + dy);
            }
            PointBefore = PointNow;
        }

        private void ButtonExampleBus_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.InitialDirectory = System.IO.Path.GetFullPath(@".\example\data");
            ofd.FileName = @"BUS.txt";
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;
            LoadBusFromFile(ofd.FileName);
        }
        private void LoadBusFromFile(string filename)
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                List<ElectricityService.ConfigStationInformation> StationList = new List<ElectricityService.ConfigStationInformation>();
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
                string line;
                string[] paras;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    paras = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    if (paras.Length == 5)
                    {
                        ElectricityService.ConfigStationInformation tmpStation = new ElectricityService.ConfigStationInformation();
                        tmpStation.StationName = paras[1];
                        tmpStation.Longitude = double.Parse(paras[2]);
                        tmpStation.Latitude = double.Parse(paras[3]);
                        tmpStation.VoltageLevel = double.Parse(paras[4]);
                        StationList.Add(tmpStation);
                    }
                }
                foreach (ElectricityService.ConfigStationInformation tmpStation in StationList)
                {
                    esc.AddConfigStationInformation(tmpStation);
                }
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }
        private void ButtonExampleLine_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.InitialDirectory = System.IO.Path.GetFullPath(@".\example\data");
            ofd.FileName = @"Line.txt";
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;
            LoadLineFromFile(ofd.FileName);
        }
        private void LoadLineFromFile(string filename)
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                List<ElectricityService.ConfigLineInformation> LineList = new List<ElectricityService.ConfigLineInformation>();
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
                string line;
                string[] paras;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    paras = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    if (paras.Length == 19)
                    {
                        int StationID_Start = esc.FindConfigStationInformationByStationName(paras[1]).ID;
                        int StationID_End = esc.FindConfigStationInformationByStationName(paras[2]).ID;
                        ElectricityService.ConfigLineInformation tmpLine = new ElectricityService.ConfigLineInformation();
                        tmpLine.LineName = paras[0];
                        tmpLine.StationID_Start = StationID_Start;
                        tmpLine.StationID_End = StationID_End;
                        tmpLine.VoltageLevel = double.Parse(paras[3]);
                        LineList.Add(tmpLine);
                    }
                }
                foreach (ElectricityService.ConfigLineInformation tmpLine in LineList)
                {
                    esc.AddConfigLineInformation(tmpLine);
                }
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }

        private void ButtonExampleDay_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Multiselect = true;
            ofd.InitialDirectory = System.IO.Path.GetFullPath(@".\example\his");
            ofd.FileName = "";
            for (int i = 0; i < 15; i++)
            {
                ofd.FileName += String.Format("\"day{0}.txt\" ", i);
            }
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;
            LoadDaysFromFiles(ofd.FileNames, DatePickerExampleDay.SelectedDate ?? DateTime.Now);
        }
        private void LoadDaysFromFiles(string[] filenames, DateTime StartDate)
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                for (int i = 0; i < filenames.Length; i++)
                {
                    DateTime TimeReady = StartDate + TimeSpan.FromDays(i);
                    string filename = filenames[i];
                    FileStream fs = new FileStream(filename, FileMode.Open);
                    List<ElectricityService.RuntimeStationData> DataList = new List<ElectricityService.RuntimeStationData>();
                    StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
                    string line;
                    string[] paras;
                    int istationid = 1, jrecordid = 0;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        paras = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                        if (paras.Length == 3)
                        {
                            ElectricityService.RuntimeStationData tmpData = new ElectricityService.RuntimeStationData()
                            {
                                StationID = istationid,
                                ActivePower = double.Parse(paras[0]),
                                ReactivePower = double.Parse(paras[1]),
                                Time = TimeReady + TimeSpan.FromMinutes(15 * jrecordid)
                            };
                            DataList.Add(tmpData);
                            jrecordid++;
                            if (jrecordid >= 96)
                            {
                                jrecordid = 0;
                                istationid++;
                            }
                        }
                    }
                    foreach (ElectricityService.RuntimeStationData tmpData in DataList)
                    {
                        esc.AddRuntimeStationData(tmpData);
                    }
                }
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }
        private void Forecast(int StationID, DateTime TargetDate)
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                this.Dispatcher.Invoke(new Action(() => { WriteLine("[BGW_ForeCast]正在生成预测数据：{0}...",StationID); }));
                esc.Forecast(StationID, DateTime.Now);
                this.Dispatcher.Invoke(new Action(() => { WriteLine("[BGW_ForeCast]预测数据生成完毕：{0}", StationID); }));
                this.Dispatcher.Invoke(new Action(() => { WriteLine("[UI]正在填充预测数据：{0}...", StationID); }));
                if (RadioForecastPointToPoint.IsChecked??false)
                {
                    TemplateWebBrowserChart.JavaScript("DeleteData", "PointToPoint");
                    List<ElectricityService.ForecastDayStationData> DataList = esc.SelectForecastDayStationData(StationID, DateTime.Now, 1).ToList();
                    foreach (ElectricityService.ForecastDayStationData record in DataList)
                    {
                        TemplateWebBrowserChart.JavaScript("AddData", "PointToPoint", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ActivePower);
                    }
                }
                if (RadioForecastSmooth.IsChecked ?? false)
                {
                    TemplateWebBrowserChart.JavaScript("DeleteData", "Smooth");
                    List<ElectricityService.ForecastDayStationData> DataList = esc.SelectForecastDayStationData(StationID, DateTime.Now, 2).ToList();
                    foreach (ElectricityService.ForecastDayStationData record in DataList)
                    {
                        TemplateWebBrowserChart.JavaScript("AddData", "Smooth", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ActivePower);
                    }
                }
                if (RadioForecastDayGray.IsChecked ?? false)
                {
                    TemplateWebBrowserChart.JavaScript("DeleteData", "DayGray");
                    List<ElectricityService.ForecastDayStationData> DataList = esc.SelectForecastDayStationData(StationID, DateTime.Now, 3).ToList();
                    foreach (ElectricityService.ForecastDayStationData record in DataList)
                    {
                        TemplateWebBrowserChart.JavaScript("AddData", "DayGray", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ActivePower);
                    }
                }
                if (RadioForecastVariationCoefficient.IsChecked ?? false)
                {
                    TemplateWebBrowserChart.JavaScript("DeleteData", "VariationCoefficient");
                    List<ElectricityService.ForecastDayStationData> DataList = esc.SelectForecastDayStationData(StationID, DateTime.Now, 4).ToList();
                    foreach (ElectricityService.ForecastDayStationData record in DataList)
                    {
                        TemplateWebBrowserChart.JavaScript("AddData", "VariationCoefficient", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ActivePower);
                    }
                }
                this.Dispatcher.Invoke(new Action(() => { WriteLine("[UI]填充预测数据完毕：{0}", StationID); }));
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }
        private void ListViewStationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TemplateWebBrowserChart.JavaScript("DeleteData", "runtime");
            if (ListViewStationList.SelectedItem == null)
                return;
            StationUI stationui = ListViewStationList.SelectedItem as StationUI;
            MapCenter = new Point(stationui.Longitude, stationui.Latitude);
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                this.Dispatcher.Invoke(new Action(() => { WriteLine("[UI]正在请求当日历史数据：{0}", stationui.ID); }));
                List<ElectricityService.RuntimeStationData> DataList = esc.SelectRuntimeStationData(stationui.ID, DateTime.Today).ToList();
                this.Dispatcher.Invoke(new Action(() => { WriteLine("[UI]当日历史数据请求完毕：{0}", stationui.ID); }));
                if (DataList.Count == 0)
                    return;
                foreach (ElectricityService.RuntimeStationData record in DataList)
                {
                    if (record.Time > DateTime.Now)
                        break;
                    TemplateWebBrowserChart.JavaScript("AddData", "runtime", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ActivePower);
                }
                Forecast(stationui.ID, DateTime.Today);
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }

        private void ConsoleOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConsoleOutput.ScrollToEnd();
        }

        private void ConsoleInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                ConsoleOutput.Text += ConsoleInput.Text + Environment.NewLine;
                string cmd = ConsoleInput.Text;
                cmd = cmd.Replace(',', ' ');
                cmd.Trim();
                if (cmd != "")
                {
                    string[] args = System.Text.RegularExpressions.Regex.Split(cmd, @"\s+");
                    switch (args[0])
                    {
                        case "MapCenter":
                            {
                                double x, y;
                                if (args.Count() != 3)
                                {
                                    WriteLine("MapCenter require 2 arguments");
                                    return;
                                }
                                if (!double.TryParse(args[1], out x))
                                {
                                    WriteLine("Cannot parse {0} to double", args[1]);
                                    return;
                                }
                                if (!double.TryParse(args[2], out y))
                                {
                                    WriteLine("Cannot parse {0} to double", args[2]);
                                    return;
                                }
                                MapCenter = new Point(x, y);
                            }
                            break;
                    }
                }
                ConsoleInput.Text = "";
            }
        }

        private void CanvasMap_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MapZoom += e.Delta / 120;
        }
        private void WriteLine(string format, params object[] args)
        {
            ConsoleOutput.Text += DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + String.Format(format, args) + Environment.NewLine;
        }
        List<StationUI> StationUIList = new List<StationUI>();
        private void Login()
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                List<ElectricityService.ConfigStationInformation> StationList=esc.SelectConfigStationInformation().ToList();
                foreach (ElectricityService.ConfigStationInformation record in StationList)
                {
                    StationUIList.Add(new StationUI()
                    {
                        ID=record.ID,
                        StationName=record.StationName,
                        Longitude=record.Longitude,
                        Latitude=record.Latitude,
                        BuildTime=record.BuildTime,
                        VoltageLevel=record.VoltageLevel,
                        InstallCapacity=record.InstallCapacity
                    });
                }
                ListViewStationList.Items.Clear();
                foreach(StationUI stationui in StationUIList)
                    ListViewStationList.Items.Add(stationui);
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }
        private void FetchAll()
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                foreach (StationUI stationui in StationUIList)
                {
                    List<ElectricityService.RuntimeStationData> DataList= esc.SelectRuntimeStationData(stationui.ID, DateTime.Today).ToList();
                    if (DataList.Count == 0)
                        return;
                    ElectricityService.RuntimeStationData LastRecord=null;
                    foreach (ElectricityService.RuntimeStationData record in DataList)
                    {
                        if (record.Time > DateTime.Now)
                            break;
                        LastRecord = record;
                    }
                    if (LastRecord != null)
                    {
                        stationui.Active = LastRecord.ActivePower;
                        stationui.Reactive = LastRecord.ReactivePower;
                        stationui.RuntimeID = LastRecord.ID;
                    }
                }
                ListViewStationList.UpdateLayout();
                DrawStations();
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }
        private void UpdateAll()
        {
            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
                foreach (StationUI stationui in StationUIList)
                {
                    List<ElectricityService.RuntimeStationData> DataList = esc.UpdateRuntimeStationData(stationui.RuntimeID,stationui.ID).ToList();
                    if (DataList.Count == 0)
                        return;
                    ElectricityService.RuntimeStationData LastRecord = null;
                    foreach (ElectricityService.RuntimeStationData record in DataList)
                    {
                        if (record.Time > DateTime.Now)
                            break;
                        if (ListViewStationList.SelectedItem ==stationui)
                        {
                            TemplateWebBrowserChart.JavaScript("AddData", "runtime", record.Time.ToString("yyyy-MM-dd HH:mm:ss"), record.ActivePower);
                        }
                        LastRecord = record;
                    }
                    if (LastRecord != null)
                    {
                        stationui.Active = LastRecord.ActivePower;
                        stationui.Reactive = LastRecord.ReactivePower;
                        stationui.RuntimeID = LastRecord.ID;
                        this.Dispatcher.Invoke(new Action(() => { WriteLine("[BGW_UpdateRuntime]数据已更新：{0}", stationui.ID); }));
                    }
                }
                ListViewStationList.UpdateLayout();
                esc.Close();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("服务器请求超时");
                esc.Abort();
            }
        }
        private void CheckBoxForecast_Checked(object sender, RoutedEventArgs e)
        {
            StationUI stationui = ListViewStationList.SelectedItem as StationUI;
            Forecast(stationui.ID,DateTime.Today);
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            Directory.Delete("./Map");
        }
        private void DrawStations()
        {
            Image IF = ImageFake;
            CanvasMap.Children.Clear();
            CanvasMap.Children.Add(IF);
            foreach (StationUI stationui in StationUIList)
            {
                Ellipse circle = new Ellipse();
                TextBlock tb = new TextBlock();
                circle.Opacity = 0.5;
                tb.Text = String.Format("{0},{1}", stationui.Longitude, stationui.Latitude);
                int dx = (int)((stationui.Longitude - MapCenter.X) / HDegreePerPx);
                int dy = (int)((stationui.Latitude - MapCenter.Y) / VDegreePerPx);
                double r = stationui.Active * MapZoom;
                if (r > 0)
                {
                    circle.Fill = Brushes.Green;
                }
                else
                {
                    r = -r;
                    circle.Fill = Brushes.Red;
                }
                r = Math.Log10(r)*10;
                circle.Width = 2 * r;
                circle.Height = 2 * r;
                CanvasMap.Children.Add(circle);
                CanvasMap.Children.Add(tb);
                Canvas.SetLeft(circle, dx + 512-r);
                Canvas.SetTop(circle, -dy + 512 - r);
                Canvas.SetLeft(tb, dx + 512);
                Canvas.SetTop(tb,- dy + 512);
            }

        }
    }
    public class StationUI
    {
        public int ID { get; set; }
        public string StationName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime BuildTime { get; set; }
        public double VoltageLevel { get; set; }
        public double InstallCapacity { get; set; }

        public double Active { get; set; }
        public double Reactive { get; set; }

        public int RuntimeID { get; set; }
    }
}