import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CuratedExperienceResultComponent } from './curated-experience-result.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { MapService } from '../../shared/map/map.service';
import { StateCodeService } from '../../shared/state-code.service';
import { HttpClientModule } from '@angular/common/http';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { Global } from '../../global';
import { PersonalizedPlanService } from '../personalized-plan/personalized-plan.service';
import { Location } from "@angular/common";

fdescribe('CuratedExperienceResultComponent', () => {
	let component: CuratedExperienceResultComponent;
	let fixture: ComponentFixture<CuratedExperienceResultComponent>;
	let mockToastr;
	let mockNavigateDataService;
	let mockGuidedAssistantResults;
	let mockRouter;
	let mockArrayUtilityService;
	let mockGlobal;
	let mockPersonalizedPlanService;
	let mockMapLocation = {
		state: "California",
		city: "Riverside County",
		county: "Indio",
		zipCode: "92201"
	};

	let mockDisplayLocationDetails = {
		locality: "Indio",
		address: "92201"
	};

	let mockLocationDetails = {
		location: mockMapLocation,
		displayLocationDetails: mockDisplayLocationDetails,
		country: "United States",
		formattedAddress: "Hjorth St, Indio, California 92201, United States"
	};

	let mockSavedTopics = ["Divorce", "ChildCustody"];
	let mockLocation = {
		subscribe: () => { }
	};

	beforeEach(async(() => {
		mockNavigateDataService = jasmine.createSpyObj(['getData']);
		mockToastr = jasmine.createSpyObj(['success']);
		mockRouter = jasmine.createSpyObj(['navigateByUrl']);
		mockPersonalizedPlanService = jasmine.createSpyObj(['saveTopicsToProfile']);

		mockGuidedAssistantResults = {
			"topIntent": "Divorce",
			"relevantIntents": [
				"Domestic Violence",
				"Tenant's rights",
				"None"
			],
			"topicIds": [
				"e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
			],
			"guidedAssistantId": "9a6a6131-657d-467d-b09b-c570b7dad242"
		}
		TestBed.configureTestingModule({
			imports: [HttpClientModule],
			declarations: [CuratedExperienceResultComponent],
			schemas: [NO_ERRORS_SCHEMA],
			providers: [
				{ provide: NavigateDataService, useValue: mockNavigateDataService },
				{ provide: ToastrService, useValue: mockToastr },
				{ provide: Router, useValue: mockRouter }, MapService,
				StateCodeService,
				{ provide: ArrayUtilityService, useValue: mockArrayUtilityService },
				{ provide: Global, useValue: { mockGlobal, userId: 'UserId', topicsSessionKey: 'test' } },
				{ provide: PersonalizedPlanService, useValue: mockPersonalizedPlanService },
				{ provide: Location, useValue: mockLocation }
			]
		})
			.compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(CuratedExperienceResultComponent);
		component = fixture.componentInstance;
		mockNavigateDataService.getData.and.returnValue(mockGuidedAssistantResults);
		component.ngOnInit();
		fixture.detectChanges();

		let store = {};
		const mockSessionStorage = {
			getItem: (key: string): string => {
				return key in store ? store[key] : null;
			},
			setItem: (key: string, value: string) => {
				store[key] = `${value}`;
			},
			removeItem: (key: string) => {
				delete store[key];
			},
			clear: () => {
				store = {};
			}
		};
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	it('should filter out relevant intents that are None', () => {
		component.filterIntent();
		expect(component.relevantIntents).toEqual(
			[
				"Domestic Violence",
				"Tenant's rights"
			]
		)
	});

	it('should call toastr when Save for Later button is clicked', () => {
		component.relevantIntents = [
			"Domestic Violence",
			"Tenant's rights"
		];
		fixture.debugElement
			.query(By.css('.btn-secondary'))
			.triggerEventHandler('click', { stopPropogration: () => { } });
		const button = fixture.debugElement.query(By.css('.btn-secondary'));
		button.triggerEventHandler('click', { stopPropogration: () => { } });
		expect(mockToastr.success).toHaveBeenCalled();
	});

	it('should call saveTopicsToProfile if user is logged in', () => {
		let mockIntentInput = { location: mockMapLocation, intents: mockSavedTopics };
		component.savedTopics = mockSavedTopics;
		spyOn(sessionStorage, 'getItem').and.returnValue(JSON.stringify(mockLocationDetails));
		component.saveForLater();
		expect(component.locationDetails).toEqual(mockLocationDetails);
		expect(component.intentInput).toEqual(mockIntentInput);
		expect(mockPersonalizedPlanService.saveTopicsToProfile).toHaveBeenCalledWith(mockIntentInput, true);
	});

	it('should call saveTopicsToProfile if user is logged in', () => {
		mockGlobal.userId = null;
		component.savedTopics = mockSavedTopics;
		spyOn(sessionStorage, 'setItem');
		component.saveForLater();
		expect(mockToastr.success).toHaveBeenCalled();
	});
});
