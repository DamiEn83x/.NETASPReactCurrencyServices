import React from "react";

import LookupSelector from "../LookupSelector";
import { render, fireEvent } from "@testing-library/react";

describe("LookupSelector component tests", () => {
  it("renders without crashing", () => {
    const renderer = render(<LookupSelector />);
  });

  it("select  item", (done) => {
    const items = [
      { code: "USD", name: "Dolar amerykański" },
      { code: "PLN", name: "Polski złoty" },
      { code: "HKD", name: "dolar Hongkongu" },
      { code: "CAD", name: "dolar kanadyjski" }
    ];
    const OnChangeSelect = (code) => {
      try {
        expect(code).toEqual("HKD");

        done();
      } catch (error) {
        done(error);
      }
    };
    const { getByTestId } = render(
      <LookupSelector Items={items} pOnChangeSelect={OnChangeSelect} />
    );
    fireEvent.change(getByTestId("select"), {
      target: { value: "HKD" }
    });
  });
});
