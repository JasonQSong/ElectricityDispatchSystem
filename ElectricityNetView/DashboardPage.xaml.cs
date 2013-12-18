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
            MapZoom = 12;
            HDegreePerPx = 0.000575;
            VDegreePerPx = 0.000495;
            TemplateWebBrowserChart = new TemplateWebBrowser(WebBrowserChart);
            TimerRefresh = new System.Windows.Threading.DispatcherTimer();
            TimerRefresh.Tick += TimerRefresh_Tick;
            BackGroundWorkerDownloadMap.DoWork += BackGroundWorkerDownloadMap_DoWork;
            BackGroundWorkerDownloadMap.RunWorkerCompleted += BackGroundWorkerDownloadMap_RunWorkerCompleted;
        }

        Random RandomSeed = new Random();
        void TimerRefresh_Tick(object sender, EventArgs e)
        {

            TemplateWebBrowserChart.JavaScript("pushdata", 0, RandomSeed.Next() % 30);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetCenter(121, 31);
            TemplateWebBrowserChart.NavigateToTemplate(@"chart/template.html");
        }

        private void ButtonDisplayNonproduct_Click(object sender, RoutedEventArgs e)
        {

        }

        System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

        private void ButtonPredictGrey_Click(object sender, RoutedEventArgs e)
        {

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ImageFake.Source = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));

        }
        public double HDegreePerPx { get; protected set; }
        public double VDegreePerPx { get; protected set; }
        private void ButtonParaConfirm_Click(object sender, RoutedEventArgs e)
        {


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
            MessageBox.Show(file);
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
        private void ButtonParaCenter_Click(object sender, RoutedEventArgs e)
        {
            double x, y;
            bool bx = double.TryParse(TextBoxCenter.Text.Substring(0, TextBoxCenter.Text.IndexOf(',')), out x);
            bool by = double.TryParse(TextBoxCenter.Text.Substring(TextBoxCenter.Text.IndexOf(',') + 1), out y);
            if (bx && by)
                SetCenter(x, y);
        }
        public void DownloadMap(string filename)
        {
            BackGroundWorkerDownloadMap.RunWorkerAsync();
        }
        public Image MapImage { get; set; }
        public Point MapCenter { get; set; }
        public int MapZoom { get; set; }
        private void SetCenter(double x, double y)
        {
            double ImageLeft = Canvas.GetLeft(ImageFake);
            ImageLeft += ( MapCenter.X-x) / HDegreePerPx;
            Canvas.SetLeft(ImageFake, ImageLeft);
            double ImageTop = Canvas.GetTop(ImageFake);
            ImageTop += (y-MapCenter.Y ) / VDegreePerPx;
            Canvas.SetTop(ImageFake, ImageTop);
            MapCenter = new Point(x, y);
            string filename = GetFileName(MapCenter.X, MapCenter.Y, MapZoom);
            if (File.Exists(filename))
            {
                try
                {
                    ImageFake.Source = new BitmapImage(new Uri(filename));
                    Canvas.SetLeft(ImageFake, 0);
                    Canvas.SetTop(ImageFake, 0);
                }
                catch (Exception) { }
            }
            else
            {
                if(!BackGroundWorkerDownloadMap.IsBusy)
                BackGroundWorkerDownloadMap.RunWorkerAsync();
            }
        }
        System.ComponentModel.BackgroundWorker BackGroundWorkerDownloadMap = new System.ComponentModel.BackgroundWorker();
        void BackGroundWorkerDownloadMap_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            string filename = GetFileName(MapCenter.X, MapCenter.Y, MapZoom);
            if (File.Exists(filename))
            {
                ImageFake.Source = new BitmapImage(new Uri(filename));
                Canvas.SetLeft(ImageFake, 0);
                Canvas.SetTop(ImageFake, 0);
            }
            else
            {
                if (!BackGroundWorkerDownloadMap.IsBusy)
                    BackGroundWorkerDownloadMap.RunWorkerAsync();
            }
        }

        void BackGroundWorkerDownloadMap_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                string filename = GetFileName(MapCenter.X, MapCenter.Y, MapZoom);
                if (File.Exists(filename))
                    break;
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                string api = String.Format(MapAPIFormat, MapCenter.X, MapCenter.Y, MapZoom);
                if (!File.Exists(filename))
                    myWebClient.DownloadFile(api, filename);
            }
        }

        String MapAPIFormat = "http://api.map.baidu.com/staticimage?copyright=1&width=1023&height=1023&center={0},{1}&zoom={2}";
        String MapFileFormat = "./{0}.{1}.{2}.png";
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
        Point PointBefore=new Point(0,0);
        private void CanvasMap_MouseMove(object sender, MouseEventArgs e)
        {
            Point PointNow = e.GetPosition(e.Source as FrameworkElement);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double dx = (PointNow.X - PointBefore.X) * HDegreePerPx;
                double dy = (PointNow.Y - PointBefore.Y) * VDegreePerPx;
                SetCenter(MapCenter.X - dx, MapCenter.Y +dy);
                PointBefore.Offset(dx, dy);
            }
            PointBefore = PointNow;
        }

        private void ButtonExampleBus_Click(object sender, RoutedEventArgs e)
        {
            /*
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
             * */
            List<ElectricityService.ConfigStationInformation> StationList = new List<ElectricityService.ConfigStationInformation>();
            FileStream fs = new FileStream(@"data\data\BUS.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs,Encoding.GetEncoding("gb2312"));
            string line;
            string[] paras;
            while (!sr.EndOfStream)
            {
                line=sr.ReadLine();
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

            ElectricityService.ElectricityServiceClient esc = new ElectricityService.ElectricityServiceClient();
            try
            {
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

        }

        private void ButtonExampleG_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExampleHis_Click(object sender, RoutedEventArgs e)
        {

        }

    }
    public class ElectricStation
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string Location { get; set; }
        public int Product { get; set; }
    }
}
