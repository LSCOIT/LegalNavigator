import { Injectable } from '@angular/core';

@Injectable()

export class NavigateDataService {
  searchResult: any;
  getResourceData() {
    return this.searchResult;
  }
  setResourceData(data: any) {
    this.searchResult = data;
  }

  searchText: any;
  getsearchText() {
    return this.searchText;
  }
  setsearchText(data: any) {
    this.searchText = data;
  }
}
