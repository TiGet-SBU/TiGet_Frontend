import React from 'react';
import { Account } from '../../FakeData/fakeData';

export const RightSide: React.FC<{ account: Account | null; onClick: (newPage: number) => void; }> = ({ account, onClick }) => {
  const handleClick = (page: number) => {
     return () => onClick(page);
  };
  return <>
    {account === null ? <div>
      </div> : 
      <div className='right-side'>
        <div className='right-side-content-holder'>
          <div className='holder'>
            <div className='image'>
            </div>
          </div>
          <div className='holder'>
            {account.first_name + " " + account.last_name}
          </div>
          <div className='holder'>
            <div className='ph'>
              {account.phone_number}
            </div>
          </div>
        </div>
        <div className='right-side-state-holder'>
          <div className='holder'>
            <div className='line'></div>
          </div>
          <div className='state-button' onClick={handleClick(0)}>
            اطلاعات حساب کاربری
          </div>
          <div className='state-button' onClick={handleClick(1)}>
            افزایش اعتبار
          </div>
          <div className='exit'>
            خروج از حساب
          </div>
        </div>
      </div>
    }
  </>;
};
