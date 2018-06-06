import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class SearchService {

  endPoint: string = "http://localhost:57203/api/search/";  

  constructor(private httpClient: HttpClient) { }

  search(searchText: string) {
    return this.httpClient.get(this.endPoint + searchText);
  }

}
