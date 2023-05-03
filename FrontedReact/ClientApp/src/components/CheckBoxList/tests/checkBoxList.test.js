import React from "react";

import CheckBoxList from "../CheckBoxList";
import { render, fireEvent } from "@testing-library/react";

describe("CheckBoxList component tests", () => {
  it("renders without crashing", () => {
    const renderer = render(<CheckBoxList />);
  });

  it("renders correct items", () => {
    const items = [
      { code: "USD", name: "Dolar amerykański" },
      { code: "PLN", name: "Polski złoty" }
    ];
    const { getByText, debug } = render(<CheckBoxList items={items} />);
    //debug();
    expect(getByText(/USD/i).textContent).toBe("USD - Dolar amerykański");
    expect(getByText(/PLN/i).textContent).toBe("PLN - Polski złoty");
  });

  it("click items changes state", (done) => {
    const items = [
      { code: "USD", name: "Dolar amerykański" },
      { code: "PLN", name: "Polski złoty" }
    ];
    let Ready = false;
    const OnChecksChanged = (ChecksList) => {
      //done=true;
      if (Ready) {
        try {
          const walutyRef = Object.values(ChecksList)
            .filter((item) => {
              return item.checked == 1;
            })
            .map((item) => {
              return item.code;
            });

          expect(walutyRef).toEqual(["USD"]);

          done();
        } catch (error) {
          done(error);
        }
      }
    };
    const { getByText } = render(
      <CheckBoxList items={items} ChecksChanged={OnChecksChanged} />
    );
    Ready = true;
    fireEvent.click(getByText(/PLN/i));
  });

  it("Unselect all", (done) => {
    const items = [
      { code: "USD", name: "Dolar amerykański" },
      { code: "PLN", name: "Polski złoty" }
    ];
    let Ready = false;
    const OnChecksChanged = (ChecksList) => {
      //done=true;
      if (Ready) {
        try {
          const walutyRef = Object.values(ChecksList)
            .filter((item) => {
              return item.checked == 1;
            })
            .map((item) => {
              return item.code;
            });

          expect(walutyRef).toEqual([]);

          done();
        } catch (error) {
          done(error);
        }
      }
    };
    const { getByText } = render(
      <CheckBoxList items={items} ChecksChanged={OnChecksChanged} />
    );
    Ready = true;
    fireEvent.click(getByText(/Select all/i));
  });
  it("Select all", (done) => {
    const items = [
      { code: "USD", name: "Dolar amerykański" },
      { code: "PLN", name: "Polski złoty" }
    ];
    let Ready = false;
    const OnChecksChanged = (ChecksList) => {
      //done=true;
      if (Ready) {
        try {
          const walutyRef = Object.values(ChecksList)
            .filter((item) => {
              return item.checked == 1;
            })
            .map((item) => {
              return item.code;
            });

          expect(walutyRef.sort()).toEqual(["PLN", "USD"].sort());

          done();
        } catch (error) {
          done(error);
        }
      }
    };
    const { getByText } = render(
      <CheckBoxList items={items} ChecksChanged={OnChecksChanged} />
    );
    fireEvent.click(getByText(/USD/i));
    fireEvent.click(getByText(/PLN/i));
    Ready = true;
    fireEvent.click(getByText(/Select all/i));
  });
});
