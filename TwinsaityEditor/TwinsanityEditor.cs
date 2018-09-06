using System;
using System.Windows.Forms;

namespace TwinsaityEditor
{
    internal static class TwinsanityEditor
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main()
        {
            //Init OpenTK
            //OpenTK.ToolkitOptions to = new OpenTK.ToolkitOptions { EnableHighResolution = false, Backend = OpenTK.PlatformBackend.PreferNative };
            //OpenTK.Toolkit.Init(to);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*try
            {*/
                Application.Run(new MainForm());
            /*}catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Unhandled exception occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
    }
}
