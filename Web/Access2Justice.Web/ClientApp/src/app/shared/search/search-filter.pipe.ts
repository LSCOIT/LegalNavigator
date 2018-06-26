import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'searchFilter', pure: true })
export class SearchFilterPipe implements PipeTransform {
  reverse: boolean = false;
  source: any;
  filterParam: string;
  sortParam: string;
  transform(items: Array<any>, args: any[]): any[] {
    let filter = args['filter'];
    if (!items || !args || !filter) {
      return items;
    }

    this.source = args['source']
    this.filterParam = filter['filterParam'];
    this.sortParam = filter['sortParam'];

    if (this.filterParam != undefined) {
      if (this.filterParam !== 'All') {
        items = items.filter(item => item.resourceType === this.filterParam);
      }
    }

    if (this.sortParam != undefined) {
      if (this.sortParam === 'name') {
        return this.sortOrder(this.orderBy(items, this.sortParam));
      } else if (this.sortParam === 'date' && this.source === 'internal') {
        return this.sortOrder(this.sortDate(items, '_ts'));
      } else if (this.sortParam === 'date' && this.source === 'external') {
        return this.sortOrder(this.sortDate(items, 'dateLastCrawled'));
      }
    } return items;
  }

  sortOrder(items) {
    if (this.reverse) {
      this.reverse = false;
      items = items.slice().reverse();
    } else if (!this.reverse) {
      this.reverse = true;
    } return items;
  }

  sortDate(items, field) {
    let data = items.sort((a, b) => {
      return new Date(b[field]).getTime() - new Date(a[field]).getTime();
    });
    return data;
  }

  orderBy(items, field) {
    return items.sort((a, b) => {
      if (a[field] < b[field]) {
        return -1;
      } else if (a[field] > b[field]) {
        return 1;
      } else {
        return 0;
      }
    });
  }
}
