import React from 'react';
import LandingPage from './Pages/LandingPage/LandingPage';
import LoginPage from './Pages/LoginPage/LoginPage';
import SearchPage from './Pages/SearchPage/SearchPage';
import AccountPage from './Pages/AccountPage/AccountPage';
import BuyPage from './Pages/BuyPage/BuyPage';
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import Support from './Pages/Support/Support';
const App : React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<LandingPage/>}/>
        <Route path='/login' element={<LoginPage/>}/>
        <Route path='/search' element={<SearchPage/>}/>
        <Route path='/account' element={<AccountPage/>}/>
        <Route path='/buyPage' element={<BuyPage/>}/>
        <Route path='/support' element={<Support/>}/>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
