import { createSlice } from "@reduxjs/toolkit";

const yyyymmdd = (pDate) => {
  var mm = pDate.getMonth() + 1; // getMonth() is zero-based
  var dd = pDate.getDate();
  return [
    pDate.getFullYear(),
    (mm > 9 ? "" : "0") + mm,
    (dd > 9 ? "" : "0") + dd
  ].join("-");
};
function addDays(date, days) {
  var result = new Date(date);
  result.setDate(result.getDate() + days);
  return result;
}
const initialState = {
  DateFrom: yyyymmdd(addDays(new Date(), -180)),
  DateTo: yyyymmdd(new Date()),
  Currency: { code: "PLN", table: "A" },
  CurrenciesREf: []
};

const FilterSlice = createSlice({
  name: "Filter",
  initialState,
  reducers: {
    walutyRefChanged(state, action) {
      state.CurrenciesREf = action.payload;
    },
    WalutaChanged(state, action) {
      state.Currency = action.payload;
    },
    DateRangeChanged(state, action) {
      state.DateFrom = action.payload.DateFrom;
      state.DateTo = action.payload.DateTo;
    }
  }
});
export const SelectetAvaibleCurrencies = (state) =>
  state.Filter.CurrenciesAvaible;
export const SelectetRefCurrencies = (state) => state.Filter.CurrenciesREf;
export const SelectetCurrency = (state) => state.Filter.Currency;
export const SelectedDateFrom = (state) => state.Filter.DateFrom;
export const SelectedDateTo = (state) => state.Filter.DateTo;

export const {
  walutyRefChanged,
  WalutaChanged,
  DateRangeChanged
} = FilterSlice.actions;

export default FilterSlice.reducer;
