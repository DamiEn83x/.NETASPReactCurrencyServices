import React, { useEffect, useState } from "react";
import WalutyReferencyjne from "./WalutyReferencyjne/WalutyReferencyjne";
import LookupSelector from "../../Components/LookupSelector/LookupSelector";
import DateFilter from "./DateFilter/DateFilter";

import FilterDebuger from "./reducers/Filter/FilterDebuger";
import { WalutaChanged } from "./reducers/Filter/FilterReducers";
import { useDispatch, useSelector } from "react-redux";
import {
  fetchWaluty,
  CurrencyItemsAllLookup,
  CurrencyItemsAllChecks,
  fetchWalutyKursy,
  GetProgressfetchWaluty,
  WalutyKursy,
  NodeIsReadyState,
  CheckNode,
  NodeIsReadyDescription
} from "./reducers/MainReducer";
import ErrorViewer from "./ErrorViewer";
import KursyViewer from "./KursyViewer/KursyViewer";
import {
  SelectetRefCurrencies,
  SelectetCurrency,
  SelectedDateFrom,
  SelectedDateTo
} from "./reducers/Filter/FilterReducers";

import StateDebuger from "./StateDebuger";

const SearchButton = () => {
  const dispatch = useDispatch();
  const currency = useSelector(SelectetCurrency);
  const DateFrom = useSelector(SelectedDateFrom);
  const DateTo = useSelector(SelectedDateTo);
  const WalutyRef = useSelector(SelectetRefCurrencies);
  const WalutyFetchStatus = useSelector(WalutyKursy).status;
  const Progress = useSelector(WalutyKursy).progress;
  const Token = useSelector(WalutyKursy).Token;
  const [MonitorTrigger, SetMonitorTrigger] = useState(0);
  const MonitorProgress = (pToken) => {
    if (WalutyFetchStatus == "loading") {
    }
  };
  useEffect(() => {
    setTimeout(() => {
      if (WalutyFetchStatus == "loading")
        dispatch(GetProgressfetchWaluty({ Token: Token }));
      SetMonitorTrigger(MonitorTrigger + 1);
    }, 1000);
  }, [MonitorTrigger, WalutyFetchStatus]);

  const GetCurrencyData = () => {
    let lToken = Math.round(Math.random() * 10000000);
    dispatch(
      fetchWalutyKursy({ currency, DateFrom, DateTo, WalutyRef, Token: lToken })
    );
  };
  return (
    <button className="btn btn-primary" onClick={GetCurrencyData}>
      Pobierz dane
    </button>
  );
};

const WalutyLookup = ({ items }) => {
  const dispatch = useDispatch();
  const OnChangeWaluta = (waluta) => {
    const table = items.filter((item) => {
      return item.code == waluta;
    })[0].table;
    dispatch(WalutaChanged({ code: waluta, table: table }));
  };
  return <LookupSelector Items={items} pOnChangeSelect={OnChangeWaluta} />;
};

const WalutyPanel = () => {
  const dispatch = useDispatch();
  const cNodeisReady = useSelector(NodeIsReadyState);
  const FechtWalutyStatus = useSelector(
    (state) => state.Main.stateWalutyAll.status
  );
  const arrCurrencyItemsAllChecks = useSelector(CurrencyItemsAllChecks);
  const arrCurrencyItemsAllLookup = useSelector(CurrencyItemsAllLookup);
  const CNodeDescription = useSelector(NodeIsReadyDescription);

  // console.log(cNodeisReady, FechtWalutyStatus);
  useEffect(() => {
    if (cNodeisReady == "ready" && FechtWalutyStatus === "idle") {
      dispatch(fetchWaluty());
    } else if (cNodeisReady == "idle") {
      dispatch(CheckNode());
    }
  }, [FechtWalutyStatus, dispatch, cNodeisReady]);
  //console.log(cNodeisReady, dispatch, FechtWalutyStatus);
  if (cNodeisReady == "ready") {
    return (
      <div className="container">
        <br />
        <WalutyReferencyjne items={arrCurrencyItemsAllChecks} />
        <br />
        <WalutyLookup items={arrCurrencyItemsAllLookup} />
        <br />
        <DateFilter />
        <br />
        <SearchButton />
        <br />

        <br />
        <KursyViewer />
        <br />
        <ErrorViewer />
      </div>
    );
  } else {
    if (cNodeisReady != "failed") {
      return (
        <div className="container">
          <br />
          <div class="alert alert-primary" role="alert">
            Oczekiwanie na odpowiedź serwera (Może to potrwać nawet 30 sekund)
          </div>
          <div className="d-flex justify-content-center">
            <div
              className="spinner-border text-primary m-5"
              style={{ width: "6rem", height: "6rem" }}
              role="status"
            >
              <span className="sr-only">Loading...</span>
            </div>
          </div>
        </div>
      );
    }
    if (cNodeisReady === "failed") {
      return (
        <div className="container">
          <br />
          <div class="alert alert-warning" role="alert">
            Nie udało się połaczyć z Nodem : {CNodeDescription}
          </div>
        </div>
      );
    }
  }
};

export default WalutyPanel;
