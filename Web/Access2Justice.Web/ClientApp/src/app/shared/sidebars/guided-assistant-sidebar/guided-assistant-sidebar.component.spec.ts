import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GuidedAssistantSidebarComponent } from './guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { ModalModule, BsModalService } from 'ngx-bootstrap';
import { MapService } from '../../map/map.service';
import { PaginationService } from '../../pagination/pagination.service';

describe('GuidedAssistantSidebarComponent', () => {
  let component: GuidedAssistantSidebarComponent;
  let fixture: ComponentFixture<GuidedAssistantSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, RouterTestingModule, ModalModule.forRoot()],
      declarations: [GuidedAssistantSidebarComponent],
      providers: [
        BsModalService,
        MapService,
        PaginationService     
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidedAssistantSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
