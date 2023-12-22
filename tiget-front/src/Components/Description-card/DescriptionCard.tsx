import React from 'react';
import './DescriptionCard.css';
import { Ticket } from '../../FakeData/fakeData';
import Button from '../Button/Button';
export const DescriptionCard : React.FC<{ ticket: Ticket}> = ({ticket}) => {
  return (
    <div className='card-holder'>
      <div className='card-top'>
        <div className='img-holder'>
          <img src={ticket.image} alt="عکس مقصد" width="100%" height="100%" />
        </div>
        <div className='name-detail-holder'>
          <div className='name'>
            {ticket.name}
          </div>
          <div className='description'>
            {ticket.description}
          </div>
        </div>
      </div>
      <div className='price-vehicle-holder'>
        <Button text='نمایش بلیت ها' onClick={()=>true}/>
      </div>
    </div>
  )
}
