using Aspose.Cells;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI_DataLoader.Common.Enitities;
using WebAPI_DataLoader.Response;

namespace WebAPI_DataLoader.Controllers
{
    public class DataLoaderController : ApiController
    {

        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files != null && httpRequest.Files.Count > 0)
            {

                FileInfo fileInfo = null;

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (!string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string uid = HttpContext.Current.Request.Form["uid"];
                        string filePath = Path.GetTempPath() + uid + postedFile.FileName;
                        fileInfo = new FileInfo(postedFile.FileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        postedFile.SaveAs(filePath);
                    }
                }
            }
            else
            {
                return BadRequest();

            }
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult RemoveFile()
        {
            string uid = HttpContext.Current.Request.Form["uid"];
            string fileName = HttpContext.Current.Request.Form["fileNames"];
            string filePath = Path.GetTempPath() + uid + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Ok();
        }

        [HttpPost]
        public FileOption ProcessFiles([FromBody] List<UploadedFile> uploadedFiles)
        {
            FileOption fileOption = new FileOption();
            foreach(UploadedFile uploadedFile in uploadedFiles) {
                uploadedFile.FilePath = Path.GetTempPath() + uploadedFile.Uid + uploadedFile.FileName;
            }
            fileOption.UploadedFiles = uploadedFiles;
            return fileOption;
        }

        [HttpPost]
        public List<Entity> ImportFile([FromBody] FileOption fileOption)
        {
            Converter converter = new Converter();
            return converter.GenerateJson(fileOption);
        }

        [HttpPost]
        public List<Entity> SheetSelection([FromBody] List<Entity> entities)
        {
            List<Entity> entityList = new List<Entity>();
            foreach (Entity entity in entities)
            {
                List<Sheet> sheets = entity.sheets;
                entity.sheets = new List<Sheet>();
                foreach (Sheet sheet in sheets)
                {
                    if (sheet.selected)
                    {
                        entity.sheets.Add(sheet);
                    }
                }
                if (entity.sheets.Count > 0)
                {
                    entityList.Add(entity);
                }
            }

            return entityList;
        }

        [HttpPost]
        public object GenerateJson([FromBody] List<Entity> entities)
        {
            EntitiesResponse entitiesResponse = CommonUtility.GetSchemaJson(entities);
            DataEntitiesResponse dataEntitiesResponse = CommonUtility.GetDataJson(entities);
            return new { schema = entitiesResponse, data = dataEntitiesResponse };
        }
    }
}
