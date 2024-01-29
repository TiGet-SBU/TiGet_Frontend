import React from "react";
import { Customer, Company, VehicleType } from "../../FakeData/fakeData";
import Button from "../../Components/Button/Button";
import axios from "axios";
import { Station } from "../../FakeData/fakeData";

const CompanyTicketAdd: React.FC<{ account: Customer | Company | null }> = ({
  account,
}) => {
  const [data, setData] = React.useState<Station[]>([]);
  const [cost, setCost] = React.useState(0);
  const [date, setDate] = React.useState(new Date("2/2/2022"));
  const [src, setSrc] = React.useState("");
  const [dst, setDst] = React.useState("");
  const [vehicle, setVehicle] = React.useState(0);

  const getDests = () => {
    axios
      .get("http://localhost:5120/api/company/stations", {
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
      })
      .then((res) => setData(res.data))
      .catch();
  };
  const handleCreate = () => {
    axios.put("http://localhost:5120/api/company/addTicket", {
      headers: { Authorization: `Bearer ${localStorage.getItem("token")}`},
      timeToGo: date,
      price: cost,
      vehicleId: vehicle,
      companyId: "0eceee0f-d3de-468a-8ab8-218142e874b1",
      sourceId: src,
      destinationId:dst    
    })
    .then(()=>console.log("yep"))
    .catch((e)=>console.log(e));
  }
  const cities = data.map((city) => (
    <option value={city.id}>{city.name}</option>
  ));
  return (
    <>
      {account === null ? (
        <div></div>
      ) : (
        <div className="add-card-form-holder">
          <form className="ticket-info-form">
            <select onChange={(e) => setSrc(e.target.value)} onClick={getDests}>
              {cities}
            </select>
            <select onChange={(e) => setDst(e.target.value)} onClick={getDests}>
              {cities}
            </select>
            <input
              onChange={(e) => setCost(parseInt(e.target.value))}
              placeholder="هزینه"
            />
            <input
              onChange={(e) => setDate(new Date(e.target.value))}
              placeholder="زمان"
            />
            <select onChange={(e)=>setVehicle(parseInt(e.target.value))} name="states">
              <option value={0}>اتوبوس</option>
              <option value={1}>قطار</option>
              <option value={2}>هواپیما</option>
            </select>
          </form>
          <div className="ticket-add">
            <Button text="ساخت بلیت" onClick={() => handleCreate} />
          </div>
        </div>
      )}
    </>
  );
};

export default CompanyTicketAdd;
