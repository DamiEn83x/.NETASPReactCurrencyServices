import React, { Component } from "react";
import {
  SelectetRefCurrencies,
  SelectetCurrency,
  SelectedDateFrom,
  SelectedDateTo
} from "./FilterReducers";
import { WalutyKursy } from "../MainReducer";
import { useSelector } from "react-redux";
const FilterDebuger = () => {
  //console.log(useSelector(SelectetRefCurrencies) );
  const Kursy = useSelector(WalutyKursy);
  return (
    <div className="card">
      <div className="card-header bg-primary text-white">Filter state view</div>
      <div className="card-body p-4">
        Currencyref: {useSelector(SelectetRefCurrencies).toString()} <br />
        Currency:{" "}
        {useSelector(SelectetCurrency).code +
          " " +
          useSelector(SelectetCurrency).table}{" "}
        <br />
        DateFrom: {useSelector(SelectedDateFrom)} <br />
        DateTo: {useSelector(SelectedDateTo)}
        <br />
        Fetch Kursy status :{Kursy.status}
        <br />
      </div>
    </div>
  );
};

export default FilterDebuger;
