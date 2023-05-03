import React from "react";
import { render } from "react-dom";
import { Line } from "react-chartjs-2";

const Chart = ({ data, labels }) => {
  const ldata = {
    labels: labels,
    datasets: [
      {
        label: "ref. index ",
        fill: false,
        lineTension: 0.1,
        backgroundColor: "rgba(75,192,192,0.4)",
        borderColor: "rgba(75,192,192,1)",
        borderCapStyle: "butt",
        borderDash: [],
        borderDashOffset: 0.0,
        borderJoinStyle: "miter",
        pointBorderColor: "rgba(75,192,192,1)",
        pointBackgroundColor: "#fff",
        pointBorderWidth: 1,
        pointHoverRadius: 5,
        pointHoverBackgroundColor: "rgba(75,192,192,1)",
        pointHoverBorderColor: "rgba(220,220,220,1)",
        pointHoverBorderWidth: 2,
        pointRadius: 1,
        pointHitRadius: 10,
        data: data
      }
    ]
  };
  //console.log("data", ldata.datasets[0].data);
  // console.log("labels", ldata.labels);
  const lineOptions = {
    scales: {
      xAxes: [
        {
          gridLines: {
            display: false
          }
        }
      ],
      yAxes: [
        {
          // stacked: true,
          gridLines: {
            display: false
          },
          ticks: {
            beginAtZero: false,
            // Return an empty string to draw the tick line but hide the tick label
            // Return `null` or `undefined` to hide the tick line entirely
            userCallback(value) {
              // Convert the number to a string and splite the string every 3 charaters from the end
              value = value.toString();
              value = value.split(/(?=(?:...)*$)/);

              // Convert the array to a string and format the output
              value = value.join(".");
              return `Index.${value}`;
            }
          }
        }
      ]
    },
    legend: {
      display: false
    },
    tooltips: {
      enabled: true
    }
  };

  return <Line data={ldata} options={lineOptions} />;
};

export default Chart;
