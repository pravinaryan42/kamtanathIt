using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using KGS.Data;
using KGS.DataTable.Search;
using KGS.Dto;
using KGS.Repo;

namespace KGS.Service
{
    public class RecordService : IRecordService
    {
        private readonly IRepository<GeoRecord> repo;
        private readonly IRepository<MasterLocation> repoMasterLocation;
        public RecordService(IRepository<GeoRecord> repo, IRepository<MasterLocation> repoMasterLocation)
        {
            this.repo = repo;
            this.repoMasterLocation = repoMasterLocation;
        }
        public PagedListResult<GeoRecord> GetGeoRecord(SearchQuery<GeoRecord> query, out int totalItems)
        {
            return repo.Search(query, out totalItems);
        }

        public GeoRecord GetGeoRecordById(long id)
        {
            return repo.FindById(id);
        }
        public GeoRecord GetGeoRecordByName(string propertyId)
        {
            return repo.Query().AsTracking().Filter(r => r.PROPERTYID == propertyId).Get().FirstOrDefault();
        }
        public GeoRecord Save(GeoRecord GeoRecord)
        {
            GeoRecord.UPLOADEDON = DateTime.UtcNow;
            repo.Insert(GeoRecord);
            return GeoRecord;
        }

        public GeoRecord Update(GeoRecord GeoRecord)
        {
            GeoRecord.UPLOADEDON = DateTime.UtcNow;
            repo.Update(GeoRecord);
            return GeoRecord;
        }

        public void Delete(GeoRecord GeoRecord)
        {
            repo.ChangeEntityState(GeoRecord, ObjectState.Deleted);
            repo.SaveChanges();
        }

        public List<GeoRecord> GetGeoRecords()
        {
            return repo.Query().AsTracking().Get().OrderBy(s => s.FID).ToList();
        }

        public List<GeoRecord> GetGeoRecordsByGP(string District, string BlockName, string GpName)
        {

            if (!string.IsNullOrEmpty(GpName))
            {
                return repo.Query().AsTracking().Filter(x => x.DISTRICT.ToLower() == District.ToLower() && x.BLOCK.ToLower() == BlockName.ToLower() && x.GP.ToLower() == GpName.ToLower()).Get().OrderBy(s => s.FID).ToList();

            }
            else {
                return repo.Query().AsTracking().Filter(x => x.DISTRICT.ToLower() == District.ToLower() && x.BLOCK.ToLower() == BlockName.ToLower()).Get().OrderBy(s => s.FID).ToList();

            }
        }
        public void SaveCollection(List<GeoRecord> importedGeoRecord)
        {
            repo.ChangeEntityCollectionState(importedGeoRecord, ObjectState.Added);
            repo.SaveChanges();
        }
        public bool IsDuplicateGeoRecord(string propertyId, int? id)
        {
            return repo.Query().Filter(r => (id.Value != 0 && r.FID != id.Value && r.PROPERTYID == propertyId) || (id.Value == 0 && r.PROPERTYID == propertyId)).Get().Count() >= 1;
        }
        public List<DistrictChartModel> GetDistrinctWiseData()
        {
            List<DistrictChartModel> lst = new List<DistrictChartModel>();
            KamtanathDbEntities _context = new KamtanathDbEntities();
            var districtCounts = _context.GeoRecords
           .GroupBy(r => r.DISTRICT)
           .Select(g => new
           {
               District = g.Key,
               Count = g.Count()
           });
            if (districtCounts != null)
            {
                foreach (var district in districtCounts)
                {
                    DistrictChartModel model = new DistrictChartModel();
                    model.DistrictName = district.District;
                    model.Count = district.Count;
                    lst.Add(model);
                }

            }
            return lst;


        }
        public List<System.Web.Mvc.SelectListItem> GetAllDistrict(string District = "")
        {
            return repoMasterLocation.Query().AsTracking().Get().SelectMany(x => new List<System.Web.Mvc.SelectListItem> { new System.Web.Mvc.SelectListItem { Text = x.District, Value = x.District } }).OrderBy(s => s.Text).ToList();

        }
        public List<System.Web.Mvc.SelectListItem> GetAllBlocks(string District = "") {
            if (!string.IsNullOrEmpty(District))
            {
                District = District.ToLower().Trim();
                return repoMasterLocation.Query().Filter(x => x.District.ToLower() == District).AsTracking().Get().SelectMany(x => new List<System.Web.Mvc.SelectListItem> { new System.Web.Mvc.SelectListItem { Text = x.Block, Value = x.Block } }).OrderBy(s => s.Text).ToList();

            }
            else {
                return repoMasterLocation.Query().AsTracking().Get().SelectMany(x => new List<System.Web.Mvc.SelectListItem> { new System.Web.Mvc.SelectListItem { Text = x.Block, Value = x.Block } }).OrderBy(s => s.Text).ToList();

            }
        }

