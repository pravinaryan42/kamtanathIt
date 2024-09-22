using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KGS.DTO
{
    public class GeoRecordData
    {

        public long FID { get; set; }
        public string DISTRICT { get; set; }
        public string BLOCK { get; set; }
        public string GP { get; set; }
        public string VILLAGE { get; set; }
        public string HABITATION { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string PROPERTYID { get; set; }
        public string PROPERTYTYPE { get; set; }
        public int NUMBE_ROF_FLOORS { get; set; }
        public string CONSTRUCTIONTYPE { get; set; }
        public string CONSUMERNAME { get; set; }
        public string PHONENO { get; set; }
        public int FAMILYCOUNT { get; set; }
        public string WATERSUPPLYTYPE { get; set; }
        public decimal SUPPLY_IN_HOURS { get; set; }
        public int NUMBER_OF_CONNECTIONS { get; set; }
        public string REMARKS { get; set; }
        public string STATUS { get; set; }
        public string SURVEYOR_LOCATION { get; set; }
        public string USERNAME { get; set; }
        public string CONNECTIONSTATUS { get; set; }
        public string CONNECTIONPHOTO { get; set; }
        public int NUMBEROFROOM { get; set; }
        public Guid? UPLOADEDBY { get; set; }
        public DateTime UPLOADEDON { get; set; }

    }
}
