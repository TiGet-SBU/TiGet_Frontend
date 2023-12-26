import React, {useContext} from 'react';
import Navbar from '../../Components/Navbar/Navbar';
import { sina } from '../../FakeData/fakeData';
import './AccountPage.css';
import { UserDataCard } from './UserDataCard';
import { UserBalanceCard } from './UserBalanceCard';
import { RightSide } from './RightSide';
import { UserContext } from '../../Components/UserProvider/UserProvider';
import Button from '../../Components/Button/Button';
import CompanyTicketAdd from './CompanyTicketAdd';
const AccountPage = () => {
  //page = 0 => user data
  //page = 1 => increase money
  const [page,setPage] = React.useState(0);
  const handlePageChange = (newPage : number) => {
    setPage(newPage);
  }
  const { userData } = useContext(UserContext);

  return (
    <div className='account-holder'>
      <Navbar/>
      <div className='main-flex'>
        <RightSide account={userData} onClick={handlePageChange}/>
        <div className='left-flex'>
          {
            page === 0 ? (<UserDataCard account={userData}/>) :
            page === 1 ? (<UserBalanceCard account={userData}/>) :
            (<CompanyTicketAdd account={userData}/>) 
          }
        </div>
      </div>
    </div>
  )
}
export default AccountPage
