import React from "react";
import "./BuyPage.css";
import PurchaseInformation from "../../Components/PurchaseInformation/PurchaseInformation";
import Navbar from "../../Components/Navbar/Navbar";
import CreateTicket from "../../Components/Ticket/CreateTicket";
import { Ticket } from "../../FakeData/fakeData";
import { fakeTickets } from "../../FakeData/fakeData";
const BuyPage = () => {
  return (
    <div>
      <Navbar />
      <CreateTicket ticket={fakeTickets[0]} />
      <PurchaseInformation />
    </div>
  );
};

export default BuyPage;
