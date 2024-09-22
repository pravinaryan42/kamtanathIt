using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace KGS.Web.Areas.Admin.Models
{
    public class ImportFileModel
    {
      public HttpPostedFileBase ImportedFile { get; set; }
    }

    public class ImportExistngFileModel
    {
        public ImportExistngFileModel(){
            DoctorList = new List<SelectListItem>();
        }
        public List<SelectListItem> DoctorList { get; set;}

        public Guid SelectedDoctorId { get; set;}

        public HttpPostedFileBase ImportedFile { get; set; }
    }
}