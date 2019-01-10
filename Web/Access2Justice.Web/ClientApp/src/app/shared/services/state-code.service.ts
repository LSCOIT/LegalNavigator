import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { api } from "../../../api/api";

const httpOptions = {
  headers: new HttpHeaders({ "Content-Type": "application/json" })
};

@Injectable()
export class StateCodeService {
  constructor(private httpClient: HttpClient) {}

  getStateCodes() {
    return this.httpClient.get(api.getStateCodesUrl, httpOptions);
  }

  getStateCode(stateName) {
    let params = new HttpParams().set("stateName", stateName);
    return this.httpClient.get(api.getStateCodeUrl + "?" + params, httpOptions);
  }

  getStateName(stateCode) {
    let params = new HttpParams().set("stateCode", stateCode);
    return this.httpClient.get(api.getStateNameUrl + "?" + params, httpOptions);
  }
}
