import React, { Component, useEffect } from "react";
import { SelectErrors } from "./reducers/MainReducer";
import { useSelector } from "react-redux";

const ErrorViewer = () => {
  let content = null;
  const Errors = useSelector(SelectErrors);
  if (Errors.length > 0) {
    content = Errors.map((item) => {
      return <div>{item}</div>;
    });

    content = (
      <div className="alert alert-danger" role="alert">
        {content}
      </div>
    );
  }
  return content;
};

export default ErrorViewer;
