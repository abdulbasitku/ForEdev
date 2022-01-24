using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_DataLoader.Common.Enitities;
using WebAPI_DataLoader.Response;

namespace WebAPI_DataLoader
{
    public static class CommonUtility
    {
        public static EntitiesResponse GetSchemaJson(List<Entity> entities)
        {
            EntitiesResponse entityList = new EntitiesResponse();
            entityList.sheets = new List<EntityResponse>();
            foreach (Entity entity in entities)
            {
                foreach (Sheet sheet in entity.sheets)
                {
                    EntityResponse entityResponse = new EntityResponse();
                    entityResponse.SheetName = sheet.SheetName;
                    entityResponse.Attributes = new List<AttributeResponse>();
                    foreach (ColumnInfo columnInfo in sheet.ColumnInfos)
                    {
                        AttributeResponse attributeResponse = new AttributeResponse();
                        attributeResponse.AttributeName = columnInfo.ColumnName;
                        attributeResponse.IsMap = columnInfo.Enable;
                        attributeResponse.DataType = columnInfo.ColumnDataType;
                        if (attributeResponse.DataType == "Reference")
                        {
                            attributeResponse.LookUpAttribute = new LookUpAttributeResponse();
                            attributeResponse.LookUpAttribute.Entity = columnInfo.ReferenceColInfo.SheetName;
                            attributeResponse.LookUpAttribute.AttributeName = columnInfo.ReferenceColInfo.ColumnName;
                            attributeResponse.LookUpAttribute.Relationship = columnInfo.ReferenceColInfo.Relationship;
                        }
                        entityResponse.Attributes.Add(attributeResponse);
                    }
                    entityList.sheets.Add(entityResponse);
                }
            }
            return entityList;
        }

        public static DataEntitiesResponse GetDataJson(List<Entity> entities)
        {
            Dictionary<string, Dictionary<string, ColumnInfo>> referenceColumns = new Dictionary<string, Dictionary<string, ColumnInfo>>();
            foreach (Entity entity in entities)
            {
                foreach (Sheet sheet in entity.sheets)
                {
                    Dictionary<string, ColumnInfo> references = new Dictionary<string, ColumnInfo>();
                    foreach (ColumnInfo columnInfo in sheet.ColumnInfos)
                    {
                        if (columnInfo.ColumnDataType == "Reference")
                        {
                            references.Add(columnInfo.ColumnName, columnInfo);
                        }
                    }
                    referenceColumns.Add(sheet.SheetName, references);
                }
            }
            DataEntitiesResponse entityList = new DataEntitiesResponse();
            entityList.sheets = new List<DataEntityResponse>();
            foreach (Entity entity in entities)
            {
                foreach (Sheet sheet in entity.sheets)
                {
                    DataEntityResponse dataEntityResponse = new DataEntityResponse();
                    dataEntityResponse.Name = sheet.SheetName;
                    dataEntityResponse.Records = new List<DataRecords>();
                    Dictionary<string, ColumnInfo> referenceCol = referenceColumns[sheet.SheetName];
                    foreach (Dictionary<string, object> record in sheet.records)
                    {
                        DataRecords dataRecords = new DataRecords();
                        List<CellData> data = new List<CellData>();
                        Dictionary<string, DataReference> dataReferenceDict = new Dictionary<string, DataReference>();
                        foreach (KeyValuePair<string, object> cellData in record) {
                            if (referenceCol.ContainsKey(cellData.Key))
                            {
                                DataReference dataReference;
                                if (!dataReferenceDict.ContainsKey(cellData.Key)) {
                                    dataReference = new DataReference();
                                    dataReference.ReferenceRecords = new List<CellData>();
                                    dataReferenceDict.Add(cellData.Key, dataReference);
                                }
                                else {
                                    dataReference = dataReferenceDict[cellData.Key];
                                }
                                ColumnInfo columnInfo = referenceCol[cellData.Key];
                                dataReference.SubEntity = columnInfo.ReferenceColInfo.SheetName;
                                dataReference.RelationName = columnInfo.ReferenceColInfo.ColumnName;
                                dataReference.ReferenceRecords.Add(new CellData(columnInfo.ColumnName, cellData.Value));

                            }
                            else {
                                data.Add(new CellData(cellData.Key, cellData.Value));
                            }
                        }
                        dataRecords.Data = data;
                        if (dataReferenceDict.Count > 0) {
                            dataRecords.DataReferences = new List<DataReference>();
                            foreach (DataReference dataReference in dataReferenceDict.Values)
                            {
                                dataRecords.DataReferences.Add(dataReference);
                            }
                        }
                        dataEntityResponse.Records.Add(dataRecords);
                    }
                    entityList.sheets.Add(dataEntityResponse);
                }
            }
            return entityList;
        }
    }
}