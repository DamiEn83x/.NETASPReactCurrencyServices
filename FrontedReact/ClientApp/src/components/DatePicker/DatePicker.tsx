import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
const CustomDatePicker = ({ pDate, pCallbackChange }) => {
  const handleChange = (date) => {
    SetDate(date);
    pCallbackChange(date);
  };

  const [lDate, SetDate] = useState(pDate == undefined ? new Date() : pDate);

  return (
    <DatePicker
      selected={lDate}
      onChange={handleChange}
      dateFormat="yyyy-MM-dd"
    />
  );
};

export default CustomDatePicker;
