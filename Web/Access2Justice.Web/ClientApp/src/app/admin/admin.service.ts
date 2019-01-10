import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { api } from "../../api/api";

@Injectable()
export class AdminService {
  constructor(private httpClient: HttpClient) {}

  savePrivacyData(input: any) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.httpClient.post(api.updatePrivacyDataUrl, input, httpOptions);
  }

  saveAboutData(input: any) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.httpClient.post(api.updateAboutDataUrl, input, httpOptions);
  }

  saveHomeData(input: any) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.httpClient.post(api.updateHomeDataUrl, input, httpOptions);
  }
  
  saveHelpAndFaqData(input: any) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.httpClient.post(
      api.updateHelpAndFaqDataUrl,
      input,
      httpOptions
    );
  }

  savePersonalizedPlanData(input: any) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.httpClient.post(
      api.updatePersonalizedPlanDataUrl,
      input,
      httpOptions
    );
  }
}
