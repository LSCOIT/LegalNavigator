import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { api } from '../../../api/api';
import { ActivatedRoute, Router } from '@angular/router';
import { NavigateDataService } from '../navigate-data.service';
import { PaginationService } from '../search/pagination.service';
import { IResourceFilter } from '../search/search-results/search-results.model';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable()
export class ServiceOrgService {
 
  constructor(
    private http: HttpClient,
    private navigateDataService: NavigateDataService,
    private router: Router,
    private paginationService: PaginationService
  ) { }

  searchResults: any;
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: { "state": "", "county": "", "city": "", "zipCode": "" }, IsResourceCountRequired: true };
  topIntent: string;
  activeResource: any;

  getOrganizationDetails(location): Observable<any> {
    var objectToSend = JSON.stringify(location);
    return this.http.post<any>(api.getOrganizationDetailsUrl, objectToSend, httpOptions);
  }

  clickSeeMoreOrganizations(resourceType: string, activeId: string) {

    this.resourceFilter.ResourceType = resourceType;
    this.resourceFilter.TopicIds = [];
    if (activeId) {
      this.resourceFilter.TopicIds.push(activeId);
    }
    this.resourceFilter.Location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    this.resourceFilter.IsResourceCountRequired = true;
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.searchResults = response;
        this.searchResults.topIntent = this.topIntent;
        this.searchResults.resourceType = resourceType;
        this.searchResults.isItFromTopicPage = true;
        this.navigateDataService.setData(this.searchResults);
        this.router.navigate(['/search']);
      }
    });
  }
}
