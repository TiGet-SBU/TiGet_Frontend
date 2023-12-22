import React from 'react';
import './DescriptionCard.css';
import { Preview } from '../../FakeData/fakeData';
import Button from '../Button/Button';
export const DescriptionCard : React.FC<{ preview: Preview}> = ({preview: preview}) => {
  return (
    <div className='card-holder'>
      <div className='card-top'>
        <div className='img-holder'>
          <img src={preview.image} alt="عکس مقصد" width="100%" height="100%" />
        </div>
        <div className='name-detail-holder'>
          <div className='name'>
            {preview.name}
          </div>
          <div className='description'>
            {preview.description}
          </div>
        </div>
      </div>
      <div className='ticket-show'>
        <Button text='نمایش بلیت ها' onClick={()=>true}/>
      </div>
    </div>
  )
}
