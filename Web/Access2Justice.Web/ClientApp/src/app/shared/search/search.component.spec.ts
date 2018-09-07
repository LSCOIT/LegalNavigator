import { APP_BASE_HREF } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NavigateDataService } from '../navigate-data.service';
import { Observable } from 'rxjs';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button/print-button.component';
import { ResourceCardComponent } from '../resource/resource-card/resource-card.component';
import { RouterModule } from '@angular/router';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button/save-button.component';
import { SearchComponent } from './search.component';
import { SearchFilterComponent } from './search-filter/search-filter.component';
import { SearchFilterPipe } from './search-filter.pipe';
import { SearchResultsComponent } from './search-results/search-results.component';
import { SearchService } from './search.service';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button/share-button.component';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
  let router: RouterModule;
  let searchService: SearchService;
  let searchResults: any = {};
  let mockSearchService;

  beforeEach(async(() => {
    mockSearchService = jasmine.createSpyObj(['search']);
    TestBed.configureTestingModule({
      declarations: [
        SearchComponent, 
        SearchResultsComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        PrintButtonComponent,
        SearchFilterComponent, 
        ResourceCardComponent,
        SearchFilterPipe
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'search', component: SearchResultsComponent }
        ]),
        HttpClientModule, FormsModule],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        { provide: SearchService, useValue: mockSearchService },
        NavigateDataService],
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
});
