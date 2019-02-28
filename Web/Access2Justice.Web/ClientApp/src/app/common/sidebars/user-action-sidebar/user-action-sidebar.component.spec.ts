import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Global } from "../../../global";
import { UserActionSidebarComponent } from "./user-action-sidebar.component";

describe("UserActionSidebarComponent", () => {
  let component: UserActionSidebarComponent;
  let fixture: ComponentFixture<UserActionSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UserActionSidebarComponent],
      providers: [
        { provide: Global, 
          useValue: { 
            role: "", 
            shareRouteUrl: "" 
          } 
        }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserActionSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
