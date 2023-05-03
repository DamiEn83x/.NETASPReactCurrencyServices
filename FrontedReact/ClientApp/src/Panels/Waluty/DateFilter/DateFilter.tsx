import React, { useState } from "react";
import DatePicker from "../../../Components/MyBootstrapDatePicker/MyBootstrapDatePicker";
import {
  SelectedDateFrom,
  SelectedDateTo,
  DateRangeChanged
} from "../reducers/Filter/FilterReducers";
import { useDispatch, useSelector } from "react-redux";
const DateFilter = () => {
  const lstrDateFRom = useSelector(SelectedDateFrom);
  const lstrDateTo = useSelector(SelectedDateTo);

  const yyyymmdd = (pDate) => {
    var mm = pDate.getMonth() + 1; // getMonth() is zero-based
    var dd = pDate.getDate();
    return [
      pDate.getFullYear(),
      (mm > 9 ? "" : "0") + mm,
      (dd > 9 ? "" : "0") + dd
    ].join("-");
  };

  const [Dates, setDates] = useState({
    DateFrom: lstrDateFRom,
    DateTo: lstrDateTo
  });

  const dispatch = useDispatch();
  const DateFromChange = (date) => {
    const tmpDate = yyyymmdd(date);
    setDates({ ...Dates, DateFrom: tmpDate });
    dispatch(DateRangeChanged({ ...Dates, DateFrom: tmpDate }));
  };

  const DateToChange = (date) => {
    const tmpDate = yyyymmdd(date);
    setDates({ ...Dates, DateTo: tmpDate });
    dispatch(DateRangeChanged({ ...Dates, DateTo: tmpDate }));
  };

  return (
    <div className="d-flex flex-row bd-highlight mb-3">
      <div className="p-0 bd-highlight">
        Data od:
        <br />
        <DatePicker
          pDate={new Date(Dates.DateFrom)}
          pCallbackChange={DateFromChange}
        />
      </div>
      <div className="p-2 bd-highlight"></div>
      <div className="p-0 bd-highlight">
        Data do:
        <br />
        <DatePicker
          pDate={new Date(Dates.DateTo)}
          pCallbackChange={DateToChange}
        />
      </div>
    </div>
  );
};

export default DateFilter;
