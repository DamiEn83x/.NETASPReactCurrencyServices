import React, { useEffect } from "react";
import { WalutyKursy } from "./reducers/MainReducer";

import { useSelector, useStore } from "react-redux";
const StateDebuger = () => {
  const stateWalutyKursy = useSelector(WalutyKursy);

  const store = useStore();
  let StateFromStore = store.getState().Main.stateWalutyKursy;

  return (
    <div className="card">
      <div className="card-header bg-primary text-white">State view</div>
      <div className="card-body p-4">
        State {JSON.stringify(StateFromStore)}
      </div>
    </div>
  );
};

export default StateDebuger;
