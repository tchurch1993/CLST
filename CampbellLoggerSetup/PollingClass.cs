// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Duke Energy" file="PollingClass.cs">
// Author: Kristopher Tyler Church
// </copyright>
// <summary>
// Description: Polling class that takes in arguments and uses those to specify which logger to poll.This class handles all the polling logic with assistance from SimplePB.cs
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace CampbellLoggerSetup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Windows.Forms;

    using CampbellLoggerSetup.Properties;

    using Newtonsoft.Json;

    /// <summary>
    /// The polling class.
    /// </summary>
    internal class PollingClass
    {
        /// <summary>
        /// config.json filename
        /// </summary>
        private const string Filename = "CampbellToAirvision\\config.json";

        /// <summary>
        /// The system path.
        /// </summary>
        private static readonly string SystemPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// The iso store.
        /// </summary>
        private IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(
            IsolatedStorageScope.User | IsolatedStorageScope.Assembly,
            null,
            null);

        /// <summary>
        /// The complete path.
        /// </summary>
        private readonly string completePath = Path.Combine(SystemPath, Filename);

        /// <summary>
        /// destination for CSV file
        /// </summary>
        private static string destination = Settings.Default.path;

        /// <summary>
        /// Arguments passed to polling class
        /// </summary>
        private readonly string[] args;

        /// <summary>
        /// The list of loggers
        /// </summary>
        private List<Logger> list = new List<Logger>();

        /// <summary>
        /// The logger connected boolean
        /// </summary>
        private bool loggerConnected;

        /// <summary>
        /// constructor that passes the arguments
        /// Initializes a new instance of the <see cref="PollingClass"/> class.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        public PollingClass(string[] args)
        {
            this.args = args;
        }

        /// <summary>
        /// The start poll.
        /// </summary>
        /// <exception cref="Exception">
        /// handle's any unexpected error that could occur during poling
        /// </exception>
        public void StartPoll()
        {
            if (destination.Equals(string.Empty))
            {
                destination = SystemPath;
            }

            // hourly average takes 24 records
            // minute values takes 1440 records

            // parses arguments into integers
            var daily = false;
            var index = -1;
            if (this.args.Length != 0)
            {
                if (this.args[1].Equals("1"))
                {
                    daily = true;
                    Console.WriteLine(@"DAILY SET TO TRUE");
                }

                try
                {
                    int.TryParse(this.args[2], out index);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine(@"Loading configuration...");

            // loads JSON List of loggers into memory
            this.LoadJson();
            Console.WriteLine(@"Polling....");

            // nested if to handle arguments
            if (!daily)
            {
                if (index == -1)
                {
                    try
                    {
                        this.PollAll();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw ex;
                    }
                }
                else
                {
                    if (index < this.list.Count)
                    {
                        try
                        {
                            this.PollLoggerAll(this.list[index]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            throw ex;
                        }
                    }
                    else
                    {
                        Console.WriteLine(@"index too large");
                    }
                }
            }
            else
            {
                if (index == -1)
                {
                    try
                    {
                        this.PollAllDaily();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            this.PollAll();
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            throw exception;
                        }

                        Console.WriteLine(ex);
                    }
                }
                else
                {
                    if (index < this.list.Count)
                    {
                        try
                        {
                            this.PollLoggerDaily(this.list[index]);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.PollLoggerAll(this.list[index]);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                throw exception;
                            }

                            Console.WriteLine(ex);
                        }
                    }
                    else
                    {
                        Console.WriteLine(@"index too large");
                    }
                }
            }
        }

        /// <summary>
        /// returns number of records a logger has in the given table
        /// (used for daily polls to not have to get all data from logger every day)
        /// </summary>
        /// <param name="pakBusId">
        /// The pak bus id.
        /// </param>
        /// <param name="tableNum">
        /// The table num.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// if communication cannot be established throw a communication error exception
        /// </exception>
        private static int GetTableRecNum(int pakBusId, int tableNum, DeviceTypeCodes deviceType)
        {
            var record = string.Empty;
            var recordNum = 0;
            var returnval = SimplePb.GetCommaData(pakBusId, deviceType, tableNum, -1, ref record);

            // retries if failed first attempt
            if (returnval != 0)
            {
                returnval = SimplePb.GetCommaData(pakBusId, deviceType, tableNum, -1, ref record);
            }

            if (returnval != 0)
            {
                throw new Exception("Communication error");
            }

            var recordList = record.Split(',').ToList();
            if (recordList.Count > 2)
            {
                int.TryParse(recordList[1], out recordNum);
            }

            // Display Results
            if (returnval == 0)
            {
                return recordNum;
            }

            return -1;
        }

        /// <summary>
        /// takes logger type from string form to DeviceTypeCodes eNum to satisfy wrapper method.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="DeviceTypeCodes"/>.
        /// </returns>
        private static DeviceTypeCodes LoggerTypeToCode(string type)
        {
            switch (type)
            {
                case "CR200":
                    return DeviceTypeCodes.CR200;
                case "CR10XPB":
                    return DeviceTypeCodes.CR10XPB;
                case "CR1000":
                    return DeviceTypeCodes.CR1000;
                case "CR3000":
                    return DeviceTypeCodes.CR3000;
                case "CR800":
                    return DeviceTypeCodes.CR800;
                case "CR6":
                    return DeviceTypeCodes.CR6;
                case "CR300":
                    return DeviceTypeCodes.CR300;
                case "CR1000X":
                    return DeviceTypeCodes.CR1000X;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// adds current time to Logger object when polled successfully.
        /// </summary>
        /// <param name="loggerName">
        /// The logger name.
        /// </param>
        private void LastSuccessfulPoll(string loggerName)
        {
            foreach (var logger in this.list)
            {
                if (!logger.LoggerName.Equals(loggerName))
                {
                    continue;
                }

                Console.WriteLine(logger.LoggerName + @" : " + DateTime.Now);
                Console.WriteLine(@"Successfully connected to " + logger.LoggerName);
                logger.LastSuccessfulPoll = DateTime.Now;
            }
        }

        /// <summary>
        /// saves updated list object back into config.json
        /// </summary>
        private void SaveJson()
        {
            // var fileInfo = new FileInfo(this.completePath);
            // if (!SimplePb.IsFileLocked(Filename))
            // {
            // var json = JsonConvert.SerializeObject(this.list, Formatting.Indented);
            // File.WriteAllText(this.completePath, json);
            // }
            // else
            // {
            // Console.WriteLine(@"Error: Could not save poll time");
            // }
            if (!SimplePb.IsFileLocked(Filename))
            {
                var json = JsonConvert.SerializeObject(this.list, Formatting.Indented);
                using (var isoStream = new IsolatedStorageFileStream(Filename, FileMode.Create, this.isoStore))
                {
                    using (var writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(json);
                    }
                }
            }
            else
            {
                Console.WriteLine(@"Error: Could not save poll time");
            }
        }

        /// <summary>
        /// loads config.json into list object.
        /// </summary>
        private void LoadJson()
        {
            // loads JSON List of loggers into memory
            // var json = File.ReadAllText(this.completePath);
            // this.list = JsonConvert.DeserializeObject<List<Logger>>(json);
            if (!this.isoStore.FileExists(Filename))
            {
                return;
            }

            using (var openStream = this.isoStore.OpenFile(Filename, FileMode.Open))
            {
                using (var reader = new StreamReader(openStream))
                {
                    var fileContent = reader.ReadToEnd();
                    if (fileContent.Equals(string.Empty))
                    {
                        Console.WriteLine(@"config file empty...");
                        return;
                    }

                    var json = fileContent;
                    this.list = JsonConvert.DeserializeObject<List<Logger>>(json);
                }
            }
        }

        /// <summary>
        /// poll all data from all loggers.
        /// </summary>
        private void PollAll()
        {
            foreach (var logger in this.list)
            {
                this.PollRecords(logger, 0);
            }

            this.SaveJson();
        }

        /// <summary>
        /// poll all data from a single logger.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <exception cref="Exception">
        /// If CSV file is missing a communication error exception will be thrown
        /// </exception>
        private void PollLoggerAll(Logger logger)
        {
            this.PollRecords(logger, 0);
            if (!File.Exists(destination + "/" + logger.LoggerName + ".csv"))
            {
                throw new Exception("Communication Error");
            }

            this.SaveJson();
        }

        /// <summary>
        /// polls single logger for last 24 hours of data.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <exception cref="Exception">
        /// throws communication error if CSV file was not created
        /// </exception>
        private void PollLoggerDaily(Logger logger)
        {
            var returnVal = SimplePb.OpenIpPort(logger.IpAddress, logger.Port);
            var recNum = -1;

            if (returnVal == 0)
            {
                // <summary>
                // returns most recent record number
                // </summary>
                recNum = GetTableRecNum(logger.PbAddress, logger.TableNum, LoggerTypeToCode(logger.LoggerType));
                this.loggerConnected = true;
            }

            if (recNum != -1)
            {
                // sets record number value based on Logger.DataInterval type (Minute data or Hourly data)
                switch (logger.DataInterval)
                {
                    case 0:
                        recNum = recNum - 24;
                        break;
                    case 1:
                        recNum = recNum - 1440;
                        break;
                    default:
                        recNum = 0;
                        break;
                }
            }

            this.PollRecords(logger, recNum);

            // <summary>
            // If PollRecords saves a CSV it means it has successfully retrieved some data
            // This function checks if CSV file was created which is to test if communication
            // was established
            // </summary>
            if (!File.Exists(destination + "\\" + logger.LoggerName + ".csv"))
            {
                throw new Exception("Communication Error");
            }

            // <summary>
            // after polling is done the Logger Objects are saved back into json format
            // this is to save the LastSuccessfulPoll value in a config file
            // </summary>
            this.SaveJson();
        }

        /// <summary>
        /// polls all loggers to get last 24 hours of data.
        /// </summary>
        private void PollAllDaily()
        {
            foreach (var logger in this.list)
            {
                var returnVal = SimplePb.OpenIpPort(logger.IpAddress, logger.Port);
                var recNum = -1;
                if (returnVal == 0)
                {
                    Console.WriteLine(@"before getTableRecNum");
                    recNum = GetTableRecNum(logger.PbAddress, logger.TableNum, LoggerTypeToCode(logger.LoggerType));
                    Console.WriteLine(@"after getTableRecNum");
                    this.loggerConnected = true;
                }

                if (recNum != -1)
                {
                    switch (logger.DataInterval)
                    {
                        case 0:
                            recNum = recNum - 24;
                            break;
                        case 1:
                            recNum = recNum - 1440;
                            break;
                        default:
                            recNum = 0;
                            break;
                    }

                    Console.WriteLine(recNum);
                }
                else
                {
                    Console.Write(@"Communication Error for " + logger.LoggerName);
                }

                this.PollRecords(logger, recNum);
                this.SaveJson();
            }
        }

        /// <summary>
        /// gets logger data from specified record number up to current record.
        /// </summary>
        /// <param name="logger">
        /// The logger object.
        /// </param>
        /// <param name="record">
        /// The record number to start with.
        /// </param>
        private void PollRecords(Logger logger, int record)
        {
            int returnVal;
            var strData = string.Empty;
            if (this.loggerConnected)
            {
                returnVal = 0;
            }
            else
            {
                returnVal = SimplePb.OpenIpPort(logger.IpAddress, logger.Port);

                this.loggerConnected = true;
            }

            if (returnVal == 0)
            {
                var deviceType = LoggerTypeToCode(logger.LoggerType);
                var dataReturn = SimplePb.GetCommaData(
                    logger.PbAddress,
                    deviceType,
                    logger.TableNum,
                    record,
                    ref strData);
                Console.WriteLine(@"dataReturn value: " + dataReturn);
                if (dataReturn >= 0)
                {
                    this.LastSuccessfulPoll(logger.LoggerName);
                }

                if (dataReturn != 0)
                {
                    dataReturn = SimplePb.GetCommaData(
                        logger.PbAddress,
                        deviceType,
                        logger.TableNum,
                        record,
                        ref strData);
                }

                if (dataReturn == 0)
                {
                    File.WriteAllText(destination + "\\" + logger.LoggerName + ".csv", strData);
                }
                else if (dataReturn > 0)
                {
                    File.WriteAllText(destination + "\\" + logger.LoggerName + ".csv", strData);

                    do
                    {
                        dataReturn = SimplePb.GetCommaData(
                            logger.PbAddress,
                            deviceType,
                            logger.TableNum,
                            record,
                            ref strData);

                        if (dataReturn < 0)
                        {
                            break;
                        }

                        File.AppendAllText(destination + "\\" + logger.LoggerName + ".csv", strData);
                    }
                    while (dataReturn > 0);
                }
            }

            Console.WriteLine(@"closing connection to " + logger.LoggerName);
            returnVal = SimplePb.CloseIPPort();
            this.loggerConnected = false;
            Console.WriteLine(returnVal == 0 ? "Close Port Successful" : "Close Port Unsuccessful");
        }

        /// <summary>
        /// gets current reading from specified logger.
        /// </summary>
        /// <TODO>
        /// Use Mod-bus to get realtime data
        /// </TODO>
        private void CurrentReading()
        {
        }
    }
}