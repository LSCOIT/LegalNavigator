import { Injectable } from '@angular/core';
@Injectable()

export class NavigateDataService
{
 
  titlefield: any;

  getSubtopics() {
    return this.titlefield;
  }
  settitlefield(data: any) {
    this.titlefield = data;
  }
}

