import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShareButtonComponent } from './share-button.component';
import { BsModalService, ComponentLoaderFactory, PositioningService } from 'ngx-bootstrap';

describe('ShareButtonComponent', () => {
  let component: ShareButtonComponent;
  let fixture: ComponentFixture<ShareButtonComponent>;
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ShareButtonComponent],
      providers: [BsModalService, ComponentLoaderFactory, PositioningService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShareButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
