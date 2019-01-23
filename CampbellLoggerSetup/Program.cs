// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Duke Energy">
//   author: Kristopher Tyler Church
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CampbellLoggerSetup
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.IO.IsolatedStorage;


    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
           var args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                FreeConsole();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormCampbell());
            }
            else
            {
                var poll = new PollingClass(args);
                poll.StartPoll();
            }
        }
    }
}