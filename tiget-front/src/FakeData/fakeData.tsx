export type Preview = {
    name: string,
    Vehicle: string, 
    description : string,
    image : any, 
   };
export type Ticket = {
   src : string,
   dst : string,
   time : Date,
   vehicle : string,
   price : number
}
const img = require("../Resources/destination.jpg");
export const fakePreview: Preview[] = [
   {name: "تهران", Vehicle : "اتوبوس", description : "پایتخت ایران",image : img },
   {name: "تبریز", Vehicle : "قطار", description : "شهر دل انگیز",image : img },
   {name: "شیراز", Vehicle : "هواپیما", description : "شهر شعر و ادب",image : img},
   {name: "اصفهان", Vehicle : "قطار", description : "نصف جهان",image : img}
 ];
 export const fakeTickets : Ticket[] = [
   {src: "تهران" , dst:"دبی",time: new Date("2018-8-9"), vehicle:"هواپیما", price:1785556687},
   {src: "تهران" , dst:"استانبول",time: new Date("2013-2-2"), vehicle:"اتوبوس", price:9978835457},
   {src: "تبریز" , dst:"تهران",time: new Date("2017-4-12"), vehicle:"قطار", price:1231548796},
   {src: "تهران" , dst:"علی آباد",time: new Date("2012-8-9"), vehicle:"هواپیما", price:1785978978},
 ]
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