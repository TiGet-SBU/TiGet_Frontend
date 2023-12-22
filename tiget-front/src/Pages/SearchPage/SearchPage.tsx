import React from 'react';
import './SearchPage.css';
import Navbar from '../../Components/Navbar/Navbar';
import SearchForm from '../../Components/SearchForm/SearchForm';
import FilterBar from '../../Components/Filter-Bar/Filter-Bar';

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
        <div className='filter-bar-holder-sp'>
          <FilterBar/>
        </div>
      </div>
    </>
  )
}

export default SearchPage