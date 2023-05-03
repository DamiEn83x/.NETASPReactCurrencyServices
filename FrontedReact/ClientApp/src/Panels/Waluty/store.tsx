import { configureStore } from "@reduxjs/toolkit";
import FilterReducer from "./reducers/Filter/FilterReducers";
import MainReducer from "./reducers/MainReducer";

export default () =>
  configureStore({
    reducer: {
      Filter: FilterReducer,
      Main: MainReducer
    }
  });
