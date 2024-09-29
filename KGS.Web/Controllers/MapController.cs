using KGS.Core;
using KGS.Data;
using KGS.DataTable.Extension;
using KGS.DataTable.Search;
using KGS.DataTable.Sort;
using KGS.DTO;
using KGS.Service;
using KGS.Web.Areas.Admin.Models;
using KGS.Web.Code.Attributes;
using KGS.Web.Code.LIBS;
using KGS.Web.Models.Others;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KGS.Web.Controllers
{
    [CustomAuthorization]
    public class MapController : BaseController
    {
        private readonly IRecordService recordService;

        public MapController(IRecordService recordService)
        {
            this.recordService = recordService;
        }
        // GET: GeoRecord
        [OutputCache(Duration = 60, VaryByParam = "none", NoStore = false)]
        public ActionResult Index()
        {
            SelectionMapModel model =new SelectionMapModel();
            List<SelectListItem> lstmodel = new List<SelectListItem>();
            lstmodel = recordService.GetAllDistrict();
            if (lstmodel!=null && lstmodel.Count > 0) {
                foreach (string text in lstmodel.Select(x => x.Text).Distinct()) {
                    model.DistrictDataList.Add(new SelectListItem { Text = text, Value = text });
                }
                model.DistrictDataList = model.DistrictDataList.OrderBy(x => x.Text).ToList();
            }
            model.SelectedDistrict = model.DistrictDataList.FirstOrDefault().Text;

            List<SelectListItem> lstBlockmodel = new List<SelectListItem>();
            lstBlockmodel = recordService.GetAllBlocks(model.SelectedDistrict);
            if (lstBlockmodel != null && lstBlockmodel.Count > 0)
            {
                foreach (string text in lstBlockmodel.Select(x => x.Text).Distinct())
                {
                    model.BlockDataList.Add(new SelectListItem { Text = text, Value = text });
                }
                model.BlockDataList = model.BlockDataList.OrderBy(x => x.Text).ToList();
            }
            model.SelectedBlock = model.BlockDataList.FirstOrDefault().Text;

            List<SelectListItem> lstGPmodel = new List<SelectListItem>();
           lstGPmodel = recordService.GetAllGPs(model.SelectedBlock);
            if (lstGPmodel != null && lstGPmodel.Count > 0)
            {
                foreach (string text in lstGPmodel.Select(x => x.Text).Distinct())
                {
                    model.GPDataList.Add(new SelectListItem { Text = text, Value = text });
                }
                model.GPDataList = model.GPDataList.OrderBy(x => x.Text).ToList();
            }

            model.SelectedGP = model.GPDataList.FirstOrDefault().Text;
            model.MapDataList = new List<LocationMap>();
            List<GeoRecord> locations = recordService.GetGeoRecordsByGP(model.SelectedDistrict.Trim(),model.SelectedBlock.Trim(), model.SelectedGP.Trim());
            if (locations.Any())
            {
                foreach (var item in locations)
                {

                    LocationMap mp = new LocationMap();
                    mp.Id = item.ID;
                    mp.Name = item.CONSUMERNAME;
                    mp.Latitude = item.LATITUDE;
                    mp.Longitude = item.LONGITUDE;
                    mp.ConnectionType = item.CONNECTIONTYPE == "N" ? "N" : "E";
                    model.MapDataList.Add(mp);
                }
            }

            return View(model);
        }

        [HttpGet]
        public string GetLocations(string DistrictName,string BlockName,string GPName) {
            List<LocationMap> locations = new List<LocationMap>();
            var lstLocations = recordService.GetGeoRecordsByGP(DistrictName?.Trim()??"",BlockName?.Trim() ?? "", GPName?.Trim() ?? "");
            if (lstLocations.Any())
            {
                foreach (var item in lstLocations)
                {

                    LocationMap mp = new LocationMap();
                    mp.Id = item.ID;
                    mp.Name = item.CONSUMERNAME;
                    mp.Latitude = item.LATITUDE;
                    mp.Longitude = item.LONGITUDE;
                    mp.ConnectionType = item.CONNECTIONTYPE == "N" ? "N" : "E";
                    locations.Add(mp);
                }
            }
            return JsonConvert.SerializeObject(locations);

        }

        public ActionResult ExportKml(string DistrictName, string BlockName, string GPName)
        {
            string name= DistrictName?.Trim() ?? ""+ BlockName?.Trim() ?? ""+ GPName?.Trim() ?? "";
            List<LocationKML> locations = new List<LocationKML>();
            var lstLocations = recordService.GetGeoRecordsByGP(DistrictName?.Trim() ?? "", BlockName?.Trim() ?? "", GPName?.Trim() ?? "");
            if (lstLocations.Any())
            {
                foreach (var item in lstLocations)
                {

                    LocationKML mp = new LocationKML();
                   
                    mp.Name = item.CONSUMERNAME;
                    mp.Latitude = Convert.ToDouble(item.LATITUDE);
                    mp.Longitude = Convert.ToDouble(item.LONGITUDE);
                    locations.Add(mp);
                }
            } 
            // Create the KML content
            var kmlContent = GenerateKml(locations);

            // Return the KML file
            return File(Encoding.UTF8.GetBytes(kmlContent), "application/vnd.google-earth.kml+xml", name+".kml");
        }

        private string GenerateKml(List<LocationKML> locations)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
            sb.AppendLine("<Document>");

            foreach (var location in locations)
            {
                sb.AppendLine("  <Placemark>");
                sb.AppendLine($"    <name>{location.Name}</name>");
                sb.AppendLine("    <Point>");
                sb.AppendLine($"      <coordinates>{location.Longitude},{location.Latitude},0</coordinates>");
                sb.AppendLine("    </Point>");
                sb.AppendLine("  </Placemark>");
            }

            sb.AppendLine("</Document>");
            sb.AppendLine("</kml>");

            return sb.ToString();
        }


        /// <summary>
        /// Get Location
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(DataTable.DataTables.DataTable dataTable,string SelectedBlock)
        {
            List<DataTableRow> table = new List<DataTableRow>();
            List<int> column1 = new List<int>();
            for (int i = dataTable.iDisplayStart; i < dataTable.iDisplayStart + dataTable.iDisplayLength; i++)
            {
                column1.Add(i);
            }
            var query = new SearchQuery<GeoRecord>();
            if (!string.IsNullOrEmpty(dataTable.sSearch))
            {
                string sSearch = dataTable.sSearch.ToLower();
                query.AddFilter(q => (q.CONSUMERNAME.Contains(sSearch)) || (q.PROPERTYTYPE.Contains(sSearch)) || (q.STATUS.Contains(sSearch)));
            }
            if (!string.IsNullOrEmpty(SelectedBlock) && SelectedBlock != "All") {
                query.AddFilter(q => q.BLOCK == SelectedBlock);
            }
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"];
            switch (sortColumnIndex)
            {
                case 2:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.DISTRICT, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 3:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.BLOCK, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;

                case 4:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.GP, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;

                case 6:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.VILLAGE, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;

                case 7:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.HABITATION, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 8:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.LATITUDE, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 9:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.LONGITUDE, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;

                case 10:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.PROPERTYID, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 11:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.PROPERTYTYPE, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 12:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, int?>(q => q.NUMBE_ROF_FLOORS, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;

                case 13:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.CONSTRUCTIONTYPE, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 14:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.CONSUMERNAME, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 15:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.PHONENO, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;

                case 16:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, int?>(q => q.FAMILYCOUNT, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 17:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.WATERSUPPLYTYPE, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 18:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, decimal?>(q => q.SUPPLY_IN_HOURS, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 19:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, int?>(q => q.NUMBER_OF_CONNECTIONS, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 20:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.REMARKS, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                case 21:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, string>(q => q.STATUS, sortDirection == "asc" ? SortDirection.Ascending : SortDirection.Descending));
                    break;
                default:
                    query.AddSortCriteria(new ExpressionSortCriteria<GeoRecord, long>(q => q.ID, SortDirection.Descending));
                    break;
            }
            query.Take = dataTable.iDisplayLength;
            query.Skip = dataTable.iDisplayStart;

            int count = dataTable.iDisplayStart + 1, total = 0;

            IEnumerable<GeoRecord> records = recordService.GetGeoRecord(query, out total).Entities;
            foreach (var item in records)
            {
                table.Add(new DataTableRow("rowId" + count.ToString(), "dtrowclass")
                {
                    item.ID.ToString(),//0
                    item.FID.ToString(),//1             
                    item.DISTRICT,
                    item.BLOCK,
                    item.GP,
                    item.VILLAGE,
                    item.HABITATION,
                    item.LATITUDE,
                    item.LONGITUDE,
                    item.PROPERTYID,
                    item.PROPERTYTYPE,
                    Convert.ToString(item.NUMBE_ROF_FLOORS),
                    item.CONSTRUCTIONTYPE,
                    item.CONSUMERNAME,
                    item.PHONENO,
                    Convert.ToString(item.FAMILYCOUNT),
                    Convert.ToString(item.WATERSUPPLYTYPE),
                    Convert.ToString(item.SUPPLY_IN_HOURS),
                    Convert.ToString(item.NUMBER_OF_CONNECTIONS),
                    item.REMARKS,
                    item.STATUS,
                    item.SURVEYOR_LOCATION,
                    item.USERNAME,
                    item.CONNECTIONSTATUS,
                    item.CONNECTIONPHOTO,//4,         
                    Convert.ToString(item.NumberOfRoom),
                });
                count++;
            }
            return new DataTableResultExt(dataTable, table.Count, total, table);
        }

        [HttpGet]
        public PartialViewResult ImportRecord()
        {
            ImportFileModel model = new ImportFileModel();
            return PartialView("ImportRecord", model);
        }
        private string test(System.Data.DataTable dt, string path)
        {

            string csv = string.Empty, resultPath = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + '\t';
            }

            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + '\t';
                }

                //Add new line.
                csv += "\r\n";
            }
            resultPath = path + ".xls";
            string filepath = Server.MapPath(resultPath);
            System.IO.File.WriteAllText(filepath, csv);
            return resultPath;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ImportRecordData(ImportFileModel model)
        {
           
            int count = 0;
            int insertedcount = 0;
            string resultPath = string.Empty;

            if (model != null && model.ImportedFile != null && model.ImportedFile.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(model.ImportedFile.FileName);

                try
                {
                    string directoryPath = SiteKeys.ImportedFilePath + "Results";
                    string directoryPathDelete = Server.MapPath(directoryPath);
                    if (Directory.Exists(directoryPath))
                    {
                        // Get all file paths in the directory
                        string[] files = Directory.GetFiles(directoryPath);
                        // Loop through each file and delete it
                        foreach (string file in files)
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                                Console.WriteLine($"Deleted file: {file}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                            }
                        }
                    }

                    var fileName = DateTime.Now.Ticks;
                    string filePathtoSave = SiteKeys.ImportedFilePath + string.Format("Results/Record{0}", DateTime.Now.Ticks);
                    string path = Server.MapPath(string.Format(filePathtoSave + "{0}", fileExtension));
                    model.ImportedFile.SaveAs(path);
                    DataSet ds = new DataSet();
                    if (fileExtension == ".csv" || fileExtension == ".CSV")
                    {
                        ds = ImportData.ConvertCsvToDataSet(model.ImportedFile.InputStream);
                    }
                    else
                    {
                        ds = ImportData.ConvertExcelToDataSet(model.ImportedFile.InputStream);
                    }

                    List<GeoRecord> records = new List<GeoRecord>();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        System.Data.DataTable dtAllergy = ds.Tables[0];
                        count = dtAllergy != null ? dtAllergy.Rows.Count : 0;
                        IQueryable<GeoRecord> allergiesList = recordService.GetGeoRecords().AsQueryable();
                        bool isSuccess = true;
                        foreach (DataRow r in dtAllergy.Rows)
                        {

                            GeoRecord importedrecord = new GeoRecord();
                            string fid = Convert.ToString(r["FID"]);
                            importedrecord.FID = Convert.ToInt64(fid ?? "0");
                            importedrecord.DISTRICT = Convert.ToString(r["DISTRICT"]);
                            importedrecord.BLOCK = Convert.ToString(r["BLOCK"]);
                            importedrecord.GP = Convert.ToString(r["GP"]);
                            importedrecord.VILLAGE = Convert.ToString(r["VILLAGE"]);
                            importedrecord.HABITATION = Convert.ToString(r["HABITATION"]);
                            importedrecord.LATITUDE = Convert.ToString(r["LATITUDE"]);
                            importedrecord.LONGITUDE = Convert.ToString(r["LONGITUDE"]);
                            importedrecord.PROPERTYID = Convert.ToString(r["PROPERTYID"]);
                            importedrecord.PROPERTYTYPE = Convert.ToString(r["PROPERTYTYPE"]);
                            importedrecord.NUMBE_ROF_FLOORS = Convert.ToInt32(r["NUMBER_OF_FLOORS"] ?? "0");
                            importedrecord.CONSTRUCTIONTYPE = Convert.ToString(r["CONSTRUCTIONTYPE"]);
                            importedrecord.CONSUMERNAME = Convert.ToString(r["CONSUMERNAME"]);
                            importedrecord.PHONENO = Convert.ToString(r["PHONENO"]);
                            importedrecord.FAMILYCOUNT = Convert.ToInt32(r["FAMILYCOUNT"]);
                            importedrecord.WATERSUPPLYTYPE = (Convert.ToString(r["WATERSUPPLYTYPE"]) ?? "0");
                            string supp = Convert.ToString(r["SUPPLY_IN_HOURS"]);
                            importedrecord.SUPPLY_IN_HOURS = Convert.ToDecimal(!string.IsNullOrEmpty(supp) ? supp : "0");
                            string suppConnection = Convert.ToString(r["NUMBER_OF_CONNECTIONS"]);
                            importedrecord.NUMBER_OF_CONNECTIONS = Convert.ToInt32(!string.IsNullOrEmpty(suppConnection) ? suppConnection : "0");
                            importedrecord.REMARKS = Convert.ToString(r["REMARKS"]);
                            importedrecord.STATUS = Convert.ToString(r["STATUS"]);
                            importedrecord.SURVEYOR_LOCATION = Convert.ToString(r["SURVEYOR_LOCATION"]);
                            importedrecord.USERNAME = Convert.ToString(r["USERNAME"]);
                            importedrecord.CONNECTIONSTATUS = Convert.ToString(r["CONNECTIONSTATUS"]);
                            importedrecord.CONNECTIONPHOTO = Convert.ToString(r["CONNECTIONPHOTO"]);
                            string NumberOfRoom = Convert.ToString(r["NUMBEROFROOM"]);
                            importedrecord.NumberOfRoom = Convert.ToInt32(!string.IsNullOrEmpty(NumberOfRoom) ? NumberOfRoom:"0");
                            importedrecord.UPLOADEDBY = CurrentUser.UserId;
                            importedrecord.UPLOADEDON = DateTime.Now;
                            if (!(allergiesList.Any(x =>x.BLOCK== importedrecord.BLOCK && x.PROPERTYID== importedrecord.PROPERTYID && x.PROPERTYTYPE== importedrecord.PROPERTYTYPE)))
                            {
                                records.Add(importedrecord);
                                r["Result"] = "Sucess";

                            }
                            else
                            {
                                r["Reason"] = "Property already exists";
                                isSuccess = false;
                            }


                        }
                        if (!isSuccess)
                        {
                            resultPath = test(ds.Tables[0], filePathtoSave);
                        }
                    }
                    insertedcount = records != null ? records.Count : 0;
                    recordService.SaveCollection(records);

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Fail", ex.Message,false);
                    return NewtonSoftJsonResult(new RequestOutcome<string> { RedirectUrl = @Url.Action("Index", "GeoRecord") });
                }

            }
            ShowSuccessMessage("Success", string.Format("Total {0} records(s) are imported successfully out of {1}.", insertedcount, count), false);

            return NewtonSoftJsonResult(new RequestOutcome<string> { Message = resultPath, RedirectUrl = @Url.Action("Index", "GeoRecord") });

        }
        #region Delete Brand

        [HttpGet]
        public PartialViewResult DeleteRecord()
        {
            return PartialView("_ModalDelete", new Modal
            {
                Size = ModalSize.Small,
                Message = "Are you sure want to delete this Record?",
                Header = new ModalHeader { Heading = "Delete Record" },
                Footer = new ModalFooter { SubmitButtonText = "Yes", CancelButtonText = "No" }
            });
        }

        [HttpPost]
        public ActionResult DeleteRecord(int id)
        {
           
            var brand = recordService.GetGeoRecordById(id);
            try
            {
                if (brand != null)
                {

                    recordService.Delete(brand);

                }
                ShowSuccessMessage("Success", "Record is successfully deleted", false);
                return NewtonSoftJsonResult(new RequestOutcome<string> { Message = "Record is successfully deleted" , RedirectUrl = @Url.Action("index", "GeoRecord") });
            }
            catch
            {
                ShowErrorMessage("Failed", "Can't delete Record because it is already referenced by another module.", false);
                return NewtonSoftJsonResult(new RequestOutcome<string> { Message = "can't delete because it is already referenced by another module.", RedirectUrl = @Url.Action("index", "GeoRecord") });
            }

        }
        #endregion
        private Dictionary<string, int> GetColumnNames(ExcelWorksheet worksheet)
        {
            var columnNames = new Dictionary<string, int>();
            var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

            foreach (var cell in headerRow)
            {
                columnNames[cell.Text] = cell.Start.Column;
            }

            return columnNames;
        }

        private int GetColumnIndex(Dictionary<string, int> columnNames, string columnName)
        {
            return columnNames.TryGetValue(columnName, out var columnIndex) ? columnIndex : -1;
        }
        public FileResult Download()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(SiteKeys.TemplateFilePath + "SampleDataImport.xlsx"));
            string fileName = "SampleDataImport.Xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}