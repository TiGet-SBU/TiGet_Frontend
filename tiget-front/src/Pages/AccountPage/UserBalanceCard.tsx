import React, { useState } from 'react';
import { Account } from '../../FakeData/fakeData';
import Button from '../../Components/Button/Button';

export const UserBalanceCard:React.FC<{account : Account}> = ({account}) => {
  const [balance,setBalance] = useState(account.balance);
  const [result,setResult] = useState(account.balance);

  const handleBalanceChange = (e : React.ChangeEvent<HTMLInputElement>) =>{
    setBalance(parseInt(e.target.value));
  } 
  const handleButtonClick = () =>{
    setResult((prevResult) => prevResult + balance)
  }
  return <div className='user-balance-card'>
    <div className='balance'>
      <div>
        موجودی فعلی 
      </div>
      <div>
        {result}
      </div>
      <input className="balance-input" placeholder={"مقدار افزایش"} onChange={handleBalanceChange}/>
      <div className='balance-button'>
        <Button text='افزودن اعتبار' onClick={handleButtonClick}/>
      </div>
    </div>

  </div>;
};
