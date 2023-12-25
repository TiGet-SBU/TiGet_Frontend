import React, { useState } from 'react';
import Navbar from '../../Components/Navbar/Navbar';
import { Account, sina } from '../../FakeData/fakeData';
import Button from '../../Components/Button/Button';
import './AccountPage.css';
const UserDataCard : React.FC<{account : Account}> = ({account}) => 
{
  const [name,setName] = useState(account.first_name);
  const [lastName,setLastName] = useState(account.last_name);
  const [email,setEmail] = useState(account.email);
  const [phoneNumber,setPhoneNumber] = useState(account.phone_number);
  const handleNameChange = (e : React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value);
  }
  const handleLastNameChange = (e : React.ChangeEvent<HTMLInputElement>) => {
    setLastName(e.target.value);
  }
  const handleEmailChange = (e : React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  }
  const handlePhoneNumberChange = (e : React.ChangeEvent<HTMLInputElement>) => {
    setPhoneNumber(e.target.value);
  }
  return <div className='data-card'>
      <form className='data-card-form'>
        <div className='data-card-holder'>
          <div>نام </div>
          <input className='data-card-input' value={name} onChange={handleNameChange}/>
        </div>
        <div className='data-card-holder'>
          <div>نام خانوادگی </div>
          <input className='data-card-input' value={lastName} onChange={handleLastNameChange}/>
        </div>
        <div className='data-card-holder'>
          <div>ایمیل</div>
          <input className='data-card-input' value={email} onChange={handleEmailChange}/>
        </div>
        <div className='data-card-holder'>
          <div>شماره همراه</div>
          <input className='data-card-input' value={phoneNumber} onChange={handlePhoneNumberChange}/>
        </div>
      </form>
      <Button text='اعمال تغییرات' onClick={()=>true}/>

  </div>
}
const UserBalanceCard = () => {
  return <div>

  </div>
}
const RightSide : React.FC<{account : Account}> = ({account}) => {
  return <div className='right-side'>
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
      <div className='state-button'>
        اطلاعات حساب کاربری
      </div>
      <div className='state-button'>
        افزایش اعتبار
      </div>
      <div className='exit'>
        خروج از حساب
      </div>
    </div>
  </div>
}
const AccountPage = () => {
  //page = 0 => user data
  //page = 1 => increase money
  const [page,setPage] = React.useState(0);
  function handlePageChange(newPage : number) {
    setPage(newPage);
  }
  return (
    <div className='account-holder'>
      <Navbar/>
      <div className='main-flex'>
        
        <RightSide account={sina}/>
        <div className='left-flex'>
          {
            page === 0 ? (<UserDataCard account={sina}/>) : (<UserBalanceCard/>) 
          }
        </div>
      </div>
    </div>
  )
}
export default AccountPage
