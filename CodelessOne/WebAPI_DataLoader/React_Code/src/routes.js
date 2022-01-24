
import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import Sheets from './Screens/Sheets';
import SheetResponse from './Screens/SheetResponse';
import SheetSelection from './Screens/SheetSelection';
import FileOptions from './Screens/FileOptions';
import Home from './Screens/Home';
import NavBar from './NavBar';

export  const Routes = () => {
  return (
    <div>
      <NavBar />
      <Switch>
        <Route exact path="/Home" component={Home} />
        <Route exact path="/">
          <Redirect to="/Home" />
              </Route>
        <Route exact path="/FileOptions" component={FileOptions} />
        <Route exact path="/Sheets" component={Sheets} />
        <Route exact path="/SheetResponse" component={SheetResponse} />
        <Route exact path="/SheetSelection" component={SheetSelection} />
      </Switch>
    </div>
  );
};