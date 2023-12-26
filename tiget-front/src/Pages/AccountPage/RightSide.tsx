import React from 'react';
import { Account, userType } from '../../FakeData/fakeData';
import { useContext } from 'react';
import { UserContext } from '../../Components/UserProvider/UserProvider';
import { useNavigate } from 'react-router-dom';

export const RightSide: React.FC<{ account: Account | null; onClick: (newPage: number) => void; }> = ({ account, onClick }) => {
  
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
          {type === userType.company ?           
          <div className='state-button' onClick={handleClick(1)}>
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
