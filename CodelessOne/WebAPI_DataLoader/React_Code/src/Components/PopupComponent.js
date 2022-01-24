import React from "react";
import { Popup } from '@progress/kendo-react-popup';
import { RadioButton } from "@progress/kendo-react-inputs";
import { DropDownList } from "@progress/kendo-react-dropdowns";

export default function PopupComponent({ anchor, show, fieldName, fileIndex, sheetIndex, importData, onOK, onClose }) {
    const [sheets, setSheets] = React.useState([]);
    const [selectedSheet, setselectedSheet] = React.useState(null);
    const [fields, setFields] = React.useState([]);
    const [field, setField] = React.useState(null);
    const [selectedRelation, setSelectedRelation] = React.useState("one_to_one");
    const handleChange = React.useCallback(
        (e) => {
            setSelectedRelation(e.value);
        },
        [selectedRelation]
    );
    const onFileChange = (e) => {
        let value = e.target.value;
        for (let i = 0; i < importData.length; i++) {
            if (value.fileName == importData[i].fileName) {
                if (i != fileIndex) {
                    setSheets(importData[i].sheets);
                } else {
                    let sheetList = []
                    for (let j = 0; j < importData[i].sheets.length; j++) {
                        if (j != sheetIndex) {
                            sheetList.push(importData[i].sheets[j]);
                        }
                    }
                    setSheets(sheetList);
                }
            }
        }
    };
    const onSheetChange = (e) => {
        let value = e.target.value;
        setselectedSheet(value);
        for (let i = 0; i < sheets.length; i++) {
            if (value.sheetName == sheets[i].sheetName) {
                setFields(sheets[i].ColumnInfos);
            }
        }
    };
    const onFieldChange = (e) => {
        let value = e.target.value;
        setField(value);
    };
    const onDone = (e) => {
        onOK(selectedSheet, field, fieldName, selectedRelation);
    }
    return (
        <Popup anchor={anchor.current} show={show}
            anchorAlign={{
                horizontal: "center",
                vertical: "top",
            }}
        >
            <div>
                <div><p>Select Column for Reference Type</p></div>
                <div><p>Select File</p>
                    <DropDownList
                        data={importData}
                        textField="fileName"
                        dataItemKey="fileName"
                        onChange={onFileChange}
                    />
                </div>
                <div><p>Select Sheet</p>
                    <DropDownList
                        data={sheets}
                        value={selectedSheet}
                        textField="sheetName"
                        dataItemKey="sheetName"
                        onChange={onSheetChange}
                    />
                </div>
                <div><p>Select Field</p>
                    <DropDownList
                        data={fields}
                        value={field}
                        textField="columnName"
                        dataItemKey="columnName"
                        onChange={onFieldChange}
                    />
                </div>
                <div><p>Relationship</p>
                    <RadioButton
                        name="group1"
                        value="one_to_one"
                        checked={selectedRelation === "one_to_one"}
                        label="One to One"
                        onChange={handleChange}
                    />
                    <RadioButton
                        name="group1"
                        value="one_to_many"
                        checked={selectedRelation === "one_to_many"}
                        label="One to Many"
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <button onClick={onDone} >OK</button>
                    <button onClick={onClose} >Close</button>
                </div>
            </div>
        </Popup>
    );
}
