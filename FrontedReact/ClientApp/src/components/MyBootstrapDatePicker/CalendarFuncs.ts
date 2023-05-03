function daysInMonth(month, year) {
  return new Date(year, month, 0).getDate();
}

const GetMatrixDays = (pMonth, pYear) => {
  const Month = pMonth;
  const Year = pYear;
  const FirstMonthDay = new Date(Year, Month - 1, 1, 0, 0, 0, 0);
  let FirstweekDay = FirstMonthDay.getDay();
  if (FirstweekDay == 0) FirstweekDay = 7;
  const DaysinMonth = daysInMonth(Month, Year);
  let weeks = [];
  //console.log(pDate,Month,Year,FirstMonthDay,FirstweekDay,DaysinMonth);
  let Day = 1;
  let MatrixCell = 1;
  for (let j = 0; Day <= DaysinMonth; j++) {
    let weekdays = [];
    for (let i = 0; i < 7; i++) {
      if (MatrixCell >= FirstweekDay && Day <= DaysinMonth) {
        const tmpDate = new Date(Year, Month - 1, Day, 0, 0, 0, 0);
        weekdays[i] = { CalendarDay: Day, CalendarDate: tmpDate };
        Day++;
      } else weekdays[i] = { CalendarDay: undefined, CalendarDate: undefined };
      MatrixCell++;
    }
    weeks[j] = weekdays;
  }
  return weeks;
};

const yyyymmdd = (pDate) => {
  var mm = pDate.getMonth() + 1; // getMonth() is zero-based
  var dd = pDate.getDate();
  return [
    pDate.getFullYear(),
    (mm > 9 ? "" : "0") + mm,
    (dd > 9 ? "" : "0") + dd
  ].join("-");
};

export { GetMatrixDays, yyyymmdd };
