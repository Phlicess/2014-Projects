using System.Diagnostics;

namespace WHC.Framework.Commons
{
    /// <summary>
    /// Pdf2Swf 将pdf转化为swf
    /// </summary>
    public class Pdf2Swf
    {
        public Pdf2Swf()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public void PDFConvertToSWF(string sourcePath, string targetPath)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe ";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string cmd = "pdf2swf.exe" + " " + sourcePath + " -o " + targetPath;
            p.StandardInput.WriteLine(cmd);
            p.Close();
        }
    }

}
