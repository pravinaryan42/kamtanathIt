using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KGS.Data;
using KGS.DataTable.Search;
using KGS.Dto;

namespace KGS.Service
{
    public interface IRecordService
    {
        DataTable.Search.PagedListResult<GeoRecord> GetGeoRecord(SearchQuery<GeoRecord> query, out int totalItems);

        GeoRecord GetGeoRecordById(int id);
        GeoRecord GetGeoRecordByName(string allergyName);
        GeoRecord Save(GeoRecord Allergies);

        GeoRecord Update(GeoRecord Allergies);

        void Delete(GeoRecord allergy);

        List<GeoRecord> GetGeoRecords();

        bool IsDuplicateGeoRecord(string allergyName, int? id);
        void SaveCollection(List<GeoRecord> importedAllergy);
        List<DistrictChartModel> GetDistrinctWiseData();
        List<DistrictChartModel> GetUserWiseData();
        List<System.Web.Mvc.SelectListItem> GetAllBlocks(string District = "");
        List<DistrictChartModel> GetGPWiseData();
        void UpdateData(string selectedBlock, string updateFrom, string updateTo);
        List<GeoRecord> GetGeoRecordsByGP(string District,string BlockName, string GpName);
        List<SelectListItem> GetAllGPs(string Block = "");
        List<System.Web.Mvc.SelectListItem> GetAllDistrict(string District = "");
    }
}
