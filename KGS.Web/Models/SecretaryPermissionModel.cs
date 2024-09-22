using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGS.Web.Models
{
    public class SecretaryPermissionModel
    {
        public SecretaryPermissionModel() {

            DoctorList = new List<SelectListItem>();
            StatusLists = new List<SelectListItem>();
            TypeLists = new List<SelectListItem>();
        }

        public List<SelectListItem> StatusLists { get; set; }
        public List<SelectListItem> TypeLists { get; set; }
        public int SelectedStatus { get; set; }
        public int SelectedType { get; set; }
        public List<SelectListItem> DoctorList { get; set;}
        public Guid SecretaryId { get; set;}
        public Guid PrimaryDoctor { get; set; }
        public bool CanScheduleAppointment{ get; set;}
        public bool CanManageCategory{ get; set; }
        public bool CanCheckInCheckOut{ get; set; }

        public bool CanReviewIntake { get; set;}
        public bool CanManageFAQ { get; set; }


        public Guid SelectedDoctorId { get; set; }



    }
}