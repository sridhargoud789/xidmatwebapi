using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDVO
{
    public class CommonDvo
    {
    }
    /// <summary>
    ///     Object for the retun json generation
    /// </summary>
    public class PassingJsonObj
    {
        //Bool error
        public bool Iserror { get; set; }
        //Error message ::: default Empty
        public string ErrorMsg { get; set; }
        //Data to send goes here
        public DataTable Data { get; set; }
    }
    public class DeviceDetails {
        public string DeviceName { get; set; }
        public string DeviceOSType { get; set; }
        public string DeviceOSVersion { get; set; }
        public string AppVersion { get; set; }        
    }
}
