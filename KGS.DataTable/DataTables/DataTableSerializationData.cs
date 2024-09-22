using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KGS.DataTable.DataTables
{
    [DataContract]
    internal class DataTableSerializationData
    {
        [DataMember(Name = "sEcho")]
        public string sEcho { get; set; }

        [DataMember(Name = "iTotalRecords")]
        public int iTotalRecords { get; set; }

        [DataMember(Name = "iTotalDisplayRecords")]
        public int iTotalDisplayRecords { get; set; }

        [DataMember(Name = "sColumns")]
        public string sColumns { get; set; }

        [DataMember(Name = "aaData")]
        public List<List<string>> aaData { get; set; }
    }
}