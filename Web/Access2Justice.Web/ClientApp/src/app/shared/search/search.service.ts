import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { api } from '../../../api/api';

@Injectable()
export class SearchService {

  constructor(private httpClient: HttpClient) { }

  search(searchText: string) {
    return this.httpClient.get(api.searchUrl + '/' + searchText);
  }
}
