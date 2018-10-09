import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatbotComponent } from './chatbot.component';
import { expand } from 'rxjs/operator/expand';

describe('ChatbotComponent', () => {
  let component: ChatbotComponent;
  let fixture: ComponentFixture<ChatbotComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChatbotComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatbotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should return style if showtyle:true by calling getStyle', () => {   
    component.showStyle = true;
    let mockStyle = '#1d0dff';
    let tmpStyle = component.getStyle();
    expect(tmpStyle.length).toEqual(mockStyle.length);
  });

  it('should return null if showstyle:false by calling getStyle', () => {
    component.showStyle = false;
    let mockStyle = '#1d0dff';
    let tmpStyle = component.getStyle();
    expect(tmpStyle.length).toBeLessThan(7);
  });
});
