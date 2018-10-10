import { ActivatedRoute, Router } from '@angular/router';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GuidedAssistantSidebarComponent } from '../../../sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from '../../../map/map.service';
import { NavigateDataService } from '../../../navigate-data.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PaginationService } from '../../../pagination/pagination.service';
import { ServiceOrgSidebarComponent } from '../../../sidebars/service-org-sidebar/service-org-sidebar.component';
import { VideosComponent } from './videos.component';
import { Global } from '../../../../global';

describe('VideosComponent', () => {
  let component: VideosComponent;
  let fixture: ComponentFixture<VideosComponent>;
  let mockRouter;
  let mockResource =[
    {
       "id":"6f6511e3-4c15-4664-9125-4d025ec52cf5",
       "name":"Moving In",
       "type":"Evictions and Tenant Issues",
       "description":"Conversations about Landlord Tenant Law in Alaska",
       "resourceType":"Videos",
       "url":"https://www.youtube.com/embed/pCPGSTYsYoU",
       "topicTags":[
          {
             "id":"62a93f03-8234-46f1-9c35-b3146a96ca8b"
          }
       ],
       "location":[
          {
             "state":"Hawaii",
             "city":"Kalawao",
             "zipCode":"96761"
          }
       ],
       "icon":"./assets/images/resources/resource.png",
       "overview":"This video covers some issues everyone should think about when moving into a new rental housing."
    }
 ]
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      declarations: [ 
        VideosComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent 
      ],
      schemas: [ NO_ERRORS_SCHEMA ],
      providers: [
        { provide: ActivatedRoute,
          useValue: {snapshot: {params: {'id': '123'}}}
        }, 
        { provide: Router, userValue: mockRouter },
        MapService,
        NavigateDataService,
        PaginationService,
        Global
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideosComponent);
    component = fixture.componentInstance;
    component.resource = mockResource;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
