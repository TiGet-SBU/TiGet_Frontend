import React from 'react'
import Navbar from '../../Components/Navbar/Navbar';
import SearchForm from '../../Components/SearchForm/SearchForm';
import { DescriptionCard } from '../../Components/Description-card/DescriptionCard';
import { Ticket } from '../../FakeData/fakeData';
import { fakeTickets } from '../../FakeData/fakeData';
import './LandingPage.css';



const CreateTickets : React.FC<{ tickets: Ticket[] }> = ({tickets}) => {
  const cards = tickets.map( ticket => <DescriptionCard ticket={ticket}/>);
  return <>
    {cards}
  </>
};

const LandingPage = () => {
  return (
    <div>
      <Navbar/>
      <div className='Logo-Background'></div>
      <div className='SearchForm-Container'>
        <SearchForm/>
      </div>
      <div className='search-card-holder'>
        <CreateTickets tickets={fakeTickets}/>
      </div>
    </div>
  );
}

export default LandingPage