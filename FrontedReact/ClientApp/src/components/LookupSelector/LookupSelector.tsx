import React from "react";

const LookupSelecetor = ({ Items, pOnChangeSelect }) => {
  const onChangeSelect = (event) => {
    console.log("onChangeSelect");
    pOnChangeSelect(event.target.value);
  };

  const content = Items.map((item) => {
    return (
      <option value={item.code}>
        {item.code} - {item.name}
      </option>
    );
  });
  return (
    <select
      data-testid="select"
      className="form-control"
      onChange={onChangeSelect}
    >
      <option key="SELECT_ANY" disabled>
        Select Any{" "}
      </option>
      {content}
    </select>
  );
};

LookupSelecetor.defaultProps = {
  Items: [],
  pOnChangeSelect: () => {}
};

export default LookupSelecetor;
