import { Injectable } from '@angular/core';

@Injectable()

 // we can use this service for transferring data between two components while routing.
export class NavigateDataService {

  data: any;

  getData() {
    return this.data;
  }
  setData(data: any) {
    this.data = data;
  }

}
