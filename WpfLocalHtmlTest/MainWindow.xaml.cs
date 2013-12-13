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

namespace WpfLocalHtmlTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream(TextBoxTemplate.Text, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string TemplateString = sr.ReadToEnd();
            TemplateString=TemplateString.Replace("<Data/>",TextBoxData.Text);
            int start,end;
            string filename;
            while(true){
                start=TemplateString.IndexOf("<Include>");
                if (start < 0)
                    break;
                start += "<Include>".Length;
                end=TemplateString.IndexOf("</Include>");
                filename = (TemplateString.Substring(start, end - start));
                fs = new FileStream(filename, FileMode.Open);
                sr = new StreamReader(fs);
                TemplateString = TemplateString.Replace("<Include>" + filename + "</Include>", sr.ReadToEnd());
            }
            this.WebBrowserTest.NavigateToString(TemplateString);
            MessageBox.Show(TemplateString);
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("这是一个内置网页的测试程序，点击Refresh后将会寻找template输入框的文件，把其中的<Data/>节点替换成Data输入框中的内容，<Include>filename</Include>节点替换为包含的文件，仅仅简单的字符串替换所以不支持XML节点格式");
        }
    }
}
