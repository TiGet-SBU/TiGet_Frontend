import React from 'react';
import LandingPage from './Pages/LandingPage/LandingPage';
import LoginPage from './Pages/LoginPage/LoginPage';
import SearchPage from './Pages/SearchPage/SearchPage';
import AccountPage from './Pages/AccountPage/AccountPage';
import {BrowserRouter, Routes, Route} from 'react-router-dom';
const App : React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<LandingPage/>}/>
        <Route path='/login' element={<LoginPage/>}/>
        <Route path='/search' element={<SearchPage/>}/>
        <Route path='/account' element={<AccountPage/>}/>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
