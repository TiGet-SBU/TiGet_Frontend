import React from 'react';
import Navbar from '../../Components/Navbar/Navbar';
import { sina } from '../../FakeData/fakeData';
import './AccountPage.css';
import { UserDataCard } from './UserDataCard';
import { UserBalanceCard } from './UserBalanceCard';
import { RightSide } from './RightSide';
const AccountPage = () => {
  //page = 0 => user data
  //page = 1 => increase money
  const [page,setPage] = React.useState(0);
  const handlePageChange = (newPage : number) => {
    setPage(newPage);
  }
  return (
    <div className='account-holder'>
      <Navbar/>
      <div className='main-flex'>
        <RightSide account={sina} onClick={handlePageChange}/>
        <div className='left-flex'>
          {
            page === 0 ? (<UserDataCard account={sina}/>) : (<UserBalanceCard account={sina}/>) 
          }
        </div>
      </div>
    </div>
  )
}
export default AccountPage
