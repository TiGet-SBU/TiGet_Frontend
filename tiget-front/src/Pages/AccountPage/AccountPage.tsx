import React from 'react';
import Navbar from '../../Components/Navbar/Navbar';
import { Account, sina } from '../../FakeData/fakeData';
import './AccountPage.css';
const UserDataCard = () => {
  return <>
  </>
}
const UserBalanceCard = () => {
  return <>
  </>
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
            page === 0 ? (<UserDataCard/>) : (<UserBalanceCard/>) 
          }
        </div>
      </div>
    </div>
  )
}
export default AccountPage