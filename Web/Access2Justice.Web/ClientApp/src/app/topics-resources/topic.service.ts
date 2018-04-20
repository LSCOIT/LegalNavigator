import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

import { Topic } from '../topics-resources/topic';
import { TOPICS } from './mock-topics';

@Injectable()
export class TopicService {

  getTopics(): Observable<Topic[]> {
    return of(TOPICS);
  }

  constructor() { }

}
