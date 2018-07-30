import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class EventUtilityService {
  private resourceUpdatedSource = new Subject<any>();
  resourceUpdated$ = this.resourceUpdatedSource.asObservable();

  constructor() { }

  updateSavedResource(event: any) {
    this.resourceUpdatedSource.next(event);
  }
}
