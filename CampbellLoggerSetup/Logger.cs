// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Duke Energy">
//   Author: Kristopher Tyler Church
// </copyright>
// <summary>
//   Defines the Logger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace CampbellLoggerSetup
{
    using System;

    /// <summary>
    /// The logger.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the logger name.
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// Gets or sets the logger type.
        /// </summary>
        public string LoggerType { get; set; }

        /// <summary>
        /// Gets or sets the pb address.
        /// </summary>
        public int PbAddress { get; set; }

        /// <summary>
        /// Gets or sets the security code.
        /// </summary>
        public int SecurityCode { get; set; }

        /// <summary>
        /// Gets or sets the table num.
        /// </summary>
        public int TableNum { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the data interval.
        /// </summary>
        public int DataInterval { get; set; }

        /// <summary>
        /// Gets or sets the last successful poll.
        /// </summary>
        public DateTime LastSuccessfulPoll { get; set; }
    }
}