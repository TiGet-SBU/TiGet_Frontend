import React, { useState } from 'react';
import { Account } from '../../FakeData/fakeData';
import Button from '../../Components/Button/Button';

export const UserDataCard: React.FC<{ account: Account | null; }> = ({ account }) => {
  
  const [name, setName] = useState(account === null ? "" : account.first_name);
  const [lastName, setLastName] = useState(account === null ? "" : account.last_name);
  const [email, setEmail] = useState(account === null ? "" : account.email);
  const [phoneNumber, setPhoneNumber] = useState(account === null ? "" : account.phone_number);
  const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value);
  };
  const handleLastNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLastName(e.target.value);
  };
  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };
  const handlePhoneNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPhoneNumber(e.target.value);
  };
  
  return <> 
  {
    account === null ? <div></div> :
    <div className='data-card'>
      <form className='data-card-form'>
        <div className='data-card-holder'>
          <div>نام </div>
          <input className='data-card-input' value={name} onChange={handleNameChange} />
        </div>
        <div className='data-card-holder'>
          <div>نام خانوادگی </div>
          <input className='data-card-input' value={lastName} onChange={handleLastNameChange} />
        </div>
        <div className='data-card-holder'>
          <div>ایمیل</div>
          <input className='data-card-input' value={email} onChange={handleEmailChange} />
        </div>
        <div className='data-card-holder'>
          <div>شماره همراه</div>
          <input className='data-card-input' value={phoneNumber} onChange={handlePhoneNumberChange} style={{ direction: 'ltr' }} />
        </div>
      </form>
      <div className='account-acceptance-button'>
        <Button text='اعمال تغییرات' onClick={() => true} />
      </div>
    </div>}
  </>;
};
