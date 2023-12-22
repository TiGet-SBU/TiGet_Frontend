import React from 'react';
import './SearchPage.css';
import Navbar from '../../Components/Navbar/Navbar';
import SearchForm from '../../Components/SearchForm/SearchForm';
import SpecificationBar from '../../Components/SpecificationBar/SpecificationBar';

const SearchPage = () => {
  return (
    <>
      <Navbar/>
      <div className='main-body-sp'>
        <div className='ticket-search-form-holder-sp'>
          <div className='search-form-holder-sp'>
            <SearchForm/>
          </div>
          <div className='ticket-holder-sp'>
            
          </div>
        </div>
        <div className='specification-bar-holder-sp'>
          <SpecificationBar/>
        </div>
      </div>
    </>
  )
}

export default SearchPage