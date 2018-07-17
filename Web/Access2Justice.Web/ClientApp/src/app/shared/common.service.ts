import { Injectable } from '@angular/core';

@Injectable()
export class CommonService {

  constructor() { }

  checkObjectExistInArray(objects, object): boolean {
    let isObjectExists = false;
    objects.forEach(item => {
      if (JSON.stringify(item) === JSON.stringify(object)) {
        isObjectExists = true;
      }
    });
    return isObjectExists;
  }

}