        public List<System.Web.Mvc.SelectListItem> GetAllGPs(string Block = "") {
            if (!string.IsNullOrEmpty(Block))
            {
                Block = Block.ToLower().Trim();
                return repoMasterLocation.Query().Filter(x => x.Block.ToLower() == Block.ToLower()).AsTracking().Get().SelectMany(x => new List<System.Web.Mvc.SelectListItem> { new System.Web.Mvc.SelectListItem { Text = x.Gram_Panchayat, Value = x.Gram_Panchayat } }).OrderBy(s => s.Text).ToList();

            }
            else
            {
                return repoMasterLocation.Query().AsTracking().Get().SelectMany(x => new List<System.Web.Mvc.SelectListItem> { new System.Web.Mvc.SelectListItem { Text = x.Gram_Panchayat, Value = x.Gram_Panchayat } }).OrderBy(s => s.Text).ToList();

            }
        }

        public List<DistrictChartModel> GetUserWiseData()
        {
            List<DistrictChartModel> lst = new List<DistrictChartModel>();
            KamtanathDbEntities _context = new KamtanathDbEntities();
            var districtCounts = _context.GeoRecords
           .GroupBy(r => r.USERNAME)
           .Select(g => new
           {
               District = g.Key,
               Count = g.Count()
           });
            if (districtCounts != null)
            {
                foreach (var district in districtCounts)
                {
                    DistrictChartModel model = new DistrictChartModel();
                    model.DistrictName = district.District;
                    model.Count = district.Count;
                    lst.Add(model);
                }

            }
            return lst;


        }
        public List<DistrictChartModel> GetGPWiseData()
        {
            List<DistrictChartModel> lst = new List<DistrictChartModel>();
            KamtanathDbEntities _context = new KamtanathDbEntities();
            var districtCounts = _context.GeoRecords
           .GroupBy(r => r.GP)
           .Select(g => new
           {
               District = g.Key,
               Count = g.Count()
           });
            if (districtCounts != null)
            {
                foreach (var district in districtCounts)
                {
                    DistrictChartModel model = new DistrictChartModel();
                    model.DistrictName = district.District;
                    model.Count = district.Count;
                    lst.Add(model);
                }

            }
            return lst;


        }
        public void UpdateData(string selectedBlock, string updateFrom, string updateTo) {
            List<GeoRecord> records = new List<GeoRecord>();
            switch (selectedBlock)
            {
                case "DISTRICT":
                    records = repo.Query().Filter(x => x.DISTRICT.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0) {
                        records.ForEach(x => x.DISTRICT = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "BLOCK":
                    records = repo.Query().Filter(x => x.BLOCK.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.BLOCK = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "GP":
                    records = repo.Query().Filter(x => x.GP.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.GP = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "VILLAGE":
                    records = repo.Query().Filter(x => x.VILLAGE.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.VILLAGE = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "HABITATION":
                    records = repo.Query().Filter(x => x.HABITATION.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.HABITATION = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "LATITUDE":
                    records = repo.Query().Filter(x => x.LATITUDE.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.LATITUDE = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "LONGITUDE":
                    records = repo.Query().Filter(x => x.LONGITUDE.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.LONGITUDE = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "PROPERTYID":
                    records = repo.Query().Filter(x => x.PROPERTYID.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.PROPERTYID = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "PROPERTYTYPE":
                    records = repo.Query().Filter(x => x.PROPERTYTYPE.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.PROPERTYTYPE = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "CONSUMERNAME":
                    records = repo.Query().Filter(x => x.CONSUMERNAME.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.CONSUMERNAME = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                case "USERNAME":
                    records = repo.Query().Filter(x => x.USERNAME.ToLower() == updateFrom.ToLower()).Get().ToList();
                    if (records != null && records.Count() > 0)
                    {
                        records.ForEach(x => x.USERNAME = updateTo);
                        repo.UpdateCollection(records);
                    }
                    break;
                default:
                    break;
            }
        }

        public long GetMaxFID(string dISTRICT) {

            return (repo.Query().Filter(x => x.DISTRICT == dISTRICT).Get().Max(x => x.FID))+1??1;
        }
        #region "Dispose"
        public void Dispose()
        {
            if (repo != null)
            {
                repo.Dispose();
            }
            if (repoMasterLocation != null)
            {
                repoMasterLocation.Dispose();
            }
        }



        #endregion "Dispose"


    }
}
