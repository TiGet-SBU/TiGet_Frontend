import React from 'react'
import Navbar from '../../Components/Navbar/Navbar';
import SearchForm from '../../Components/SearchForm/SearchForm';
import { DescriptionCard } from '../../Components/Description-card/DescriptionCard';
import { Preview } from '../../FakeData/fakeData';
import { fakePreview } from '../../FakeData/fakeData';
import './LandingPage.css';



const CreatePreview : React.FC<{ previews: Preview[] }> = ({previews: previews}) => {
  const cards = previews.map( preview => <DescriptionCard preview={preview}/>);
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
      <div className='search-card-tag'>
        مقاصد پرطرفدار
      </div>
      <div className='search-card-holder'>
        <CreatePreview previews={fakePreview}/>
      </div>
    </div>
  );
}

export default LandingPage