export type Ticket = {
    name: string,
    Vehicle: string, 
    description : string,
    image : any, 
    price : number
   };
const img = require("../Resources/destination.jpg");
export const fakeTickets: Ticket[] = [
   {name: "تهران", Vehicle : "اتوبوس", description : "پایتخت ایران",image : img ,price : 1780000 },
   {name: "تبریز", Vehicle : "قطار", description : "شهر دل انگیز",image : img ,price : 98725532  },
   {name: "شیراز", Vehicle : "هواپیما", description : "شهر شعر و ادب",image : img ,price : 1764487  },
   {name: "اصفهان", Vehicle : "قطار", description : "نصف جهان",image : img ,price : 5542378965  }
 ];
 export const fakeAirLines: string[] = [
   "iran air",
   "mahan",
   "lufthansa",
   "Qatar Airlines",
   "United Airlines"
 ]
 export const types: string[] = [
   "مستقیم",
   "یک",
   "دو",
   "بیشتر از 2",
   "همه"
 ]