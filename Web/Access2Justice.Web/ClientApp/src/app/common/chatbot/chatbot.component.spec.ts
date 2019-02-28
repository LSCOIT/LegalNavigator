import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { SearchService } from "../search/search.service";
import { ChatbotComponent } from "./chatbot.component";

describe("ChatbotComponent", () => {
  let component: ChatbotComponent;
  let fixture: ComponentFixture<ChatbotComponent>;
  let mockSearchService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ChatbotComponent],
      providers: [
        {
          provide: SearchService,
          useValue: mockSearchService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatbotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
