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
            this.BaseWebBrowser.Navigating += BaseWebBrowser_Navigating;
            this.BaseWebBrowser.Navigated += BaseWebBrowser_Navigated;
        }
        public bool Navigating { get; set; }
        void BaseWebBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Navigating = true;
            ScriptWaitingList.Clear();
        }
        void BaseWebBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Navigating = false;
            foreach (ScriptClass SingleScript in ScriptWaitingList)
            {
                JavaScript(SingleScript.Script, SingleScript.Args);
            }
            ScriptWaitingList.Clear();
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
            fs.Close();
            this.BaseWebBrowser.NavigateToString(TemplateString);
        }
        public class ScriptClass
        {
            public string Script { get; set; }
            public object[] Args { get; set; }
        }
        public List<ScriptClass> ScriptWaitingList = new List<ScriptClass>();
        public bool JavaScript(string script,params object [] args)
        {
            try
            {
                this.BaseWebBrowser.InvokeScript(script, args);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void PureJavaScript(string script)
        {
            this.BaseWebBrowser.Navigate("javascript:"+script);
        }
    }
}
