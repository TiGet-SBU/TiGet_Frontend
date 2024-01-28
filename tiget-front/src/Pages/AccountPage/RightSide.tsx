import React from 'react';
import { Customer, Role, Company } from '../../FakeData/fakeData';
import { useContext } from 'react';
import { UserContext } from '../../Components/UserProvider/UserProvider';
import { useNavigate } from 'react-router-dom';

export const RightSide: React.FC<{ account: Customer | Company | null; onClick: (newPage: number) => void; }> = ({ account, onClick }) => {
  
  const navigate = useNavigate();
  const {logout,type} = useContext(UserContext);
  const handleLogout = () => {
    navigate("/");
    logout();
  }
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
            {(account as Customer).firstName + " " + (account as Customer).lastName}
          </div>
          <div className='holder'>
            <div className='ph'>
              {account.phoneNumber}
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
          {type === Role.User ? 
              <div className='state-button' onClick={handleClick(1)}>
                افزایش اعتبار
              </div>
            :
              <div></div>
          }
          {type === Role.Company ?           
          <div className='state-button' onClick={handleClick(2)}>
            افزودن بلیت
          </div>:
          <div></div>
          }
          <div className='exit' onClick={handleLogout}>
            خروج از حساب
          </div>
        </div>
      </div>
    }
  </>;
};
