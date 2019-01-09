import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class EventUtilityService {
  private resourceUpdatedSource = new Subject<any>();
  private upperNavClickedSource = new Subject<any>();

  resourceUpdated$ = this.resourceUpdatedSource.asObservable();
  upperNavClicked$ = this.upperNavClickedSource.asObservable();

  constructor() { }

  updateSavedResource(event: any) {
    this.resourceUpdatedSource.next(event);
  }

  closeSideNav(event: any) {
    this.upperNavClickedSource.next(event);
  }
}
