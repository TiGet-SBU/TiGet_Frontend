import React, { useState } from 'react';
import { Customer, Company } from '../../FakeData/fakeData';
import Button from '../../Components/Button/Button';

export const UserBalanceCard:React.FC<{account : Customer | Company | null}> = ({account}) => {
  const [balance,setBalance] = useState( account === null ?  0 : (account as Customer).balance);
  const [result,setResult] = useState(account === null ? 0 : (account as Customer).balance);

  const handleBalanceChange = (e : React.ChangeEvent<HTMLInputElement>) =>{
    setBalance(parseInt(e.target.value));
  } 
  const handleButtonClick = () =>{
    if (account !== null)
      (account as Customer).balance = result + balance;
    setResult((prevResult) => prevResult + balance)
  }
  return <>
    {account === null ? 
     <div></div> : 
    <div className='user-balance-card'>
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
    </div>}
  </>;
};
