using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Cells;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using WebAPI_DataLoader.Common.Enitities;
using WebAPI_DataLoader.Common;

namespace WebAPI_DataLoader
{
    public class Converter
    {
        /*public string GenerateJson(FileInfos request)
        {
            try
            {
                //var context = HttpContext.Current.Request;

                string response = string.Empty;

                string fileType = "Xlsx";
                FileType ftype = (FileType)Enum.Parse(typeof(FileType), fileType.ToLower());

                switch (ftype)
                {
                    case FileType.xlsx:
                        response = GenerateJsonFromExcel(request);
                        break;

                    case FileType.csv | FileType.xls:
                        response = GenerateJsonFromCSV(request);
                        break;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        public List<Entity> GenerateJson(FileOption fileOption)
        {
            try
            {
                string response = string.Empty;
                List<Entity> entites = new List<Entity>();
                foreach (UploadedFile uploadedFile in fileOption.UploadedFiles)
                {
                    FileInfo fileInfo = new FileInfo(uploadedFile.FileName);
                    FileType ftype = (FileType)Enum.Parse(typeof(FileType), fileInfo.Extension.Remove(0, 1));
                    LoadOptions loadOptions = null;
                    switch (ftype)
                    {
                        case FileType.xlsx:
                            loadOptions = new LoadOptions(LoadFormat.Xlsx);
                            break;

                        default:
                            loadOptions = GetLoadOption(fileOption);
                            break;
                    }
                    Entity entity = GenerateEntity(fileOption, loadOptions, uploadedFile, new Csv_Xls_DataTypeMapping());
                    entites.Add(entity);
                }
                return entites;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Entity GenerateJsonFromExcel(UploadedFile uploadedFile, DataTypeMapping dataTypeMapping)
        {
            try
            {
                Entity entity;
                using (FileStream fstream = File.OpenRead(uploadedFile.FilePath))
                {
                    LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
                    Workbook workbook = new Workbook(fstream, loadOptions);
                    entity = new Entity
                    {
                        FileName = uploadedFile.FileName,
                        Uuid = uploadedFile.Uid
                    };

                    if (workbook.Worksheets != null)
                    {
                        entity.sheets = new List<Sheet>();

                        foreach (Worksheet workSheet in workbook.Worksheets)
                        {
                            Sheet sheet = new Sheet();
                            sheet.SheetName = workSheet.Name;
                            sheet.selected = true;
                            sheet.records = new List<Dictionary<string, object>>();
                            sheet.ColumnInfos = new List<ColumnInfo>();

                            #region Row Creation Work

                            for (int rowIndex = 1; rowIndex < workSheet.Cells.Rows.Count; rowIndex++)
                            {
                                Dictionary<string, object> rowData = new Dictionary<string, object>();
                                #region Column Creation Work
                                for (int columnIndex = 0; columnIndex <= workSheet.Cells.MaxDataColumn; columnIndex++)
                                {
                                    Cell cellKey = workSheet.Cells.GetCell(0, columnIndex);
                                    Cell cell = workSheet.Cells.GetCell(rowIndex, columnIndex);
                                    string key = cellKey.Value.ToString();
                                    string cellValue = cell.Value.ToString();
                                    CellData cellData = new CellData(key, cellValue);
                                    rowData.Add(cellData.Key, cellData.Value);

                                    if (rowIndex == 1)
                                    {
                                        sheet.ColumnInfos.Add(new ColumnInfo()
                                        {
                                            ColumnName = cellKey.Value.ToString(),
                                            ColumnDataType = dataTypeMapping.GetDatype(cell.GetStyle().Number.ToString())
                                        });
                                    }
                                }
                                sheet.records.Add(rowData);
                                #endregion
                            }
                            entity.sheets.Add(sheet);
                            #endregion
                        }
                    }
                    fstream.Flush();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Entity GenerateEntity(FileOption fileOption, LoadOptions loadOptions, UploadedFile uploadedFile, DataTypeMapping dataTypeMapping)
        {
            try
            {
                Entity entity;
                using (FileStream fstream = File.OpenRead(uploadedFile.FilePath))
                {
                    entity = new Entity
                    {
                        FileName = uploadedFile.FileName,
                    };
                    Workbook workbook = new Workbook(fstream, loadOptions);

                    if (workbook.Worksheets != null)
                    {
                        entity.sheets = new List<Sheet>();
                        foreach (Worksheet workSheet in workbook.Worksheets)
                        {
                            Sheet sheet = new Sheet();
                            sheet.SheetName = workSheet.Name;
                            sheet.selected = true;
                            sheet.records = new List<Dictionary<string, object>>();
                            sheet.ColumnInfos = new List<ColumnInfo>();

                            #region Row Creation Work

                            for (int rowIndex = fileOption.DataRow - 1; rowIndex < workSheet.Cells.MaxRow + 1; rowIndex++)
                            {
                                Dictionary<string, object> rowData = new Dictionary<string, object>();

                                #region Column Creation Work

                                for (int columnIndex = 0; columnIndex < workSheet.Cells.MaxDataColumn; columnIndex++)
                                {
                                    Cell cellKey = workSheet.Cells.GetCell(fileOption.HeaderRow -1, columnIndex);
                                    Cell cell = workSheet.Cells.GetCell(rowIndex, columnIndex);

                                    string key = cellKey.Value.ToString();
                                    string cellValue = cell.Value.ToString();
                                    CellData cellData = new CellData(key, cellValue);
                                    rowData.Add(cellData.Key, cell.Value);

                                    if (rowIndex == fileOption.DataRow)
                                    {
                                        sheet.ColumnInfos.Add(new ColumnInfo()
                                        {
                                            ColumnName = cellKey.Value.ToString(),
                                            ColumnDataType = dataTypeMapping.GetDatype(cell.GetStyle().Number.ToString())
                                        });
                                    }
                                }

                                #endregion
                                sheet.records.Add(rowData);
                            }
                            entity.sheets.Add(sheet);
                            #endregion
                        }
                    }
                    fstream.Flush();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LoadOptions GetLoadOption(FileOption fileOption) {
            if (fileOption.SepratedOption == "comma") {
                return new LoadOptions(LoadFormat.Csv);
            }
            if (fileOption.SepratedOption == "tab")
            {
                return new LoadOptions(LoadFormat.Tsv);
            }
            TxtLoadOptions txtLoadOptions = new TxtLoadOptions();
            if(fileOption.SepratedOption == "pipe")
            {
                txtLoadOptions.Separator = '|';
                return txtLoadOptions;
            }
            if (fileOption.SepratedOption == "semicolon")
            {
                txtLoadOptions.Separator = ':';
                return txtLoadOptions;
            }
            if (fileOption.SepratedOption == "space")
            {
                txtLoadOptions.Separator = ' ';
                return txtLoadOptions;
            }
            txtLoadOptions.Separator = fileOption.SepratedOption.ToCharArray()[0];
            return txtLoadOptions;
        }

        /*public string GenerateJsonFromExcel(FileInfos request)
        {
            try
            {
                string val = request.stream;
                string response = string.Empty;
                Dictionary<string, string> columnDataType = new Dictionary<string, string>();
                APIResponseModel apiResponseModel = new APIResponseModel();
                ColumnInfos columnInfoCollection = new ColumnInfos();
                ColumnInfo columnInfo = null;

                using (FileStream fstream = File.OpenRead("D:\\DemoFile\\FileB.xlsx"))
                {
                    LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);

                    #region Test Region
                    //try
                    //{

                    //    System.Net.Http.MultipartMemoryStreamProvider provider = new System.Net.Http.MultipartMemoryStreamProvider();

                    //System.Threading.Tasks.Task<string> str = System.Net.Http.HttpContentMultipartExtensions.ReadAsMultipartAsync( // await HttpContext.Current.Request.Content.ReadAsMultipartAsync(provider);

                    // extract file name and file contents
                    //Stream stream = new MemoryStream(await provider.Contents[0].ReadAsByteArrayAsync());

                    ////get fileName
                    //var filename = provider.Contents[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty);

                    ////Check file type 
                    //if (filename.EndsWith(".xls"))
                    //{
                    //    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //}

                    //else if (filename.EndsWith(".xlsx"))
                    //{
                    //    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //}
                    //else
                    //{
                    //    return "Not Valid";
                    //}

                    //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(val);
                    //MemoryStream memStream = new MemoryStream();
                    //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //memStream.Write(bytes, 0, bytes.Length);
                    //memStream.Seek(0, SeekOrigin.Begin);
                    //Object obj = (Object)binForm.Deserialize(memStream);
                    //Workbook workbook2 = new Workbook(obj, loadOptions);

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    #endregion

                    Workbook workbook = new Workbook(fstream, loadOptions);

                    if (workbook.Worksheets != null)
                    {
                        CellData cellData;
                        Entity entity;
                        EntitiesCollection entitiesCollection = new EntitiesCollection();
                        RowData rowData;
                        RecordsCollection recordsCollection;
                        RowCollection rowCollection;
                        Records record;
                        bool isTypeAddInColumnInfo = true;
                        foreach (Worksheet sheet in workbook.Worksheets)
                        {
                            entity = new Entity
                            {
                                Name = sheet.Name
                            };

                            recordsCollection = new RecordsCollection();
                            rowCollection = new RowCollection();

                            #region Row Creation Work

                            for (int rowIndex = 1; rowIndex < sheet.Cells.Rows.Count; rowIndex++)
                            {
                                Row row = sheet.Cells.GetRow(rowIndex);
                                rowData = new RowData();

                                #region Column Creation Work

                                for (int columnIndex = 0; columnIndex <= sheet.Cells.MaxDataColumn; columnIndex++)
                                {
                                    Cell cellKey = sheet.Cells.GetCell(0, columnIndex);
                                    Cell cell = sheet.Cells.GetCell(rowIndex, columnIndex);

                                    int number = cell.GetStyle().Number;

                                    string key = cellKey.Value.ToString();
                                    string cellValue = cell.Value.ToString();
                                    cellData = new CellData(key, cellValue);
                                    rowData.Add(cellData);

                                    if (isTypeAddInColumnInfo)
                                    {
                                        columnInfo = new ColumnInfo()
                                        {
                                            ColumnName = key,
                                            ColumnDataType = number
                                        };
                                        columnInfoCollection.Add(columnInfo);
                                    }
                                }

                                #endregion

                                record = new Records
                                {
                                    data = rowData
                                };
                                recordsCollection.Add(record);

                                isTypeAddInColumnInfo = false;
                            }

                            #endregion

                            entity.Records = recordsCollection;
                            entitiesCollection.Add(entity);
                        }
                        if (entitiesCollection != null && entitiesCollection.Count > 0)
                        {
                            JsonEntities jsonEntities = new JsonEntities
                            {
                                Entities = entitiesCollection
                            };
                            //File.WriteAllText("D:\\DemoFile\\FileA-CSV.json", JsonConvert.SerializeObject(jsonEntities, Formatting.Indented));
                            apiResponseModel.Entities = JsonConvert.SerializeObject(jsonEntities, Formatting.Indented);
                            apiResponseModel.ColumnInfoCollection = columnInfoCollection;

                            response = JsonConvert.SerializeObject(apiResponseModel, Formatting.Indented);
                        }
                    }
                }

                #region Commented COde

                //byte[] bytes = Convert.FromBase64String(val);
                //var contents = new System.Net.Http.StreamContent(new MemoryStream(bytes));

                //byte[] imageBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(val);
                //byte[] imageBytesUTF8 = System.Text.Encoding.UTF8.GetBytes(val);
                //byte[] imageBytesUTF7 = System.Text.Encoding.UTF7.GetBytes(val);
                //byte[] imageBytesUTF32 = System.Text.Encoding.UTF32.GetBytes(val);


                //FileStream fstream2 = Write(info, 0, info.Length);

                //FileStream fs = new FileStream(@"C:\Windows\Temp\MyTest.txt", FileMode.Create);

                //byte[] info = new System.Text.UTF8Encoding(true).GetBytes(val);

                //fs.Write(info, 0, info.Length);

                //Workbook workbook2 = new Workbook(fs, loadOptions);

                //MemoryStream stream = new MemoryStream(imageBytes);
                //MemoryStream streamUTF8 = new MemoryStream(imageBytesUTF8);
                //MemoryStream streamUTF7 = new MemoryStream(imageBytesUTF7);
                //MemoryStream streamUTF32 = new MemoryStream(imageBytesUTF32);


                //Workbook workbook2 = new Workbook(stream, loadOptions);
                //Workbook workbookUTF8 = new Workbook(streamUTF8, loadOptions);
                //Workbook workbookUTF7 = new Workbook(streamUTF7, loadOptions);
                //Workbook workbookUTF32 = new Workbook(streamUTF32, loadOptions);


                //int numOfBytes = val.Length / 8;
                //byte[] bytes = new byte[numOfBytes];
                //for (int i = 0; i < numOfBytes; ++i)
                //{
                //    bytes[i] = Convert.ToByte(val.Substring(8 * i, 8), 2);
                //}

                //Workbook workbook2 = new Workbook(stream, loadOptions);

                #endregion


                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateJsonFromCSV(FileInfos request)
        {
            try
            {
                string response = string.Empty;
                FileStream fstream = File.OpenRead("D:\\DemoFile\\FileB.csv");
                //FileStream fstream = File.OpenRead("D:\\DemoFile\\97-2003.xls");
                LoadOptions loadOptions = new LoadOptions(LoadFormat.Csv);
                //loadOptions.ConvertNumericData = false;

                Workbook workbook = new Workbook(fstream, loadOptions);

                if (workbook.Worksheets != null)
                {
                    CellData cellData;
                    Entity entity;
                    EntitiesCollection entitiesCollection = new EntitiesCollection();
                    RowData rowData;
                    RecordsCollection recordsCollection;
                    RowCollection rowCollection;
                    Records record;
                    foreach (Worksheet sheet in workbook.Worksheets)
                    {
                        entity = new Entity
                        {
                            Name = sheet.Name
                        };

                        recordsCollection = new RecordsCollection();
                        rowCollection = new RowCollection();

                        #region Row Creation Work

                        for (int rowIndex = 1; rowIndex < sheet.Cells.MaxRow; rowIndex++)
                        {
                            Row row = sheet.Cells.GetRow(rowIndex);
                            rowData = new RowData();

                            #region Column Creation Work

                            for (int columnIndex = 0; columnIndex < sheet.Cells.MaxColumn; columnIndex++)
                            {
                                Cell cellKey = sheet.Cells.GetCell(0, columnIndex);
                                Cell cell = sheet.Cells.GetCell(rowIndex, columnIndex);

                                //cellKey.GetStyle().Number

                                string key = cellKey.Value.ToString();
                                string cellValue = cell.Value.ToString();
                                cellData = new CellData(key, cellValue);
                                rowData.Add(cellData);
                            }

                            #endregion

                            record = new Records
                            {
                                data = rowData
                            };
                            recordsCollection.Add(record);
                        }

                        #endregion

                        entity.Records = recordsCollection;
                        entitiesCollection.Add(entity);
                    }
                    if (entitiesCollection != null && entitiesCollection.Count > 0)
                    {
                        JsonEntities jsonEntities = new JsonEntities
                        {
                            Entities = entitiesCollection
                        };
                        //File.WriteAllText("D:\\DemoFile\\FileA-CSV.json", JsonConvert.SerializeObject(jsonEntities, Formatting.Indented));
                        response = JsonConvert.SerializeObject(jsonEntities, Formatting.Indented);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
    }
}