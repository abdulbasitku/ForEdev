/// <reference path="../routes.js" />
import React, { useState } from "react";
import GridComponent from "../Components/table/GridComponent";
import "@progress/kendo-theme-material/dist/all.css";

export default function Sheets(props) {
    const [importData, setImportData] = useState(props.location.importData);
    const [fileIndex, setFileIndex] = useState(0);
    const [sheetIndex, setSheetIndex] = useState(0);
    const UserButton = (e) => (
        <div className="container padded">
            <div className="row">
                <div className="col-6 offset-md-3">
                    <button
                        className={"k-button"}
                        style={{ margin: 10 }}
                        data-file={e.file}
                        data-sheet={e.sheet}
                        onClick={selectGridData}
                    >
                        {e.params.sheetName} <b>+</b>

                    </button>
                </div>
            </div>
        </div>
    );


    const selectGridData = (e) => {
        setFileIndex(parseInt(e.target.dataset.file));
        setSheetIndex(parseInt(e.target.dataset.sheet));
    }

    const renderUserButtons = () => {
        return importData.map((workBook, i) => (
            workBook.sheets.map((sheetData, j) => (
                <UserButton key={sheetData.sheetName} params={sheetData} file={i} sheet={j} />
            ))
        ));
    };

    const nextStepOnClick = (events) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(importData),
        };
        fetch('http://localhost:49730/api/DataLoader/GenerateJson/', requestOptions)
            .then(response => response.json())
            .then(data => {
                props.history.push({ pathname: "/SheetResponse/", response: data });
            })
            .catch(error => {
                throw (error);
            })
    }


    return (
        <div>
            <div style={styles.mainDiv}>
                <p> Sheet(s) </p>
                {renderUserButtons()}
            </div>
            <div style={styles.mainDiv}>
                {fileIndex >= 0 && sheetIndex >= 0 && <GridComponent importData={importData}
                    fileIndex={fileIndex} sheetIndex={sheetIndex} setImportData={setImportData} />}
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
}

const styles = {
    mainDiv: {
        display: "flex",
        alignItems: "center",
        justifyContent: "center"
    },
};

