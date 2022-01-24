import React from "react";

import {
    Grid,
    GridColumn as Column
} from "@progress/kendo-react-grid";
import PopupComponent from "../PopupComponent";
import SwitchComponent from "../SwitchComponent";
import DropDownListComponent from "../DropDownListComponent";
import {
    Input
} from "@progress/kendo-react-inputs";

export default function GridComponent({ importData, setImportData, fileIndex, sheetIndex }) {
    const anchor = React.useRef(null);
    const data = importData[fileIndex].sheets[sheetIndex].records;
    const columns = importData[fileIndex].sheets[sheetIndex].ColumnInfos;
    const [show, setShow] = React.useState(false);
    const [popUpField, setPopUpField] = React.useState(null);
    const GridHeader = (props) => {
        let index = getColumnIndex(props.field);
        return (
            <div className="k-link" >
                <span>{props.title}</span>
                <div><Input value={props.title} /></div>
                <div><DropDownListComponent  value={importData[fileIndex].sheets[sheetIndex].ColumnInfos[index].columnDataType}
                    fieldName={importData[fileIndex].sheets[sheetIndex].ColumnInfos[index].columnName}
                    onDataTypeChange={DataTypeChange} /></div>
                <div><SwitchComponent enable={importData[fileIndex].sheets[sheetIndex].ColumnInfos[index].enable} onToggle={onSwichChange}
                    fieldName={importData[fileIndex].sheets[sheetIndex].ColumnInfos[index].columnName}/></div>
            </div>
        );
    };

    const getColumnIndex = (columnName) => {
        for (let i = 0; i < columns.length; i++) {
            if (columns[i].columnName == columnName) {
                return i;
            }
        }
        return null;
    }

    const onSwichChange = (fieldName, enable) => {
        for (let i = 0; i < columns.length; i++) {
            if (columns[i].columnName == fieldName) {
                importData[fileIndex].sheets[sheetIndex].ColumnInfos[i].enable = enable;
            }
        }
        setImportData(importData);
    };

    const DataTypeChange = (columnName, dataType) => {
        if (dataType == "Reference") {
            setPopUpField(columnName);
            setShow(true);
        } else {
            for (let i = 0; i < columns.length; i++) {
                if (columns[i].columnName == columnName) {
                    importData[fileIndex].sheets[sheetIndex].ColumnInfos[i].columnDataType = dataType;
                }
            }
            setImportData(importData);
        }
    };

    const onPopupOk = (selectedSheet, refColumn, fieldName, relationship) => {
        for (let i = 0; i < columns.length; i++) {
            if (columns[i].columnName == fieldName) {
                importData[fileIndex].sheets[sheetIndex].ColumnInfos[i].columnDataType = "Reference";
                let referenceColInfo = { sheetName: selectedSheet.sheetName, columnName: refColumn.columnName, relationship: relationship };
                importData[fileIndex].sheets[sheetIndex].ColumnInfos[i].referenceColInfo = referenceColInfo;
            }
        }
        setImportData(importData);
        setShow(false)
        setPopUpField(null);
    };

    const onClose = (fieldName) => {
        setShow(false);
        setPopUpField(null);
    };

    return (
        <div className="k-grid-table" style={styles.gridTable}>
            <PopupComponent importData={importData} anchor={anchor} show={show} sheetIndex={sheetIndex} fileIndex={fileIndex} fieldName={popUpField} onOK={onPopupOk} onClose={onClose} />
            <div className="k-grid-table" ref={anchor}>
                <Grid
                    style={{ height: "600px" }}
                    data={data}>
                    {columns.map((column, i) => {
                        return (
                            <Column key={i} index="0" field={column.columnName} title={column.columnName}
                                headerCell={GridHeader} />
                        );
                    })}
                </Grid>
            </div >
        </div>
    );
}

const styles = {
    gridTable: {
        display: "flex",
        width: "70%",
        alignItems: "center",
        justifyContent: "center",
        alignSelf: "center",
        justifySelf: "center",
    },
};
