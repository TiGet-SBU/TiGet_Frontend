import React from 'react';
import './DescriptionCard.css';
import { Ticket } from '../../FakeData/fakeData';
export const DescriptionCard : React.FC<{ ticket: Ticket}> = ({ticket}) => {
  return (
    <div className='card-holder'>
      {ticket.name}
    </div>
  )
}
