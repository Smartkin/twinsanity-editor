using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TwinsaityEditor
{
    static class TwinsanityEditor
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Init OpenTK
            OpenTK.ToolkitOptions to = new OpenTK.ToolkitOptions { EnableHighResolution = false, Backend = OpenTK.PlatformBackend.PreferNative };
            OpenTK.Toolkit.Init(to);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*try
            {*/
                Application.Run(new TwinsanityEditorForm());
            /*}catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Unhandled exception occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
    }
}
