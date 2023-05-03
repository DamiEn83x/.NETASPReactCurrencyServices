import React, { Component, useState, useEffect } from "react";
import { GetMatrixDays, yyyymmdd } from "./CalendarFuncs";
const weekDays = ["Pn", "Wt", "Śr", "Cz", "Pt", "So", "N"];
const CalendarGrid = ({ pDate, pMonth, pYear, pCallBackChoose }) => {
  const TruncatedDate = new Date(
    pDate.getYear() + 1900,
    pDate.getMonth(),
    pDate.getDate(),
    0,
    0,
    0,
    0
  );
  const CurrDate = new Date();
  const TruncatedCurrDate = new Date(
    CurrDate.getYear() + 1900,
    CurrDate.getMonth(),
    CurrDate.getDate(),
    0,
    0,
    0,
    0
  );
  const DaysMatrix = GetMatrixDays(pMonth, pYear);
  const header = (
    <tr>
      {weekDays.map((day) => {
        return <td>{day}</td>;
      })}
    </tr>
  );
  const GridClick = (event) => {
    //console.log('GridClick',event.target.value)
    pCallBackChoose(event.target.id);
  };
  const content = (
    <table>
      {header}
      {DaysMatrix.map((week) => {
        const content = week.map((day) => {
          //if (day.CalendarDay != undefined)
          //console.log(day.CalendarDate.toString(),TruncatedDate.toString());
          return (
            <td>
              {day.CalendarDay != undefined ? (
                day.CalendarDate.getTime() == TruncatedDate.getTime() ? (
                  <button
                    onClick={GridClick}
                    type="button"
                    className="btn btn-primary btn-sm btn-block"
                    id={yyyymmdd(day.CalendarDate)}
                    Date={day.CalendarDay}
                  >
                    {" "}
                    {day.CalendarDay}
                  </button>
                ) : day.CalendarDate.getTime() ==
                  TruncatedCurrDate.getTime() ? (
                  <button
                    onClick={GridClick}
                    type="button"
                    className="btn btn-secondary btn-sm btn-block"
                    id={yyyymmdd(day.CalendarDate)}
                    Date={day.CalendarDay}
                  >
                    {" "}
                    {day.CalendarDay}
                  </button>
                ) : (
                  <button
                    onClick={GridClick}
                    type="button"
                    className="btn btn-light btn-sm btn-block"
                    id={yyyymmdd(day.CalendarDate)}
                  >
                    {" "}
                    {day.CalendarDay}
                  </button>
                )
              ) : (
                ""
              )}
            </td>
          );
        });
        return <tr>{content}</tr>;
      })}
    </table>
  );

  //console.log('content',content)
  return <div>{content}</div>;
};

export default CalendarGrid;
