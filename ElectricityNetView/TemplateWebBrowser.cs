using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
namespace ElectricityNetView
{
    public class TemplateWebBrowser
    {
        public WebBrowser BaseWebBrowser { get; protected set; }
        public TemplateWebBrowser(WebBrowser BaseWebBrowser)
        {
            this.BaseWebBrowser = BaseWebBrowser;
        }
        public void NavigateToTemplate(string TemplateFile)
        {
            TemplateFile = @".\" + TemplateFile;
            string Folder = Path.GetDirectoryName(TemplateFile);
            FileStream fs = new FileStream(TemplateFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string TemplateString = sr.ReadToEnd();
            int start, end;
            string filename;
            while (true)
            {
                start = TemplateString.IndexOf("<Include>");
                if (start < 0)
                    break;
                start += "<Include>".Length;
                end = TemplateString.IndexOf("</Include>");
                filename = (TemplateString.Substring(start, end - start));
                fs = new FileStream(Path.Combine(Folder,filename), FileMode.Open);
                sr = new StreamReader(fs);
                TemplateString = TemplateString.Replace("<Include>" + filename + "</Include>", sr.ReadToEnd());
            }
            this.BaseWebBrowser.NavigateToString(TemplateString);
        }
        public void JavaScript(string script,params object [] args)
        {
            this.BaseWebBrowser.InvokeScript(script, args);
        }
        public void PureJavaScript(string script)
        {
            this.BaseWebBrowser.Navigate("javascript:"+script);
        }
    }
}
