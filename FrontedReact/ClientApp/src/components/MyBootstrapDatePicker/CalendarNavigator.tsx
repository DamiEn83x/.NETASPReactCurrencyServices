import React, { Component, useState, useEffect } from "react";

const monthNames = [
  "Styczeń",
  "Luty",
  "Marzec",
  "Kwieceń",
  "Maj",
  "Czerwiec",
  "Lipiec",
  "Sierpień",
  "Wrzesień",
  "Październik",
  "Listopad",
  "Grudzień"
];

const CalendarNavigator = ({ StartMonth, StartYear, ChangedMonthCallback }) => {
  const [Month, SetMonth] = useState(StartMonth);
  const [Year, SetYear] = useState(StartYear);
  const PrevMonth = () => {
    let NewMonth = Month - 1;
    let NewYear = Year;
    if (NewMonth < 1) {
      NewMonth = 12;
      NewYear = NewYear - 1;
    }
    SetMonth(NewMonth);
    SetYear(NewYear);
    if (ChangedMonthCallback != undefined)
      ChangedMonthCallback(NewMonth, NewYear);
  };

  const NextMonth = () => {
    // console.log('Month',Month,'Year',Year);
    let NewMonth = Month + 1;
    let NewYear = Year;
    if (NewMonth > 12) {
      NewMonth = 1;
      NewYear = NewYear + 1;
    }
    //console.log('newMonth',NewMonth,'NewYear',NewYear);
    SetMonth(NewMonth);
    SetYear(NewYear);
    if (ChangedMonthCallback != undefined)
      ChangedMonthCallback(NewMonth, NewYear);
  };
  return (
    <div>
      <div className=".container-fluid">
        <div className="row">
          <div className="col text-left">
            <button
              data-testid="PrevButton"
              type="button"
              className="btn btn-primary btn-sm "
              onClick={PrevMonth}
            >
              {"<<"}
            </button>
          </div>
          <div className="col">
            {monthNames[Month - 1]}&nbsp;{Year}
          </div>
          <div className="col text-right">
            <button
              type="button"
              data-testid="NextButton"
              className="btn btn-primary btn-sm"
              onClick={NextMonth}
            >
              {">>"}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CalendarNavigator;
