import React from 'react';
import { Upload } from "@progress/kendo-react-upload";
import { useHistory } from "react-router";


export default function FileUpload() {
    const [files, setFiles] = React.useState([]);

    const onAdd = (event) => {
        console.log("onAdd: ", event.affectedFiles);
        setFiles(event.newState);
    };

    const onRemove = (event) => {
        console.log("onRemove: ", event.affectedFiles);
        setFiles(event.newState);
    };
    const onProgress = (event) => {
        setFiles(event.newState);
    };
    const onBeforeUpload = (event) => {
        event.additionalData.uid = event.files[0].uid;
    }; 
    const onBeforeRemove = (event) => {
        event.additionalData.uid = event.files[0].uid;
    }; 
    const onStatusChange = (event) => {
        setFiles(event.newState);
    };
    const history = useHistory();

    const nextStepOnClick = (events) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(files),
        };
        fetch('http://localhost:49730/api/DataLoader/ProcessFiles/', requestOptions)
            .then(response => response.json())
            .then(data => {
                history.push({ pathname: "/FileOptions/", fileOption: data });
            })
            .catch(error => {
                throw (error);
            })

    }

    return (
        <div >
            <p>
                <br />
                AllowedExtensions: [".xlsx", ".xls", ".csv"]
                <br />
            </p>
            <Upload
                batch={false}
                multiple={true}
                files={files}
                onAdd={onAdd}
                onRemove={onRemove}
                onProgress={onProgress}
                onStatusChange={onStatusChange}
                onBeforeUpload={onBeforeUpload}
                onBeforeRemove={onBeforeRemove}
                withCredentials={false}
                saveUrl={"http://localhost:49730/api/DataLoader/UploadFile"}
                removeUrl={"http://localhost:49730/api/DataLoader/RemoveFile"}
            />
            <div
                style={{
                    margin: 10,
                    display: "flex",
                    justifyContent: "center",
                    justifySelf: "center",
                    alignSelf: "center",
                    alignItems: "center",
                }}
            >
                <button
                    className={"k-button"}
                    onClick={nextStepOnClick}
                    disabled={files.length <= 0}
                >
                    NEXT
        </button>
            </div>
        </div>
    );
}