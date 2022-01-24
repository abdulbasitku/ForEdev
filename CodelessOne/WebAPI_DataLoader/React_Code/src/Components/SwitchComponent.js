import React from "react";
import {
    Switch
} from "@progress/kendo-react-inputs";

export default function SwitchComponent({ enable, fieldName, onToggle }) {
    const [checked, setChecked] = React.useState(enable);
    const onChange = (e) => {
        onToggle(fieldName, !checked);
        setChecked(!checked)
    }
    return (
        <Switch checked={checked} onChange={onChange}/>
    );
}
