using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models.Response
{
    public class SessionListResp
    {
        public SessionListRespData[] Data { get; set; }
    }

    public class SessionListRespData
    {
        public string MT { get; set; }
        public string MID { get; set; }
        public string CN { get; set; }
        public string CID { get; set; }
        public string MN { get; set; }
        public string ML { get; set; }
        public string MI { get; set; }
        public string MTR { get; set; }
        public string SD { get; set; }
        public string DU { get; set; }
        public string RT { get; set; }
        public string SP { get; set; }
        public string GR { get; set; }
        public string MDU { get; set; }
        public string DD { get; set; }
        public Mslst[] MSLst { get; set; }
        public Experience[] Experiences { get; set; }
        public string YTU { get; set; }
        public int SC { get; set; }
    }

    public class Mslst
    {
        public string SC { get; set; }
        public string SD { get; set; }
        public string ASD { get; set; }
        public string CID { get; set; }
        public string CN { get; set; }
        public string SID { get; set; }
        public string EX { get; set; }
        public string VISTAEX { get; set; }
        public string AV { get; set; }
        public bool isAgeRestricted { get; set; }
        public string CPPText { get; set; }
        public string CPPHText { get; set; }
        public bool isSessionUpgradable { get; set; }
    }

    public class Experience
    {
        public string Type { get; set; }
        public string ImageUrl { get; set; }
    }

}