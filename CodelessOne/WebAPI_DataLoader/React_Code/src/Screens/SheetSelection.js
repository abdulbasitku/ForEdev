/// <reference path="../routes.js" />
import React, { useState } from "react";
import {
    Grid,
    GridColumn as Column
} from "@progress/kendo-react-grid";

import {
    Switch
} from "@progress/kendo-react-inputs";

import "@progress/kendo-theme-material/dist/all.css";

export default function SheetResponse(props) {
    let importData = props.location.importData;
    let sheetSelection = []
    let count = 0;
    for (let i = 0; i < importData.length; i++) {
        for (let j = 0; j < importData[i].sheets.length; j++) {
            let sheet = importData[i].sheets[j];
            sheetSelection.push({ fileName: importData[i].fileName, sheetName: sheet.sheetName, dataIndex : count, selected: true, sheetIndex : j, fileIndex : i  });
            count++;
        }
    }
    const [gridData, setGridData] = useState(sheetSelection);
    const onChange = (e) => {
        let newData = [];
        for (let j = 0; j < sheetSelection.length; j++) {
            newData.push({
                fileName: sheetSelection[j].fileName, sheetName: sheetSelection[j].sheetName, dataIndex: sheetSelection[j].dataIndex,
                selected: sheetSelection[j].selected, sheetIndex: sheetSelection[j].sheetIndex, fileIndex: sheetSelection[j].fileIndex
            });
        }
        let dataIndex = e.target.props.dataIndex;
        newData[dataIndex].selected = !newData[dataIndex].selected;
        let fileIndex = e.target.props.fileIndex;
        let sheetIndex = e.target.props.sheetIndex;      
        importData[fileIndex].sheets[sheetIndex].selected = newData[dataIndex].selected;
        setGridData(newData);
    }
  
    const nextStepOnClick = (events) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(importData),
        };
        fetch('http://localhost:49730/api/DataLoader/SheetSelection/', requestOptions)
            .then(response => response.json())
            .then(data => {
                props.history.push({ pathname: "/Sheets/", importData: data });
            })
            .catch(error => {
                throw (error);
            })


        //history.push({ pathname: "/Response/", importData: response });
    }

    const CustomCell = (props) => {
        return (
            <td><Switch checked={gridData[props.dataItem.dataIndex].selected} onChange={onChange} dataIndex={props.dataItem.dataIndex} fileIndex={props.dataItem.fileIndex} sheetIndex={props.dataItem.sheetIndex} /></td>
        );
    };
    return (
        <div>
            <div style={styles.mainDiv}>
                <div className="k-grid-table" style={styles.gridTable}>
                    <Grid
                        style={{ height: "600px" }}
                        data={gridData}>
                        <Column field="fileName" title="File" />
                        <Column field="sheetName" title="Sheet" />
                        <Column field="selected" cell={CustomCell} />
                    </Grid>
                </div>
            </div>
            <div style={styles.mainDiv}>
                <button
                    className={"k-button"}
                    onClick={nextStepOnClick}>
                    NEXT
                </button>
            </div>
        </div>
    );
};


const styles = {
    mainDiv: {
        display: "flex",
        alignItems: "center",
        justifyContent: "center"
    },
    gridTable: {
        display: "flex",
        width: "70%",
        alignItems: "center",
        justifyContent: "center",
        alignSelf: "center",
        justifySelf: "center",
    },
};
