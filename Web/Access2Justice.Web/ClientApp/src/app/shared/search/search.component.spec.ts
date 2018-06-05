import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button.component';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button.component';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button.component';
import { SearchComponent } from './search.component';
import { SearchResultsComponent } from './search-results.component';
import { SearchFilterComponent } from './search-filter.component';
import { ResourceCardComponent } from '../resource/resource-card/resource-card.component';
import { NavigateDataService } from '../navigate-data.service';
import { SearchService } from './search.service';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
  let router: RouterModule;
  let searchService: SearchService;
  let searchResults: any = {};

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchComponent, SearchResultsComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        PrintButtonComponent,
        SearchFilterComponent, ResourceCardComponent],
      imports: [
        RouterModule.forRoot([
          { path: 'search', component: SearchResultsComponent }
        ]),
        HttpClientModule, FormsModule],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        SearchService, NavigateDataService],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  xit('make api call after entering search text', () => {
    spyOn(searchService, 'search').and.returnValue(Observable.of());
    fixture.detectChanges();
    expect(searchService.search).toHaveBeenCalled();
  });

});
