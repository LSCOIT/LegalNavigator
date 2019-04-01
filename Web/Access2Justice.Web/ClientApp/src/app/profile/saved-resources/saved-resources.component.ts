import { Component, OnDestroy, OnInit, Optional } from '@angular/core';
import { TabDirective } from 'ngx-bootstrap';
import { Subscription } from 'rxjs';

import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { ResourceResult } from '../../common/search/search-results/search-result';
import { EventUtilityService } from '../../common/services/event-utility.service';

export interface SortingParams {
  field: '_ts' | 'name';
  order: 'ASC' | 'DESC';
}

export interface SortingOptions {
  label: string;
  params: SortingParams;
}

export const SORTING_OPTIONS: SortingOptions[] = [
  {
    label: 'A-Z',
    params: {field: 'name', order: 'ASC'}
  },
  {
    label: 'Z-A',
    params: {field: 'name', order: 'DESC'}
  },
  {
    label: 'Newest to Oldest',
    params: {field: '_ts', order: 'DESC'}
  },
  {
    label: 'Oldest to Newest',
    params: {field: '_ts', order: 'ASC'}
  },
];

@Component({
  selector: 'app-saved-resources',
  templateUrl: './saved-resources.component.html',
  styleUrls: ['./saved-resources.component.scss']
})
export class SavedResourcesComponent implements OnInit, OnDestroy {
  personalizedResources: {
    resources: any[];
    topics: any[];
    webResources: any[];
  };
  countsByType: ResourceResult[];
  typeFilter = 'All';
  sortingParams: SortingParams = {field: '_ts', order: 'DESC'};

  private resources: any[] = [];
  private eventsSub: Subscription;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    @Optional() private tab: TabDirective,
    private eventUtilityService: EventUtilityService
  ) {
  }

  get processedResources(): any[] {
    let result: any[];

    if (this.typeFilter && this.typeFilter !== 'All') {
      result = this.resources.filter(resource => resource.resourceType === this.typeFilter);
    } else {
      result = this.resources.slice(0);
    }

    if (this.sortingParams) {
      const field = this.sortingParams.field;
      const orderModifier = this.sortingParams.order === 'ASC' ? 1 : -1;

      result.sort((res1, res2) => {
        if (res1[field] === res2[field]) {
          return 0;
        } else if (res1[field] > res2[field]) {
          return orderModifier;
        } else {
          return -1 * orderModifier;
        }
      });
    }
    return result;
  }

  get filterDropdownLabel(): string {
    let label = 'Resource type';

    if (this.typeFilter && Array.isArray(this.countsByType)) {
      const activeFilter = this.countsByType.find(filter => filter.ResourceName === this.typeFilter);
      if (activeFilter) {
        label = `${activeFilter.ResourceName} (${activeFilter.ResourceCount})`;
      }
    }

    return label;
  }

  get filterDropdownItems(): ResourceResult[] {
    if (!this.typeFilter) {
      return this.countsByType;
    }

    return this.countsByType.filter(filter => filter.ResourceName !== this.typeFilter);
  }

  get sortingDropdownLabel(): string {
    if (this.sortingParams) {
      const activeOption = SORTING_OPTIONS.find(
        option => option.params.field === this.sortingParams.field && option.params.order === this.sortingParams.order
      );
      if (activeOption) {
        return activeOption.label;
      }
    }

    return 'Sort by';
  }

  get sortingOptions(): SortingOptions[] {
    if (!this.sortingParams) {
      return SORTING_OPTIONS;
    } else {
      return SORTING_OPTIONS.filter(
        option => !(option.params.field === this.sortingParams.field && option.params.order === this.sortingParams.order)
      );
    }
  }

  ngOnInit() {
    if (this.tab) {
      this.tab.select.subscribe(() => this.getResources());
    } else {
      this.getResources();
    }

    this.eventsSub = this.eventUtilityService.resourceUpdated$.subscribe(() => this.getResources());
  }

  ngOnDestroy() {
    if (this.eventsSub) {
      this.eventsSub.unsubscribe();
    }
  }

  private getResources(): void {
    this.personalizedPlanService.getPersonalizedResources().subscribe(personalizedResources => {
      this.personalizedResources = personalizedResources;
      console.log(personalizedResources);
      if (this.personalizedResources) {
        this.resources = this.personalizedResources.resources.concat(
          this.personalizedResources.topics,
          this.personalizedResources.webResources
        );

        const countsByType: Map<string, number> = new Map<string, number>();
        this.resources.forEach(resource => {
          const typeCount: number = countsByType.has(resource.resourceType) ? countsByType.get(resource.resourceType) : 0;
          countsByType.set(resource.resourceType, typeCount + 1);
        });
        this.countsByType = [
          {
            ResourceName: 'All',
            ResourceCount: this.resources.length
          },
          ...Array.from(countsByType, ([type, count]) => ({
            ResourceName: type,
            ResourceCount: count
          }))
        ];

        if (!countsByType.has(this.typeFilter)) {
          this.typeFilter = 'All';
        }
      }
    });
  }
}
