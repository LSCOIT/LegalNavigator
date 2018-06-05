import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';

import { RouterModule } from '@angular/router';

import { SearchResultsComponent } from './search-results.component';
import { SearchFilterComponent } from './search-filter.component';
import { ResourceCardComponent } from '../resource/resource-card/resource-card.component';
import { GuidedAssistantSidebarComponent } from '../sidebars/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../sidebars/service-org-sidebar.component';
import { SaveButtonComponent } from '../resource/user-action/save-button.component';
import { ShareButtonComponent } from '../resource/user-action/share-button.component';
import { NavigateDataService } from '../navigate-data.service';
import { isNullOrUndefined } from 'util';
import { ResourceResult } from '../search/search-result';
import { WebResourceComponent } from '../resource/web-resource/web-resource.component';

describe('SearchResultsComponent', () => {
  let component: SearchResultsComponent;
  let fixture: ComponentFixture<SearchResultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        SearchResultsComponent,
        SearchFilterComponent,
        ResourceCardComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        WebResourceComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'search', component: SearchResultsComponent }
        ])
      ],
      providers: [NavigateDataService,
        { provide: APP_BASE_HREF, useValue: '/' }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
