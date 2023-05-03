import React from "react";

import MyBootstrapDatePicker from "../MyBootstrapDatePicker";
import { render, fireEvent } from "@testing-library/react";
import { GetMatrixDays } from "../CalendarFuncs";
import CalendarNavigator from "../CalendarNavigator";
import CalendarGrid from "../CalendarGrid";

describe("MyBootstrapDatePicker component tests", () => {
  it("renders without crashing", () => {
    const renderer = render(<MyBootstrapDatePicker />);
  });

  it("renders and click  without crashing", () => {
    const { getByTestId } = render(<MyBootstrapDatePicker />);
    fireEvent.click(getByTestId("inputCalendar"));
  });
  it("Callback without change value", () => {
    const MockedCallback = jest.fn();
    render(<MyBootstrapDatePicker pCallbackChange={MockedCallback} />);
    expect(MockedCallback.mock.calls.length).toBe(0);

    render(
      <MyBootstrapDatePicker
        pDate="2020-01-01"
        pCallbackChange={MockedCallback}
      />
    );
    expect(MockedCallback.mock.calls.length).toBe(0);
  });
  it("text displayed with the default value", () => {
    const yyyymmdd = (pDate) => {
      var mm = pDate.getMonth() + 1; // getMonth() is zero-based
      var dd = pDate.getDate();
      return [
        pDate.getFullYear(),
        (mm > 9 ? "" : "0") + mm,
        (dd > 9 ? "" : "0") + dd
      ].join("-");
    };
    expect(yyyymmdd(new Date("2020-02-01"))).toBe("2020-02-01");

    const renderer = render(<MyBootstrapDatePicker pDate="2020-02-01" />);
    expect(renderer.getByTestId("inputCalendar").value).toBe("2020-02-01");
    fireEvent.click(renderer.getByTestId("inputCalendar"));
    expect(renderer.getByText("Luty 2020")).toBeTruthy();

    /* let CurrDate = new Date();
    renderer = render(<MyBootstrapDatePicker />);
    expect(renderer.getByTestId("inputCalendar").value).toBe(
      yyyymmdd(CurrDate)*/
  });

  it("text displayed without  default value", () => {
    const yyyymmdd = (pDate) => {
      var mm = pDate.getMonth() + 1; // getMonth() is zero-based
      var dd = pDate.getDate();
      return [
        pDate.getFullYear(),
        (mm > 9 ? "" : "0") + mm,
        (dd > 9 ? "" : "0") + dd
      ].join("-");
    };
    expect(yyyymmdd(new Date("2020-02-01"))).toBe("2020-02-01");

    let CurrDate = new Date();
    const renderer = render(<MyBootstrapDatePicker />);
    expect(renderer.getByTestId("inputCalendar").value).toBe(
      yyyymmdd(CurrDate)
    );
  });
  it("GetMatrixDays", () => {
    const DaysMatrix = [
      [undefined, undefined, undefined, 1, 2, 3, 4],
      [5, 6, 7, 8, 9, 10, 11],
      [12, 13, 14, 15, 16, 17, 18],
      [19, 20, 21, 22, 23, 24, 25],
      [26, 27, 28, 29, 30, 31, undefined]
    ];

    const retarray = GetMatrixDays(10, 2020).map((week) => {
      return week.map((day) => day.CalendarDay);
    });

    //console.log(retarray);
    expect(retarray).toEqual(DaysMatrix);

    const DaysMatrix2 = [
      [undefined, undefined, undefined, undefined, undefined, undefined, 1],
      [2, 3, 4, 5, 6, 7, 8],
      [9, 10, 11, 12, 13, 14, 15],
      [16, 17, 18, 19, 20, 21, 22],
      [23, 24, 25, 26, 27, 28, 29],
      [30, undefined, undefined, undefined, undefined, undefined, undefined]
    ];
    const retarray2 = GetMatrixDays(11, 2020).map((week) => {
      return week.map((day) => day.CalendarDay);
    });
    //console.log(GetMatrixDays(new Date("2020-11-10")));
    expect(retarray2).toEqual(DaysMatrix2);
  });
  it("Test Calendar Navigator", () => {
    const MockedCallback = jest.fn();
    const { getByText, getByTestId } = render(
      <CalendarNavigator
        StartMonth={1}
        StartYear={2020}
        ChangedMonthCallback={MockedCallback}
      />
    );
    expect(getByText("Styczeń 2020")).toBeTruthy();
    fireEvent.click(getByTestId("NextButton"));
    expect(getByText("Luty 2020")).toBeTruthy();
    expect(MockedCallback.mock.calls.length).toBe(1);
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    fireEvent.click(getByTestId("NextButton"));
    expect(getByText("Styczeń 2021")).toBeTruthy();
  });

  it("Test Calendar Grid", () => {
    const MockedCallback = jest.fn();
    const renderer = render(
      <CalendarGrid
        pDate={new Date("2020-02-01")}
        pMonth={2}
        pYear={2020}
        pCallBackChoose={MockedCallback}
      />
    );

    fireEvent.click(renderer.getByText("9"));
    expect(MockedCallback.mock.calls.length).toBe(1);
    expect(MockedCallback.mock.calls[0][0]).toEqual("2020-02-09");
    expect(renderer.getByText("1").className).toEqual(
      "btn btn-primary btn-sm btn-block"
    );
  });

  it("Callback with change value", () => {
    const MockedCallback = jest.fn();
    const { getByTestId, getByText } = render(
      <MyBootstrapDatePicker
        pDate="2020-01-01"
        pCallbackChange={MockedCallback}
      />
    );
    fireEvent.click(getByTestId("inputCalendar"));
    fireEvent.click(getByText("9"));
    expect(MockedCallback.mock.calls.length).toBe(1);
    expect(MockedCallback.mock.calls[0][0]).toEqual(new Date("2020-01-09"));
  });
});
