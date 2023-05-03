import React, { Component } from "react";
import CheckBoxList from "../../../Components/CheckBoxList/CheckBoxList";
import {
  walutyRefChanged,
  SelectetAvaibleCurrencies,
  SelectetCurrency
} from "../reducers/Filter/FilterReducers";

import { WalutyTableFetchStatus } from "../reducers/MainReducer";
import { useDispatch, useSelector } from "react-redux";

const WalutyReferencyjne = ({ items }) => {
  const dispatch = useDispatch();
  const OnChecksChanged = (ChecksList) => {
    const walutyRef = Object.values(ChecksList)
      .filter((item) => {
        return item.checked == 1;
      })
      .map((item) => {
        return item.code;
      });
    dispatch(walutyRefChanged(walutyRef));
  };
  const SelectedCurrency = useSelector(SelectetCurrency);

  let FilterWauty = items.filter((item) => {
    return item.code != SelectedCurrency.code;
  });
  // console.log("SelectedCurrency", SelectedCurrency);
  //console.log("FilterWauty", FilterWauty)

  const FetchStatus = useSelector(WalutyTableFetchStatus);
  const content =
    FetchStatus == "idle" ? null : FetchStatus == "loading" ? (
      <div className="d-flex justify-content-center">
        <div
          className="spinner-border text-primary m-5"
          style={{ width: "6rem", height: "6rem" }}
          role="status"
        >
          <span className="sr-only">Loading...</span>
        </div>
      </div>
    ) : (
      <CheckBoxList
        items={FilterWauty}
        ChecksChanged={OnChecksChanged}
        Columns="3"
      />
    );
  return (
    <div className="card">
      <div className="card-header bg-primary text-white">
        Waluty referencyjne
      </div>
      <div className="card-body p-4">{content}</div>
    </div>
  );
};
export default WalutyReferencyjne;
