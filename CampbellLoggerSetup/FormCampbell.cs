// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Duke Energy" file="FormCampbell.cs">
// Author: Kristopher Tyler Church
// </copyright>
// <summary>
// Description: GUI that takes care of the configuration of the campbell logger
// and also has the capability to poll either all of the loggers
// or just one of the loggers based on which logger is selected
// in the listbox.
// </summary>

// NOTE: config file is stored in AppData\Local\IsolatedStorage\*\*\*\CampbellToAirvision
// 
// --------------------------------------------------------------------------------------------------------------------

namespace CampbellLoggerSetup
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using CampbellLoggerSetup.Properties;

    using Newtonsoft.Json;

    /// <inheritdoc />
    /// <summary>
    /// The form campbell.
    /// </summary>
    public partial class FormCampbell : Form
    {
        /// <summary>
        /// The iso store.
        /// </summary>
        private IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(
            IsolatedStorageScope.User | IsolatedStorageScope.Assembly,
            null,
            null);

        /// <summary>
        /// The file path.
        /// </summary>
        private const string filePath = @"CampbellToAirvision\config.json";

        /// <summary>
        /// The list.
        /// </summary>
        private List<Logger> list = new List<Logger>();

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CampbellLoggerSetup.FormCampbell" /> class.
        /// </summary>
        public FormCampbell()
        {
            this.InitializeComponent();
            this.ClientSize = new Size(590, 400);
            this.progressBar1.Visible = false;
            this.progressBar1.Style = ProgressBarStyle.Marquee;
        }

        /// <summary>
        /// The is valid pb add.
        /// </summary>
        /// <param name="pakAdd">
        /// The pb add.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidPbAdd(int pakAdd)
        {
            var pakAddress = pakAdd;
            return pakAddress > 0 && pakAddress < 4000;
        }

        /// <summary>
        /// The is valid port.
        /// </summary>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidPort(int port)
        {
            var tcpPort = port;
            return tcpPort > 0 && tcpPort < 65536;
        }

        /// <summary>
        /// The is valid sec code.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidSecCode(int code)
        {
            var secCode = code;
            return secCode >= 0 && secCode < 65536;
        }

        /// <summary>
        /// The is valid table num.
        /// </summary>
        /// <param name="num">
        /// The num.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidTableNum(int num)
        {
            var tableNum = num;
            return tableNum > 0 && tableNum < 100;
        }

        /// <summary>
        /// The is valid ip.
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValidIp(string ip)
        {
            var stringIpAddress = ip;

            if (!IPAddress.TryParse(stringIpAddress, out var address))
            {
                return false;
            }

            switch (address.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    return true;
                case AddressFamily.InterNetworkV6:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The update list box.
        /// </summary>
        private void UpdateListBox()
        {
            foreach (var item in this.list)
            {
                if (item.LoggerName.Equals(string.Empty))
                {
                    item.LoggerName = "empty";
                }

                this.loggerListBox.Items.Add(item.LoggerName);
            }
        }

        /// <summary>
        /// The form campbell_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FormCampbellLoad(object sender, EventArgs e)
        {
            if (!this.isoStore.DirectoryExists("CampbellToAirvision"))
            {
                this.isoStore.CreateDirectory("CampbellToAirvision");
            }

            this.LoadList();
        }

        /// <summary>
        /// The load list.
        /// </summary>
        private void LoadList()
        {
            try
            {
                if (this.isoStore.FileExists(filePath))
                {
                    using (var isoStream = new IsolatedStorageFileStream(filePath, FileMode.Open, this.isoStore))
                    {
                        using (var reader = new StreamReader(isoStream))
                        {
                            var fileContent = reader.ReadToEnd();
                            if (fileContent.Equals(string.Empty))
                            {
                                MessageBox.Show(@"config file empty");
                                return;
                            }
                            var json = fileContent;
                            this.list = JsonConvert.DeserializeObject<List<Logger>>(json);
                            this.UpdateListBox();
                        }
                    }
                }
                else
                {
                    using (var isoStream = new IsolatedStorageFileStream(filePath, FileMode.Create, this.isoStore))
                    {
                        using (var writer = new StreamWriter(isoStream))
                        {
                            writer.WriteLine("[]");
                        }
                    }

                    this.list = JsonConvert.DeserializeObject<List<Logger>>("[]");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                // this.Close();
            }
        }

        /// <summary>
        /// The button 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AddButtonClick(object sender, EventArgs e)
        {
            var ipAddress = this.ipAddressTextBox.Text;
            var loggerName = this.loggerNameTextBox.Text;
            int.TryParse(this.pbAddressTextBox.Text, out var pakAddress);
            int.TryParse(this.securityCodeTextBox.Text, out var securityCode);
            int.TryParse(this.tableNumTextBox.Text, out var tableNum);
            int.TryParse(this.portTextBox.Text, out var tcpPort);
            var dataInterval = this.dataIntervalComboBox.Text;
            var loggerType = this.loggerTypeComboBox.Text;

            if (!this.IsValid(ipAddress, loggerName, pakAddress, tcpPort, securityCode, tableNum))
            {
                return;
            }

            IPAddress.TryParse(ipAddress, out var address);
            var result = MessageBox.Show(
                @"Logger Name: " + loggerName + "\n" + @"IP Address: " + address + "\n" + @"Logger Type: " + loggerType
                + "\n" + @"PakBus Address: " + pakAddress + "\n" + @"Port: " + tcpPort + "\n" + @"Table Number: "
                + tableNum + "\n" + @"Security Code: " + securityCode + "\n" + @"Data Interval: " + dataInterval,
                @"Add Logger",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.AddLogger(
                    address.ToString(),
                    loggerName,
                    loggerType,
                    pakAddress,
                    tcpPort,
                    securityCode,
                    tableNum,
                    this.dataIntervalComboBox.SelectedIndex);
            }
        }

        /// <summary>
        /// The add logger.
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="pakAdd">
        /// The pb add.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="secCode">
        /// The sec code.
        /// </param>
        /// <param name="tableNum">
        /// The table num.
        /// </param>
        /// <param name="dataInterval">
        /// The data interval.
        /// </param>
        private void AddLogger(
            string ip,
            string name,
            string type,
            int pakAdd,
            int port,
            int secCode,
            int tableNum,
            int dataInterval)
        {
            var logger = new Logger
                             {
                                 IpAddress = ip,
                                 LoggerName = name,
                                 LoggerType = type,
                                 PbAddress = pakAdd,
                                 Port = port,
                                 SecurityCode = secCode,
                                 TableNum = tableNum,
                                 DataInterval = dataInterval
                             };

            if (!SimplePb.IsFileLocked(filePath))
            {
                this.list.Add(logger);
                this.loggerListBox.Items.Add(logger.LoggerName);
                var json = JsonConvert.SerializeObject(this.list, Formatting.Indented);
                using (var isoStream = new IsolatedStorageFileStream(filePath, FileMode.Create, this.isoStore))
                {
                    using (var writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(json);
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Error: Configuration file can not be accessed");
            }
        }

        /// <summary>
        /// The delete logger.
        /// </summary>
        private void DeleteLogger()
        {
            if (!SimplePb.IsFileLocked(filePath))
            {
                this.list.RemoveAt(this.loggerListBox.SelectedIndex);
                this.loggerListBox.Items.Remove(this.loggerListBox.SelectedItem);
                var json = JsonConvert.SerializeObject(this.list, Formatting.Indented);
                using (var isoStream = new IsolatedStorageFileStream(filePath, FileMode.Create, this.isoStore))
                {
                    using (var writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(json);
                        writer.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Error: Configuration file can not be accessed");
            }
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="pakAdd">
        /// The pb add.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="secCode">
        /// The sec code.
        /// </param>
        /// <param name="tableNum">
        /// The table num.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsValid(string ip, string name, int pakAdd, int port, int secCode, int tableNum)
        {
            if (!IsValidIp(ip))
            {
                MessageBox.Show(@"Not a valid IP Address");
                return false;
            }

            if (!this.IsValidName(name))
            {
                MessageBox.Show(@"Not a valid Name");
                return false;
            }

            if (!IsValidPbAdd(pakAdd))
            {
                MessageBox.Show(@"PakBus Address must be between 1 and 3999");
                return false;
            }

            if (!IsValidPort(port))
            {
                MessageBox.Show(@"Not a valid port number");
                return false;
            }

            if (!IsValidSecCode(secCode))
            {
                MessageBox.Show(@"security code must be between 1 and 65536 (0 if no security code set)");
                return false;
            }

            if (IsValidTableNum(tableNum))
            {
                return true;
            }

            MessageBox.Show(@"Table Number must be between 1 and 100");
            return false;
        }

        /// <summary>
        /// The list box 1_ double click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ListBox1DoubleClick(object sender, EventArgs e)
        {
            if (this.loggerListBox.SelectedItem == null)
            {
                return;
            }

            var dL = this.list[this.loggerListBox.SelectedIndex];
            var dataInterval = dL.DataInterval == 0 ? "1 hour intervals" : "1 minute intervals";
            var successfulPoll = dL.LastSuccessfulPoll == new DateTime()
                                     ? "Never Polled"
                                     : dL.LastSuccessfulPoll + string.Empty;

            MessageBox.Show(
                @"Logger Name: " + dL.LoggerName + "\n" + @"IP Address: " + dL.IpAddress + "\n" + @"Logger Type: "
                + dL.LoggerType + "\n" + @"PakBus Address: " + dL.PbAddress + "\n" + @"Port: " + dL.Port + "\n"
                + @"Table Number: " + dL.TableNum + "\n" + @"Security Code: " + dL.SecurityCode + "\n"
                + @"Data Interval: " + dataInterval + "\n" + @"Last successful Poll: " + successfulPoll,
                dL.LoggerName + @" Info",
                MessageBoxButtons.OKCancel);
        }

        /// <summary>
        /// The is valid name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsValidName(string name)
        {
            var loggerName = name;
            var isValid = !string.IsNullOrEmpty(loggerName)
                          && loggerName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
            return this.list.All(p => p.LoggerName != loggerName) && isValid;
        }

        /// <summary>
        /// The delete button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show(@"Do you want to delete logger?", @"Delete Logger", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.DeleteLogger();
            }
        }

        /// <summary>
        /// The poll_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void PollClick(object sender, EventArgs e)
        {
            string[] args = { string.Empty, string.Empty, string.Empty };

            // MessageBox.Show(Settings.Default.path);
            args[1] = "0";
            if (this.loggerListBox.SelectedIndex > 0)
            {
                args[2] = this.loggerListBox.SelectedIndex.ToString();
            }

            if (this.loggerListBox.SelectedIndex >= 0)
            {
                this.pollButton.Text = @"Polling " + this.list[this.loggerListBox.SelectedIndex].LoggerName + @"...";
                this.progressBar1.Visible = true;
                this.pollButton.Enabled = false;
                this.PollAllLoggerButton.Enabled = false;

                await Task.Run(
                    () =>
                        {
                            var poll = new PollingClass(args);
                            try
                            {
                                poll.StartPoll();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        });

                this.loggerListBox.Items.Clear();
                this.LoadList();
                this.progressBar1.Visible = false;
                this.pollButton.Enabled = true;
                this.PollAllLoggerButton.Enabled = true;
                this.pollButton.Text = @"Poll Logger";
            }
            else
            {
                MessageBox.Show(@"No Logger Selected");
            }
        }

        /// <summary>
        /// The csv save path tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CsvSavePathToolStripMenuItemClick(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            var result = folderBrowserDialog1.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            Settings.Default.path = folderBrowserDialog1.SelectedPath;
            Settings.Default.Save();

            // MessageBox.Show(Settings.Default.path);
        }

        /// <summary>
        /// The poll all loggers button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void PollAllLoggersButtonClick(object sender, EventArgs e)
        {
            string[] args = { string.Empty, string.Empty };

            args[1] = "0";
            this.PollAllLoggerButton.Text = @"Polling all loggers...";

            this.progressBar1.Visible = true;
            this.PollAllLoggerButton.Enabled = false;
            this.pollButton.Enabled = false;

            await Task.Run(
                () =>
                    {
                        var poll = new PollingClass(args);
                        try
                        {
                            poll.StartPoll();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
            this.loggerListBox.Items.Clear();
            this.LoadList();
            this.progressBar1.Visible = false;
            this.PollAllLoggerButton.Enabled = true;
            this.pollButton.Enabled = true;

            this.PollAllLoggerButton.Text = @"Poll All Loggers";
        }
    }
}