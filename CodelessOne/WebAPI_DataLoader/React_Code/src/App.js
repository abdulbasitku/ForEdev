import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import { Routes } from "./routes";
import "@progress/kendo-theme-material/dist/all.css";

function App() {
  return (
    <div className="Home" id="appMainDivId">
      <div>
        <Router>
          <Routes />
        </Router>
      </div>
    </div>
  );
  
}

export default App;
