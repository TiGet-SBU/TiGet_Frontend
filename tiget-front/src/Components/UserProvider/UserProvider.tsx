import React, { createContext, useState, useContext } from 'react';
import { Account, MyComponentProps } from '../../FakeData/fakeData';


interface UserContextType {
  isLoggedIn: boolean;
  userData: Account | null;
  login: (user: Account) => void;
  logout: () => void;
}

export const UserContext = createContext<UserContextType>({
  isLoggedIn: false,
  userData: null,
  login: () => {},
  logout: () => {},
});

const UserProvider: React.FC<MyComponentProps> = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [userData, setUserData] = useState<Account | null>(null);

  const login = (user: Account) => {
    setIsLoggedIn(true);
    setUserData(user);
  };

  const logout = () => {
    setIsLoggedIn(false);
    setUserData(null);
  };

  return (
    <UserContext.Provider value={{ isLoggedIn, userData, login, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;