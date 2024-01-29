import React from 'react';
import './FilterBar.css';
import { fakeAirLines } from '../../FakeData/fakeData';
const CreateCheckbox : React.FC<{ items: string[] }> = ({items}) => {
  const ret = items.map( it => <div>
    <input type='checkbox' id={it}/>
    <label>{it}</label>
  </div>
  );
  return <>
    {ret}
  </>
};
const FilterBar = () => {
  return (
    <div>
      <form>
        <div className='input-name'>
          قیمت
        </div>
        <div className='price-input-range'>
          <input type="range" min="1" max="1000" />
        </div>
        <div className='line'></div>
        <div className='input-name'>
          زمان حرکت
        </div>
        <div className='time-input-range'>
          <input type="range" min="0" max="24" />
        </div>
        <div className='line'></div>
        <div className='input-name'>
          شرکت فروشنده
        </div>
        <div className='company-input-checkbox'>
          <CreateCheckbox items={fakeAirLines}/>
        </div>
      </form>
    </div>
  )
}

export default FilterBar