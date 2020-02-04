using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDVO
{
    public class CybReturnWithStatus
    {
        public CybReturnWithStatus()
        {

        }

        public enum CybOperationStatus
        {
            Success,
            Failed,
            Exception,
            Valid,
            PAEnroled
        }

        public object returnObject;
        public Exception ex;
        public string customError;
        public string customMessage;
        public CybOperationStatus status = new CybOperationStatus();
    }
}
