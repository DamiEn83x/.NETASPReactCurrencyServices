import React, { Component, useState, useEffect } from "react";

function chunkArray(myArray, chunk_size) {
  var index = 0;
  var arrayLength = myArray.length;
  var tempArray = [];

  for (index = 0; index < arrayLength; index += chunk_size) {
    const myChunk = myArray.slice(index, index + chunk_size);
    // Do something if you want with the group
    tempArray.push(myChunk);
  }

  return tempArray;
}

const CheckBoxList = ({ items, ChecksChanged, Columns }) => {
  const [Checks, setChecks] = useState([]);

  const SelectAllChange = (event) => {
    //Checks.forEach((item)=>{}));
    CheckAll(event.target.checked);
  };

  const SelectChange = (code) => {
    let tChecks = JSON.parse(JSON.stringify(Checks));
    tChecks[code].checked = tChecks[code].checked == 1 ? 0 : 1;
    setChecks(tChecks);
    ChecksChanged(tChecks);
  };
  useEffect(() => {
    const tChecks = items.reduce(function (accum, currentVal) {
      accum[currentVal.code] = { ...currentVal, checked: 1 };
      return accum;
    }, {});

    setChecks(tChecks);
    ChecksChanged(tChecks);
  }, [items]);

  const CheckAll = (checked) => {
    let tChecks = JSON.parse(JSON.stringify(Checks));
    items.forEach(function (item, index) {
      tChecks[item.code].checked = checked ? 1 : 0;
    });
    setChecks(tChecks);
    ChecksChanged(tChecks);
  };

  const DividedArray = chunkArray(items, Math.round(items.length / Columns));

  const content = (
    <div className="row">
      {DividedArray.map((subArray) => {
        return (
          <div className="col">
            {subArray.map((item) => {
              return (
                <div
                  className="custom-control custom-checkbox mb-3"
                  id={item.code + "div"}
                >
                  <input
                    className="custom-control-input"
                    type="checkbox"
                    id={item.code + "input"}
                    name={item.code + "name"}
                    checked={
                      Checks[item.code] == undefined
                        ? ""
                        : Checks[item.code].checked == 1
                        ? "1"
                        : ""
                    }
                    onClick={() => {
                      SelectChange(item.code);
                    }}
                  />
                  <label
                    className="custom-control-label"
                    id={item.code + "label"}
                    htmlFor={item.code + "input"}
                  >
                    {item.code} - {item.name}
                  </label>
                </div>
              );
            })}
          </div>
        );
      })}
    </div>
  );

  const AllChecked = (pChecks) => {
    if (pChecks == undefined) return false;
    const ChecksTab = Object.values(Checks);
    if (ChecksTab.length == 0) return false;
    const ret =
      ChecksTab.filter((item) => {
        return item.checked == 0;
      }).length == 0;

    return ret;
  };

  return (
    <div>
      <div className="custom-control custom-checkbox mb-3">
        <input
          type="checkbox"
          className="custom-control-input"
          id="customCheckAll"
          name="all"
          onClick={SelectAllChange}
          checked={AllChecked(Checks) ? "1" : ""}
        />
        <label className="custom-control-label" htmlFor="customCheckAll">
          Select all
        </label>
        {content}
      </div>
    </div>
  );
};

CheckBoxList.defaultProps = {
  items: [],
  Columns: 1,
  ChecksChanged: () => {}
};

export default CheckBoxList;
