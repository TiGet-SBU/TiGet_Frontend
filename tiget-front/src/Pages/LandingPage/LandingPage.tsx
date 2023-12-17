import React from 'react'
import Navbar from '../../Components/Navbar/Navbar';
import SearchForm from '../../Components/SearchForm/SearchForm';
import './LandingPage.css';

const LandingPage = () => {
  return (
    <div>
      <Navbar/>
      <div className='Logo-Background'></div>
      <div className='SearchForm-Container'>
        <SearchForm/>
      </div>
    </div>
  );
}

export default LandingPage
