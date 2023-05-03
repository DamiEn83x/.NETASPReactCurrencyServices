import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import httpserviceWaluty from "../services/walutyexternal";
import LoadCurrenciesAvaible from "./Filter/FilterReducers";

import { useSelector } from "react-redux";

export const fetchWaluty = createAsyncThunk("Main/fetchWaluyty", () => {
  const service = new httpserviceWaluty();
  let response;
  response = service.GettabelaWalutAB();
  return response;
});

export const CheckNode = createAsyncThunk("Main/CheckNode", () => {
  const service = new httpserviceWaluty();
  let response;
  response = service.CheckAndWaitForNode();
  return response;
});

export const GetProgressfetchWaluty = createAsyncThunk(
  "Main/GetProgressfetchWaluty",
  ({ Token }) => {
    const service = new httpserviceWaluty();
    let response;
    response = service.GetProgressPowerChanges(Token);
    return response;
  }
);
export const fetchWalutyKursy = createAsyncThunk(
  "Main/fetchWalutyKursy",
  ({ currency, DateFrom, DateTo, WalutyRef, Token }) => {
    const service = new httpserviceWaluty();
    const response = service.GetCurrencyPowerChanges(
      currency.code,
      currency.table,
      DateFrom,
      DateTo,
      WalutyRef,
      Token
    );
    return response;
  }
);

const initialState = {
  stateWalutyAll: {
    waluty: { WalutyRefAll: [], WalutySelectAll: [] },
    status: "idle",
    error: ""
  },
  stateWalutyKursy: {
    walutyKursy: {},
    status: "idle",
    progress: 0,
    Token: 0,
    error: ""
  },
  stateNode: {
    State: "idle",
    Description: ""
  }
};
const PLN = { table: "A", code: "PLN", name: "Polski złoty" };
const MainSlice = createSlice({
  name: "Main",
  initialState,
  reducers: {
    ResetState(state, action) {
      state.stateWalutyAll.waluty = { WalutyRefAll: [], WalutySelectAll: [] };
      state.stateWalutyAll.status = "idle";
      state.stateWalutyAll.error = "";

      state.stateWalutyKursy.walutyKursy = { walutyKursy: [] };
      state.stateWalutyKursy.status = "idle";
      state.stateWalutyKursy.error = "";

      state.stateNode.State = "idle";
      state.stateNode.Description = "";
    }
  },
  extraReducers: {
    [fetchWaluty.pending]: (state, action) => {
      state.stateWalutyAll.status = "loading";
    },
    [fetchWaluty.fulfilled]: (state, action) => {
      state.stateWalutyAll.status = "succeeded";
      state.stateWalutyAll.waluty.WalutyRefAll = [].concat(
        PLN,
        action.payload.data.WalutyA
      );
      state.stateWalutyAll.waluty.WalutySelectAll = [].concat(
        PLN,
        action.payload.data.WalutyA,
        action.payload.data.WalutyB
      );
    },
    [fetchWaluty.rejected]: (state, action) => {
      state.stateWalutyAll.status = "failed";
      state.stateWalutyAll.error =
        "Pobieranie słownika walut: " + action.error.message;
    },
    [fetchWalutyKursy.pending]: (state, action) => {
      state.stateWalutyKursy.Token = action.meta.arg.Token;
      state.stateWalutyKursy.progress = 0;
      state.stateWalutyKursy.status = "loading";
      state.stateWalutyKursy.walutyKursy = {};
      state.stateWalutyKursy.error = "";
    },
    [fetchWalutyKursy.rejected]: (state, action) => {
      state.stateWalutyKursy.status = "failed";
      state.stateWalutyKursy.walutyKursy = {};
      state.stateWalutyKursy.error =
        "Pobieranie kursow walut: " + action.error.message;
    },
    [fetchWalutyKursy.fulfilled]: (state, action) => {
      state.stateWalutyKursy.status = "succeeded";
      state.stateWalutyKursy.progress = 100;
      state.stateWalutyKursy.walutyKursy = action.payload.data;
    },
    [GetProgressfetchWaluty.fulfilled]: (state, action) => {
      state.stateWalutyKursy.progress = action.payload.data;
    },
    [CheckNode.pending]: (state, action) => {
      state.stateNode.State = "loading";
    },
    [CheckNode.rejected]: (state, action) => {
      state.stateNode.State = "failed";
      state.stateNode.Description = action.error.message;
    },
    [CheckNode.fulfilled]: (state, action) => {
      state.stateNode.State = "ready";
    }
  }
});
export const SelectErrors = (state) => {
  let Errors = "";
  Errors = [];
  if (state.Main.stateWalutyAll.error != "")
    Errors.push(state.Main.stateWalutyAll.error);
  if (state.Main.stateWalutyKursy.error != "")
    Errors.push(state.Main.stateWalutyKursy.error);

  return Errors;
};
export const CurrencyItemsAllChecks = (state) => {
  return state.Main.stateWalutyAll.waluty.WalutyRefAll;
};
export const CurrencyItemsAllLookup = (state) => {
  return state.Main.stateWalutyAll.waluty.WalutySelectAll;
};

export const {} = MainSlice.actions;

export const WalutyKursy = (state) => {
  return state.Main.stateWalutyKursy;
};

export const WalutyTableFetchStatus = (state) => {
  return state.Main.stateWalutyAll.status;
};
export const NodeIsReadyState = (state) => {
  return state.Main.stateNode.State;
};
export const NodeIsReadyDescription = (state) => {
  return state.Main.stateNode.Description;
};

export const { ResetState } = MainSlice.actions;

export default MainSlice.reducer;
