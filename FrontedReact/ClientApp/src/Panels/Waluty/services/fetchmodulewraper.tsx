let DoFakeFetch = false;
let Response = {};
let Succes = true;
let MockedFuncion = undefined;
const FetchWraper = (url, params) => {
  if (!DoFakeFetch) return fetch(url, params);
  else return MockedFuncion(url, params);
};

const EnableMockFetch = (
  MockedFetchFuncion: (url: string, params: any) => {}
) => {
  DoFakeFetch = true;
  MockedFuncion = MockedFetchFuncion;
};
const DisableMockFetch = () => {
  DoFakeFetch = false;
};
export { EnableMockFetch, DisableMockFetch };
export default FetchWraper;
