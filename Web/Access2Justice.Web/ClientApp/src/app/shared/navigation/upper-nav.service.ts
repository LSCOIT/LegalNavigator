import { Injectable } from '@angular/core';

@Injectable()
export class UpperNavService {

  constructor() { }
  data: any;

  getData() {
    return this.data;
  }

  setData(data: any) {
    this.data = data;
  }
}
