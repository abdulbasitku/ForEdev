import React from "react";
import { Link } from "react-router-dom";

import "./App.css";

const NavBar = () => {
  return (
    <div className="navBar">
      <div >
        <h1>
          Codele<span className="first">ss</span>One
        </h1>
      </div>
      
      {/* <ul style={{ display: "flow", flexDirection: "column" }}>
        <li>
          <Link to="/Home">Home</Link>
        </li>
        <li>
          <Link to="/Sheets">Grid</Link>
        </li>
        <li>
          <Link to="/Sample">Sample</Link>
        </li>
      </ul> */}
      {/* <hr /> */}
    </div>
  );
};
export default NavBar;
