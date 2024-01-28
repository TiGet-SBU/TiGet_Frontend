import React, { createContext, useState, useContext } from 'react';
import { User, MyComponentProps, Role, Customer, Company } from '../../FakeData/fakeData';


interface UserContextType {
  isLoggedIn: boolean;
  type : Role;
  userData: Customer | Company | null;
  updateUserData :  (user: Customer | Company | null) => void;
  login: (user: Customer | Company | null, type: Role) => void;
  logout: () => void;
}

export const UserContext = createContext<UserContextType>({
  isLoggedIn: false,
  type: Role.User,
  userData: null,
  updateUserData : ()=>{},
  login: () => {},
  logout: () => {},
});

const UserProvider: React.FC<MyComponentProps> = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [userData, setUserData] = useState<Customer | Company | null>(null);
  const [type,setType] = useState(Role.User);

  const updateUserData = (user: Customer | Company | null) => {
    setUserData(user);
  }
  const login = (user: Customer | Company | null, type : Role) => {
    setIsLoggedIn(true);
    setType(type);
    setUserData(user);
  };

  const logout = () => {
    setIsLoggedIn(false);
    setUserData(null);
  };

  return (
    <UserContext.Provider value={{ isLoggedIn, type ,userData, updateUserData, login, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;