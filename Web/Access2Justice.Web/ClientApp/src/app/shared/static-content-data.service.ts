import { Injectable } from '@angular/core';

@Injectable()
export class StaticContentDataService {
  data: any;

  constructor() { }

  getData() {
    return this.data;
  }
  setData(data: any) {
    this.data = data;
  }

}
