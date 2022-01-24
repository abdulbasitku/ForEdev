/// <reference path="../routes.js" />
import JSONPretty from 'react-json-prettify';
import "@progress/kendo-theme-material/dist/all.css";

export default function SheetResponse(props) {
    return (
        <div>
            <div style={styles.mainDiv}>

                <h1>Schema</h1>

            </div>
            <div style={styles.mainDiv}>

                <p><JSONPretty json={props.location.response.schema}/></p>

            </div>
            <div style={styles.mainDiv}>

                <h1>Data</h1>

            </div>
            <div style={styles.mainDiv}>

                <p><JSONPretty json={props.location.response.data} /></p>

            </div>
        </div>
    );
};


const styles = {
    mainDiv: {
        display: "flex",
        
    },
    content: {
        display: "flex",
        width: "100%", 
    },
    jsonStyle: {
        display: "flex",
        backgroundColor: 'black',
        width:"100%"
    },
};
