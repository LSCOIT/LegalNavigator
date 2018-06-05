import { Pipe, PipeTransform } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Pipe({ name: 'searchFilter', pure: true })
export class SearchFilterPipe implements PipeTransform {
  transform(items: Array<any>, args: any[]): any[] {
    let source = args['source']
    let filter = args['filter'];
    let filterParam: string;
    let actionType: string;

    if (!items || !args || isNullOrUndefined(filter)) {
      return items;
    }

    if (!isNullOrUndefined(filter)) {
      filterParam = filter['filterParam'];
      actionType = filter['actionType'];
    }

    if (!isNullOrUndefined(filter) && actionType == 'filter') {
      if (filterParam != 'All') {
        console.log(filterParam);
        items = items.filter(item => item.resourceType == filterParam);
        return items;
      }
      else {
        return items;
      }
    }
    if (!isNullOrUndefined(filter) && actionType == 'sort') {
      if (filterParam == 'name') {
        console.log(filterParam);
        return this.sortOrderBy(items, filterParam);
      }
      else if (filterParam == 'date' && source == 'internal') {
        return this.sortDate(items, 'modifiedTimeStamp');
      }
      else if (filterParam == 'date' && source == 'external') {
        return this.sortDate(items, 'dateLastCrawled');
      }
      else {
        return items;
      }
    }
  }

  sortDate(items, field) {
    let data = items.sort(function (a, b) {
      return new Date(b[field]).getTime() - new Date(a[field]).getTime();
    });
    return data;
  }

  sortOrderBy(items, field) {
    return items.sort(function (a, b) {
      if (a[field] < b[field]) {
        return -1;
      }
      else if (a[field] > b[field]) {
        return 1;
      }
      else {
        return 0;
      }
    });
  }
}




