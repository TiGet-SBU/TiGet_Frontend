import React from 'react';
import './SearchPage.css';
import Navbar from '../../Components/Navbar/Navbar';
import SearchForm from '../../Components/SearchForm/SearchForm';
import FilterBar from '../../Components/Filter-Bar/Filter-Bar';
import { fakeTickets } from '../../FakeData/fakeData';
import CreateTicket from '../../Components/Ticket/CreateTicket';

const SearchPage = () => {
  return (
    <>z
      <Navbar/>
      <div className='main-body-sp'>
        <div className='ticket-search-form-holder-sp'>
          <div className='search-form-holder-sp'>
            <SearchForm/>
          </div>
          <div className='ticket-holder-sp'>
            {
              fakeTickets.map(ft => <CreateTicket ticket={ft}/>)
            }
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