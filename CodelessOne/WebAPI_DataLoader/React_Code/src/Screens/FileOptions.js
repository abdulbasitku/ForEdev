/// <reference path="../routes.js" />
import React, { useState } from "react";
import { NumericTextBox, RadioButton } from "@progress/kendo-react-inputs";
import { PanelBar, PanelBarItem } from "@progress/kendo-react-layout"
import "@progress/kendo-theme-material/dist/all.css";

export default function FileOptions(props) {
    const [option, setOption] = useState({
        sepratedOption: "comma",
        headerRow: 1,
        dataRow: 2
    });
    const nextStepOnClick = (events) => {
        let fileOption = { uploadedFiles: props.location.fileOption.uploadedFiles,  sepratedOption: option.sepratedOption, headerRow: option.headerRow, dataRow: option.dataRow }
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(fileOption),
        };
        fetch('http://localhost:49730/api/DataLoader/ImportFile/', requestOptions)
            .then(response => response.json())
            .then(data => {
                props.history.push({ pathname: "/sheetSelection/", importData: data });
            })
            .catch(error => {
                throw (error);
            })
    };

    const changeHeaderRow = (e) => {
        let value = e.value !== null ? e.value : 1;
        setOption({
            ...option,
            ["headerRow"]: value
        });
    };

    const changeDataRow = (e) => {
        let value =  e.value !== null ?  e.value : 1;
        setOption({
            ...option,
            ["dataRow"]: value
        });
    };

    const handleDelimiterChange = React.useCallback(
        (e) => {
            setOption({
                ...option,
                ["sepratedOption"]: e.value
            });
        },
        [setOption]
    );

    return (
        <div>
            <div >
                <div> <label>Header Row {" "}
                    <NumericTextBox
                        onChange={changeHeaderRow}
                        value={option.headerRow} />
                    
                 </label>
                </div>
            </div>
            <div>
                <div> <label>Data Row {" "}
                    <NumericTextBox
                        onChange={changeDataRow}
                        value={option.dataRow} />

                </label>
                </div>
            </div>
            <div >
                <div><PanelBar>
                    <PanelBarItem expanded={false} title="Advance Option">
                        <p>Fields in the file seprated by :</p>
                        <RadioButton
                            name="group1"
                            value="comma"
                            checked={option.sepratedOption === "comma"}
                            onChange={handleDelimiterChange}
                            label="Comma(,)"
                        />
                        <RadioButton
                            name="group1"
                            value="semicolon"
                            checked={option.sepratedOption === "semicolon"}
                            label="Semicolon(:)"
                            onChange={handleDelimiterChange}
                        />
                        <RadioButton
                            name="group1"
                            value="pipe"
                            checked={option.sepratedOption === "pipe"}
                            label="Pipe(|)"
                            onChange={handleDelimiterChange}
                        />
                        <RadioButton
                            name="group1"
                            value="space"
                            checked={option.sepratedOption === "space"}
                            label="Space( )"
                            onChange={handleDelimiterChange}
                        />
                        <RadioButton
                            name="group1"
                            value="tab"
                            checked={option.sepratedOption === "tab"}
                            label="Tab"
                            onChange={handleDelimiterChange}
                        />
                    </PanelBarItem>
                </PanelBar>
                </div>
            </div>
            <div style={styles.mainDiv}>
                <div style={{
                        margin: 10,
                        display: "flex",
                        justifyContent: "center",
                        justifySelf: "center",
                        alignSelf: "center",
                        alignItems: "center",
                    }}>

                    <button
                        className={"k-button"}
                        onClick={nextStepOnClick}
                    >
                        NEXT
                    </button>
                </div>
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
};

