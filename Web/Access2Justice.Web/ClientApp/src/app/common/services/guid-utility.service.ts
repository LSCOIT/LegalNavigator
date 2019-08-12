import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GuidUtilityService {

  constructor() { }

  getGuidFromResourceUrl(url: string) {
    const pattern =  /[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}/;
    const found = url.match(pattern);
    return found && found.length === 1 ? found[0] : null ;
  }
}
