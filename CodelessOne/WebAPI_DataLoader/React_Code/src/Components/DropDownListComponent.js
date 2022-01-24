import React from "react";
import { DropDownList } from "@progress/kendo-react-dropdowns";

export default function DropDownListComponent({ value, fieldName, onDataTypeChange }) {
    const [currentValue, setCurrentValue] = React.useState(value);
    const types = ["Text", "Number", "Dropdown", "Date Time", "Email", "Link", "Rich Content", "True / False", "Long Text", "Tags"];
    const onChange = (e) => {
        onDataTypeChange(fieldName, e.target.value);
        setCurrentValue(e.target.value)
    }
    return (
        <DropDownList data={types} value={currentValue} style={{ width: "100px" }}
            columnName={fieldName} onChange={onChange} />
    );
}
