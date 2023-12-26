import React, { createContext, useState, useContext } from 'react';
import { Account, MyComponentProps, userType } from '../../FakeData/fakeData';


interface UserContextType {
  isLoggedIn: boolean;
  type : userType;
  userData: Account | null;
  login: (user: Account, type: userType) => void;
  logout: () => void;
}

export const UserContext = createContext<UserContextType>({
  isLoggedIn: false,
  type: userType.user,
  userData: null,
  login: () => {},
  logout: () => {},
});

const UserProvider: React.FC<MyComponentProps> = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [userData, setUserData] = useState<Account | null>(null);
  const [type,setType] = useState(userType.user);

  const login = (user: Account, type : userType) => {
    setIsLoggedIn(true);
    setType(type);
    setUserData(user);
  };

  const logout = () => {
    setIsLoggedIn(false);
    setUserData(null);
  };

  return (
    <UserContext.Provider value={{ isLoggedIn, type ,userData, login, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;