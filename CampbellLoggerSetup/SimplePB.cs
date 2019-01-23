// --------------------------------------------------------------------------------------------------------------------
// <copyright file="" company="">
//   
// </copyright>
// <summary>
//   // This class "wraps" the SimplePB.dll. It encapsulates the underlying dll function 
// calls with overloaded versions of those functions for compatibility with the .Net Framework.
// 
// The SimplePB class initiates calls to the unmanaged SimplePB.dll functions as Platform Invokes and
// utilizes the System.Runtime.InteropServices.DllImport attribute for this purpose.
// 
// Many of the unmanaged functions take a pointer to an array of characters, either a char* or a char**, as an argument.
// To accommodate this, the wrapper class will convert a .Net String value to a Byte array on the unmanaged heap and
// pass a System.IntPtr to the array as an argument. In the case of a char* parameter, the IntPtr is passed by value; for a 
// char**, the IntPtr is passed by reference (ref).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CampbellLoggerSetup
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The simple pb.
    /// </summary>
    public static class SimplePb
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Import the SimplePB.Dll and declare the CloseIPPort function signature.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("SimplePB.dll", EntryPoint = "CloseIPPort", CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseIPPort();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the SetSecurity function signature
        [DllImport("SimplePB.dll", EntryPoint = "SetSecurity", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetSecurity(int securityCode);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the FileControl function signature
        [DllImport("SimplePB.dll", EntryPoint = "FileControl", CallingConvention = CallingConvention.StdCall)]
        private static extern FileControlReturn FileControl(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            IntPtr fileName,
            FileControlCommandType command);

        // Wrapper for the FileControl function
        /// <summary>
        /// The file control.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The <see cref="FileControlReturn"/>.
        /// </returns>
        public static FileControlReturn FileControl(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            string fileName,
            FileControlCommandType command)
        {
            // Get pointer to the 'char' parameter created on the unmanaged heap
            var pfileName = GetBytesForString(fileName);

            // Call the SimplePB.Dll function
            var rtn = FileControl(pakBusAddress, deviceType, pfileName, command);

            // Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(pfileName);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the FileSend function signature
        [DllImport("SimplePB.dll", EntryPoint = "File_Send", CallingConvention = CallingConvention.StdCall)]
        private static extern FileSendReturn File_Send(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            IntPtr fileName,
            out IntPtr returnData,
            out int returnLen);

        // Wrapper for the FileSend function
        /// <summary>
        /// The file send.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="FileSendReturn"/>.
        /// </returns>
        public static FileSendReturn FileSend(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            string fileName,
            ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Get pointer to the 'char' parameter created on the unmanaged heap
            var pfileName = GetBytesForString(fileName);

            // Call the SimplePB.Dll function
            var rtn = File_Send(pakBusAddress, deviceType, pfileName, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(pfileName);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetAddress function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetAddress", CallingConvention = CallingConvention.StdCall)]
        private static extern GetAddressReturn GetAddress(
            DeviceTypeCodes deviceType,
            out IntPtr returnData,
            out int returnLen);

        // Wrapper for the GetAddress function
        /// <summary>
        /// The get address.
        /// </summary>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetAddressReturn"/>.
        /// </returns>
        public static GetAddressReturn GetAddress(DeviceTypeCodes deviceType, ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetAddress(deviceType, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetClock function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetClock", CallingConvention = CallingConvention.StdCall)]
        private static extern ClockReturn GetClock(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            out IntPtr returnData,
            out int returnDataLen);

        // Wrapper for the GetClock function
        /// <summary>
        /// The get clock.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="ClockReturn"/>.
        /// </returns>
        public static ClockReturn GetClock(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetClock(pakBusAddress, deviceType, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetCommaData function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetCommaData", CallingConvention = CallingConvention.StdCall)]
        private static extern GetDataReturn GetCommaData(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            int tableNo,
            int recordNo,
            out IntPtr returnData,
            out int returnLen);

        // Wrapper for the GetCommaData function
        /// <summary>
        /// The get comma data.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="tableNumber">
        /// The table number.
        /// </param>
        /// <param name="recordNumber">
        /// The record number.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetDataReturn"/>.
        /// </returns>
        public static GetDataReturn GetCommaData(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            int tableNumber,
            int recordNumber,
            ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetCommaData(
                pakBusAddress,
                deviceType,
                tableNumber,
                recordNumber,
                out var returnPtr,
                out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetData function signature
        /// <summary>
        /// The get data.
        /// </summary>
        /// <param name="pakbusAddress">
        /// The pakbus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="tableNo">
        /// The table no.
        /// </param>
        /// <param name="recordNo">
        /// The record no.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <param name="returnDataLen">
        /// The return data len.
        /// </param>
        /// <returns>
        /// The <see cref="GetDataReturn"/>.
        /// </returns>
        [DllImport("SimplePB.dll", EntryPoint = "GetData", CallingConvention = CallingConvention.StdCall)]

        // ReSharper disable once MemberCanBePrivate.Global
        public static extern GetDataReturn GetData(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            int tableNo,
            int recordNo,
            out IntPtr returnData,
            out int returnDataLen);

        // Wrapper for the GetData function
        /// <summary>
        /// The get data.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="tableNumber">
        /// The table number.
        /// </param>
        /// <param name="recordNumber">
        /// The record number.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetDataReturn"/>.
        /// </returns>
        public static GetDataReturn GetData(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            int tableNumber,
            int recordNumber,
            ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetData(
                pakBusAddress,
                deviceType,
                tableNumber,
                recordNumber,
                out var returnPtr,
                out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetDataHeader function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetDataHeader", CallingConvention = CallingConvention.StdCall)]
        private static extern GetDataReturn GetDataHeader(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            int tableNo,
            out IntPtr returnData,
            out int returnLen);

        // Wrapper for the GetDataHeader function
        /// <summary>
        /// The get data header.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="tableNumber">
        /// The table number.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetDataReturn"/>.
        /// </returns>
        public static GetDataReturn GetDataHeader(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            int tableNumber,
            ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetDataHeader(pakBusAddress, deviceType, tableNumber, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetDLLVersion function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetDLLVersion", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetDLLVersion(out IntPtr returnData, out int returnDataLen);

        // Wrapper for the GetDLLVersion function
        /// <summary>
        /// The get dll version.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDllVersion()
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            GetDLLVersion(out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            return GetStringFromIntPtr(returnPtr, returnPtrLength);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetStatus function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetStatus", CallingConvention = CallingConvention.StdCall)]
        private static extern GetStatusReturn GetStatus(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            out IntPtr returnData,
            out int returnDataLen);

        // Wrapper for the GetStatus function
        /// <summary>
        /// The get status.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetStatusReturn"/>.
        /// </returns>
        public static GetStatusReturn GetStatus(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetStatus(pakBusAddress, deviceType, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetTableNames function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetTableNames", CallingConvention = CallingConvention.StdCall)]
        private static extern GetTablesReturn GetTableNames(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            out IntPtr returnData,
            out int returnDataLen);

        // Wrapper for the GetTableNames function
        /// <summary>
        /// The get table names.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetTablesReturn"/>.
        /// </returns>
        public static GetTablesReturn GetTableNames(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = GetTableNames(pakBusAddress, deviceType, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the GetValue function signature
        [DllImport("SimplePB.dll", EntryPoint = "GetValue", CallingConvention = CallingConvention.StdCall)]
        private static extern GetValueReturn GetValue(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            int swath,
            IntPtr tableName,
            IntPtr fieldName,
            out IntPtr returnData,
            out int returnDataLen);

        /// <summary>
        /// Wrapper for the GetValue function
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="swath">
        /// The swath.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="GetValueReturn"/>.
        /// </returns>
        public static GetValueReturn GetValue(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            int swath,
            string tableName,
            string fieldName,
            ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Get pointers to the 'char' parameters created on the unmanaged heap
            var ptableName = GetBytesForString(tableName);
            var pfieldName = GetBytesForString(fieldName);

            // Call the SimplePB.Dll function
            var rtn = GetValue(
                pakBusAddress,
                deviceType,
                swath,
                ptableName,
                pfieldName,
                out var returnPtr,
                out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(ptableName);
            Marshal.FreeHGlobal(pfieldName);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the OpenIPPort function signature
        [DllImport("SimplePB.dll", EntryPoint = "OpenIPPort", CallingConvention = CallingConvention.StdCall)]
        private static extern int OpenIPPort(IntPtr ipAddress, int tcpPort);

        // Wrapper for the OpenIPPort function
        /// <summary>
        /// The open ip port.
        /// </summary>
        /// <param name="iPAddress">
        /// The i p address.
        /// </param>
        /// <param name="tcpPort">
        /// The tcp port.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int OpenIpPort(string iPAddress, int tcpPort)
        {
            // Get pointer to the 'char' parameter created on the unmanaged heap
            var ipAddress = GetBytesForString(iPAddress);

            // Call the SimplePB.Dll function
            var rtn = OpenIPPort(ipAddress, tcpPort);

            // Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(ipAddress);

            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the SetClock function signature
        [DllImport("SimplePB.dll", EntryPoint = "SetClock", CallingConvention = CallingConvention.StdCall)]
        private static extern ClockReturn SetClock(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            out IntPtr returnData,
            out int returnDataLen);

        // Wrapper for the SetClock function
        /// <summary>
        /// The set clock.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="returnData">
        /// The return data.
        /// </param>
        /// <returns>
        /// The <see cref="ClockReturn"/>.
        /// </returns>
        public static ClockReturn SetClock(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            // Declare variables for 'returned' parameters

            // Call the SimplePB.Dll function
            var rtn = SetClock(pakBusAddress, deviceType, out var returnPtr, out var returnPtrLength);

            // Use the 'returned' parameters to retrieve the string data
            returnData = GetStringFromIntPtr(returnPtr, returnPtrLength);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Import the SimplePB.Dll and declare the SetValue function signature
        [DllImport("SimplePB.dll", EntryPoint = "SetValue", CallingConvention = CallingConvention.StdCall)]
        private static extern SetValueReturn SetValue(
            int pakbusAddress,
            DeviceTypeCodes deviceType,
            IntPtr tableName,
            IntPtr fieldName,
            IntPtr value);

        // Wrapper for the SetValue function
        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="pakBusAddress">
        /// The pak bus address.
        /// </param>
        /// <param name="deviceType">
        /// The device type.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="SetValueReturn"/>.
        /// </returns>
        public static SetValueReturn SetValue(
            int pakBusAddress,
            DeviceTypeCodes deviceType,
            string tableName,
            string fieldName,
            string value)
        {
            // Get pointers to the 'char' parameters created on the unmanaged heap
            var ptableName = GetBytesForString(tableName);
            var pfieldName = GetBytesForString(fieldName);
            var pvalue = GetBytesForString(value);

            // Call the SimplePB.Dll function
            var rtn = SetValue(pakBusAddress, deviceType, ptableName, pfieldName, pvalue);

            // Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(ptableName);
            Marshal.FreeHGlobal(pfieldName);
            Marshal.FreeHGlobal(pvalue);

            // Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Takes a pointer to an array of bytes or (char*) on the unmanaged heap,
        // converts the array to a string value on the managed heap and returns the String.
        // Assumes a byte array of UTF8 chars.
        /// <summary>
        /// The get string from int ptr.
        /// </summary>
        /// <param name="ptrToString">
        /// The ptr to string.
        /// </param>
        /// <param name="dataLength">
        /// The data length.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetStringFromIntPtr(IntPtr ptrToString, int dataLength)
        {
            if (ptrToString == IntPtr.Zero)
            {
                return string.Empty;
            }

            var myData = new byte[dataLength];
            Marshal.Copy(ptrToString, myData, 0, dataLength);
            return Encoding.UTF8.GetString(myData);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Converts the String value to an Array of UTF8 encoded Bytes on the unmanaged heap and
        // returns a System.IntPtr to the Array.
        /// <summary>
        /// The get bytes for string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        private static IntPtr GetBytesForString(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            IntPtr unmanagedMemory;
            try
            {
                unmanagedMemory = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, unmanagedMemory, bytes.Length);

                // ' write null terminating character
                Marshal.WriteByte(unmanagedMemory, bytes.Length, 0);
            }
            catch
            {
                unmanagedMemory = IntPtr.Zero;
            }

            return unmanagedMemory;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The is file locked.
        /// </summary>
        /// <param name="filePath">
        /// The file Path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (var stream = IsolatedStorageFile
                    .GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null)
                    .OpenFile(filePath, FileMode.Open))
                {
                    // File is not locked
                    return !stream.CanRead;
                }
            }
            catch (IOException)
            {
                // the file is unavailable because it is:
                // still being written to
                // or being processed by another thread
                // or does not exist (has already been processed)
                Console.WriteLine(@"exception occured");
                return true;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// The clock return.
    /// </summary>
    public enum ClockReturn
    {
        // ''clock_success -> 0
        /// <summary>
        /// The clock success.
        /// </summary>
        ClockSuccess = 0,

        // ''clock_comm_failure -> -1
        /// <summary>
        /// The clock comm failure.
        /// </summary>
        ClockCommFailure = -1,

        // ''clock_port_not_opened -> -2
        /// <summary>
        /// The clock port not opened.
        /// </summary>
        ClockPortNotOpened = -2
    }

    public enum DeviceTypeCodes
    {
        // ''device_cr200 -> 1
        CR200 = 1,

        // ''device_cr10xpb -> 2
        CR10XPB = 2,

        // ''device_cr1000 -> 3
        CR1000 = 3,

        // ''device_cr3000 -> 4
        CR3000 = 4,

        // ''device_cr800 -> 5
        CR800 = 5,

        // ''device_cr6 -> 9
        CR6 = 9,

        // ''device_cr300 -> 13
        CR300 = 13,

        // ''device_cr1000x -> 14
        CR1000X = 14
    }

    public enum FileControlCommandType
    {
        // ''command_compile_and_run -> 1
        command_compile_and_run = 1,

        // ''command_set_run_on_power_up -> 2
        command_set_run_on_power_up = 2,

        // ''command_make_hidden -> 3
        command_make_hidden = 3,

        // ''command_delete_file -> 4
        command_delete_file = 4,

        // ''command_format_device -> 5
        command_format_device = 5,

        // ''command_compile_and_run_leave_tables -> 6
        command_compile_and_run_leave_tables = 6,

        // ''command_stop_program -> 7
        command_stop_program = 7,

        // ''command_stop_program_and_delete -> 8
        command_stop_program_and_delete = 8,

        // ''command_make_os -> 9
        command_make_os = 9,

        // ''command_compile_and_run_no_power_up -> 10
        command_compile_and_run_no_power_up = 10,

        // ''command_pause -> 11
        command_pause = 11,

        // ''command_resume -> 12
        command_resume = 12,

        // ''command_stop_delete_and_run -> 13
        command_stop_delete_and_run = 13,

        // ''command_stop_delete_and_run_no_power -> 14
        command_stop_delete_and_run_no_power = 14
    }

    public enum FileControlReturn
    {
        // ''filecontrol_success -> 0
        filecontrol_success = 0,

        // ''filecontrol_comm_failure -> -1
        filecontrol_comm_failure = -1,

        // ''filecontrol_not_open -> -2
        filecontrol_not_open = -2
    }

    public enum GetAddressReturn
    {
        // ''getaddr_success -> 0
        getaddr_success = 0,

        // ''getaddr_comm_failure -> -1
        getaddr_comm_failure = -1,

        // ''getaddr_not_open -> -2
        getaddr_not_open = -2
    }

    public enum GetDataReturn
    {
        // ''getdata_more -> 1
        getdata_more = 1,

        // ''getdata_complete -> 0
        getdata_complete = 0,

        // ''getdata_comm_failure -> -1
        getdata_comm_failure = -1,

        // ''getdata_not_open -> -2
        getdata_not_open = -2,

        // ''getdata_invalid_table_no -> -3
        getdata_invalid_table_no = -3
    }

    public enum GetValueReturn
    {
        // ''getval_success -> 0
        getval_success = 0,

        // ''getval_comm_failure -> -1
        getval_comm_failure = -1,

        // ''getval_port_not_opened -> -2
        getval_port_not_opened = -2
    }

    public enum GetTablesReturn
    {
        // ''gettables_success -> 0
        gettables_success = 0,

        // ''gettables_comm_failure -> 1
        gettables_comm_failure = 1,

        // ''gettables_not_open -> -2
        gettables_not_open = -2
    }

    public enum GetStatusReturn
    {
        // ''getstatus_success -> 0
        getstatus_success = 0,

        // ''getstatus_comm_failure -> -1
        getstatus_comm_failure = -1,

        // ''getstatus_not_open -> -2
        getstatus_not_open = -2
    }

    public enum FileSendReturn
    {
        // ''filesend_more -> 1
        filesend_more = 1,

        // ''filesend_complete -> 0
        filesend_complete = 0,

        // ''filesend_comm_failure -> -1
        filesend_comm_failure = -1,

        // ''filesend_not_opened -> -2
        filesend_not_opened = -2,

        // ''filesend_cant_open_source -> -3
        filesend_cant_open_source = -3,

        // ''filesend_file_name_invalid -> -4
        filesend_file_name_invalid = -4,

        // ''filesend_logger_timeout -> -5
        filesend_logger_timeout = -5,

        // ''filesend_invalid_file_offset -> -6
        filesend_invalid_file_offset = -6,

        // ''filesend_datalogger_error -> -7
        filesend_datalogger_error = -7,

        // ''filesend_filecontrol_error -> -8
        filesend_filecontrol_error = -8,

        // ''filesend_cant_get_prog_status -> -9
        filesend_cant_get_prog_status = -9
    }

    public enum SetValueReturn
    {
        // ''setval_success -> 0
        setval_success = 0,

        // ''setval_comm_failure -> -1
        setval_comm_failure = -1,

        // ''setval_not_opened -> -2
        setval_not_opened = -2
    }
}