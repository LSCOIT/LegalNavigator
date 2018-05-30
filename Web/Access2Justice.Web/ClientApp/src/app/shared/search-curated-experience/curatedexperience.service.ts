import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { CuratedExperience } from './curatedexperience';
import { CuratedExp } from './mock-curated-experience';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CuratedExperienceService {

  getSearchCuratedExperience(): Observable<CuratedExperience[]> {
    return of(CuratedExp);
  }

  constructor() { }

}
