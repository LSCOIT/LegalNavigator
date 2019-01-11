import { Injectable } from "@angular/core";

@Injectable()
export class NavigateDataService {
  data: any;

  getData() {
    return this.data;
  }
  setData(data: any) {
    this.data = data;
  }
}
