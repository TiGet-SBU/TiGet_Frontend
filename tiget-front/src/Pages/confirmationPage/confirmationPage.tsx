// ConfirmationPage.tsx
import React from "react";
import { useLocation } from 'react-router-dom';

interface UserInfo {
  id: number;
  firstName: string;
  lastName: string;
}

const ConfirmationPage: React.FC = () => {
  const location = useLocation();
  const params = new URLSearchParams(location.search);

  // Retrieve information from URL parameters and decode it
  const userNames: UserInfo[] = Array.from(params.getAll('user')).map((value) => JSON.parse(decodeURIComponent(value)));

  return (
    <div>
      <h1>Confirmation Page</h1>
      <ul>
        {userNames.map((user, index) => (
          <li key={index}>
            {`User ${user.id + 1}: ${user.firstName} ${user.lastName}`}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ConfirmationPage;
