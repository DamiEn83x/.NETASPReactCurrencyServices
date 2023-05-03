import React from "react";
import storeCreator from "./store";
import {
  WalutaChanged,
  walutyRefChanged,
  DateRangeChanged,
  SelectetAvaibleCurrencies,
  SelectetRefCurrencies,
  SelectetCurrency,
  SelectedDateFrom,
  SelectedDateTo
} from "./reducers/Filter/FilterReducers";
import {
  WalutyKursy,
  fetchWaluty,
  fetchWalutyKursy,
  CurrencyItemsAllChecks,
  CurrencyItemsAllLookup,
  ResetState
} from "./reducers/MainReducer";
import {
  EnableMockFetch,
  DisableMockFetch
} from "./services/fetchmodulewraper";
import { Provider } from "react-redux";
import WalutyPanel from "./CurrencyPowerPanel";
import { render } from "@testing-library/react";
import { CURR_SERVICE_API } from "./services/walutyexternal.tsx";

describe("Test Reducers,actions nad states in ReduxStore", () => {
  let store = {};
  beforeEach(() => {
    store = storeCreator();
  });
  afterEach(() => {
    store = null;
  });
  it("Test correct states", () => {
    let WalutyRef = ["USD", "EUR", "GBP", "THB"];

    store.dispatch(walutyRefChanged(WalutyRef));

    store.dispatch(WalutaChanged({ code: "PLN", table: "A" }));
    store.dispatch(
      DateRangeChanged({ DateFrom: "2020-01-01", DateTo: "2020-06-01" })
    );

    // grab current state
    const state = store.getState();

    expect([...SelectetRefCurrencies(state)].sort()).toEqual(
      ["USD", "EUR", "GBP", "THB"].sort()
    );
    expect(SelectetCurrency(state)).toEqual({ code: "PLN", table: "A" });

    expect(SelectedDateFrom(state)).toEqual("2020-01-01");

    expect(SelectedDateTo(state)).toEqual("2020-06-01");
  });

  it("test load proper  ref currencies", (done) => {
    const MockedFetchFuncion = jest.fn().mockReturnValue(
      new Promise((resolve, reject) => {
        resolve(
          new Response(
            JSON.stringify([
              {
                table: "A",
                no: "193/A/NBP/2020",
                effectiveDate: "2020-10-02",
                rates: [
                  { currency: "bat (Tajlandia)", code: "THB", mid: 0.1213 },
                  { currency: "dolar amerykański", code: "USD", mid: 3.8366 },
                  { currency: "dolar australijski", code: "AUD", mid: 2.7425 }
                ]
              },
              {
                table: "B",
                no: "039/B/NBP/2020",
                effectiveDate: "2020-09-30",
                rates: [
                  {
                    currency: "afgani (Afganistan)",
                    code: "AFN",
                    mid: 0.050061
                  },
                  {
                    currency: "ariary (Madagaskar)",
                    code: "MGA",
                    mid: 0.000991
                  },
                  { currency: "balboa (Panama)", code: "PAB", mid: 3.8658 }
                ]
              }
            ])
          )
        );
      })
    );
    EnableMockFetch(MockedFetchFuncion);
    store.dispatch(fetchWaluty());
    DisableMockFetch();

    expect(MockedFetchFuncion.mock.calls.length).toBe(1);

    setTimeout(() => {
      try {
        const state = store.getState();
        expect(CurrencyItemsAllChecks(state)).toEqual([
          { code: "PLN", name: "Polski złoty", table: "A" },
          { code: "THB", name: "bat (Tajlandia)", table: "A" },
          { code: "USD", name: "dolar amerykański", table: "A" },
          { code: "AUD", name: "dolar australijski", table: "A" }
        ]);
        expect(CurrencyItemsAllLookup(state)).toEqual([
          { code: "PLN", name: "Polski złoty", table: "A" },
          { code: "THB", name: "bat (Tajlandia)", table: "A" },
          { code: "USD", name: "dolar amerykański", table: "A" },
          { code: "AUD", name: "dolar australijski", table: "A" },
          { code: "AFN", name: "afgani (Afganistan)", table: "B" },
          { code: "MGA", name: "ariary (Madagaskar)", table: "B" },
          { code: "PAB", name: "balboa (Panama)", table: "B" }
        ]);
        done();
      } catch (error) {
        done(error);
      }
    }, 300);
  });

  it("test request to  api  ref currencies on  component startup", (done) => {
    const MockedFetchFuncion = jest
      .fn()
      .mockReturnValueOnce(
        new Promise((resolve, reject) => {
          resolve(new Response(JSON.stringify({ msg: "Node is starting" })));
        })
      )
      .mockReturnValueOnce(
        new Promise((resolve, reject) => {
          resolve(new Response(JSON.stringify({ msg: "Node is working" })));
        })
      )
      .mockReturnValueOnce(
        new Promise((resolve, reject) => {
          resolve(
            new Response(
              JSON.stringify([
                {
                  table: "A",
                  no: "193/A/NBP/2020",
                  effectiveDate: "2020-10-02",
                  rates: [
                    { currency: "bat (Tajlandia)", code: "THB", mid: 0.1213 },
                    { currency: "dolar amerykański", code: "USD", mid: 3.8366 },
                    { currency: "dolar australijski", code: "AUD", mid: 2.7425 }
                  ]
                },
                {
                  table: "B",
                  no: "039/B/NBP/2020",
                  effectiveDate: "2020-09-30",
                  rates: [
                    {
                      currency: "afgani (Afganistan)",
                      code: "AFN",
                      mid: 0.050061
                    },
                    {
                      currency: "ariary (Madagaskar)",
                      code: "MGA",
                      mid: 0.000991
                    },
                    { currency: "balboa (Panama)", code: "PAB", mid: 3.8658 }
                  ]
                }
              ])
            )
          );
        })
      );

    store.dispatch(ResetState());
    EnableMockFetch(MockedFetchFuncion);

    render(
      <Provider store={store}>
        {" "}
        <WalutyPanel />
      </Provider>
    );

    setTimeout(() => {
      try {
        expect(MockedFetchFuncion.mock.calls.length).toBe(3);
        expect(MockedFetchFuncion.mock.calls[0][0]).toEqual(CURR_SERVICE_API);
        expect(MockedFetchFuncion.mock.calls[1][0]).toEqual(CURR_SERVICE_API);
        expect(MockedFetchFuncion.mock.calls[2][0]).toEqual(
          CURR_SERVICE_API + "/?query=GettabelaWalutAB"
        );

        const state = store.getState();
        expect(CurrencyItemsAllChecks(state)).toEqual([
          { code: "PLN", name: "Polski złoty", table: "A" },
          { code: "THB", name: "bat (Tajlandia)", table: "A" },
          { code: "USD", name: "dolar amerykański", table: "A" },
          { code: "AUD", name: "dolar australijski", table: "A" }
        ]);
        expect(CurrencyItemsAllLookup(state)).toEqual([
          { code: "PLN", name: "Polski złoty", table: "A" },
          { code: "THB", name: "bat (Tajlandia)", table: "A" },
          { code: "USD", name: "dolar amerykański", table: "A" },
          { code: "AUD", name: "dolar australijski", table: "A" },
          { code: "AFN", name: "afgani (Afganistan)", table: "B" },
          { code: "MGA", name: "ariary (Madagaskar)", table: "B" },
          { code: "PAB", name: "balboa (Panama)", table: "B" }
        ]);
        done();
      } catch (error) {
        done(error);
      }
    }, 4000);
  });

  it("test states to api parameters", (done) => {
    const MockedFetchFuncion = jest.fn().mockReturnValue(
      new Promise((resolve, reject) => {
        resolve(
          new Response(
            JSON.stringify([
              {
                date: "2020-04-07",
                CenaIlosciBazowej: 35,
                Wskaznik: 1
              },
              {
                date: "2020-04-08",
                CenaIlosciBazowej: 34.98840081521338,
                Wskaznik: 1.000331515145487
              }
            ])
          )
        );
      })
    );
    EnableMockFetch(MockedFetchFuncion);
    store.dispatch(
      fetchWalutyKursy({
        currency: "PLN",
        DateFrom: "2020-01-01",
        DateTo: "2020-06-01",
        WalutyRef: ["USD", "EUR", "GBP", "THB"],
        Token: 343
      })
    );
    DisableMockFetch();
    expect(MockedFetchFuncion.mock.calls[0][0]).toEqual(CURR_SERVICE_API);
    expect(JSON.parse(MockedFetchFuncion.mock.calls[0][1].body).Query).toEqual(
      "GetCurrencyPowerChanges"
    );
    expect(
      JSON.parse(MockedFetchFuncion.mock.calls[0][1].body).DayFrom
    ).toEqual("2020-01-01");
    expect(JSON.parse(MockedFetchFuncion.mock.calls[0][1].body).DayTo).toEqual(
      "2020-06-01"
    );
    expect(
      JSON.parse(
        JSON.parse(MockedFetchFuncion.mock.calls[0][1].body).tabelaWalut
      )
    ).toEqual(["USD", "EUR", "GBP", "THB"]);

    setTimeout(() => {
      try {
        const state = store.getState();
        expect(WalutyKursy(state)).toEqual({
          error: "",
          status: "succeeded",
          Token: 343,
          progress: 100,
          walutyKursy: {
            "2020-04-07": { Wskaznik: 1, date: "2020-04-07" },
            "2020-04-08": { Wskaznik: 1.000331515145487, date: "2020-04-08" }
          }
        });
        done();
      } catch (error) {
        done(error);
      }
    }, 300);
  });
  it("test  fetch exception", (done) => {
    const MockedFetchFuncion = jest.fn().mockReturnValue(
      new Promise((resolve, reject) => {
        throw "Cannot fetch example exeption";
      })
    );
    EnableMockFetch(MockedFetchFuncion);
    store.dispatch(
      fetchWalutyKursy({
        currency: "PLN",
        DateFrom: "2020-01-01",
        DateTo: "2020-06-01",
        WalutyRef: ["USD", "EUR", "GBP", "THB"],
        Token: 343
      })
    );
    DisableMockFetch();
    expect(MockedFetchFuncion.mock.calls[0][0]).toEqual(CURR_SERVICE_API);

    setTimeout(() => {
      try {
        const state = store.getState();
        expect(WalutyKursy(state)).toEqual({
          error:
            "Pobieranie kursow walut: Fetch error:Cannot fetch example exeption",
          status: "failed",
          Token: 343,
          progress: 0,
          walutyKursy: {}
        });
        done();
      } catch (error) {
        done(error);
      }
    }, 300);
  });
});
