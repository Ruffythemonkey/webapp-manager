using System.Diagnostics;

namespace FoxyWebAppManager.Extensions;

public static class ProcessExtensions
{
    extension(Process proc)
    {
        /// <summary>
        /// Open Folder in Explorer
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task OpenPathInExplorer(string path)
        {

            try
            {
                proc.StartInfo.Verb = "explore";
                proc.StartInfo.FileName = path;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }
            catch (Exception ex)
            {
               await ex.ShowMessageUIAsync(); 
            }
        }
    }
}