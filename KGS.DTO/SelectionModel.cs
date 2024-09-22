using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
namespace KGS.DTO
{
    public class SelectionModel
    {

        public SelectionModel() {
            DistrictDataList = new List<SelectListItem>();
            BlockDataList = new List<SelectListItem>();
            GPDataList = new List<SelectListItem>();
        }
        public List<SelectListItem> DistrictDataList { get; set; }
        public List<SelectListItem> BlockDataList { get; set; }
        public List<SelectListItem> GPDataList { get; set; }
        public string SelectedBlock { get; set; }
        public string SelectedGP { get; set; }
        public string SelectedDistrict { get; set; }
        public string UpdateFrom { get; set; }
        public string UpdateTo { get; set; }

    }

    public class SelectionMapModel
    {

        public SelectionMapModel()
        {
            MapDataList = new List<LocationMap>();
            BlockDataList = new List<SelectListItem>();
            GPDataList = new List<SelectListItem>();
            DistrictDataList = new List<SelectListItem>();
        }
        public List<SelectListItem> DistrictDataList { get; set; }
        public List<SelectListItem> GPDataList { get; set; }
        public List<SelectListItem> BlockDataList { get; set; }
        public List<LocationMap> MapDataList { get; set; }
        public string SelectedDistrict { get; set; }
        public string SelectedBlock { get; set; }
        public string SelectedGP { get; set; }


    }
}
