import React from 'react';
import { Ticket } from '../../FakeData/fakeData';
import './Ticket.css';
import Button from '../Button/Button';
const CreateTicket : React.FC<{ ticket: Ticket}> = ({ticket}) =>{
  return (
    <div className='main-holder'>
      <div className='src-dst-holder'>
        <div className='src'>
          {ticket.src}
        </div>
        <div className='dst'>
          {ticket.dst}
        </div>
        <div className='buy'>
          <Button text='خرید' onClick={()=>true}/>
        </div>
      </div>
      <div className='price'>
        {ticket.price}
      </div>
    </div>
  )
}

export default CreateTicket