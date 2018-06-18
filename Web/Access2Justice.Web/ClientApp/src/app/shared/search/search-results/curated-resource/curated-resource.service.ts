import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { CuratedResource } from './curated-resource';
import { curatedRes } from './mock-curated-resource';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CuratedResourceService {

  getCuratedResource(): Observable<CuratedResource[]> {
    return of(curatedRes);
  }

  constructor() { }

}
