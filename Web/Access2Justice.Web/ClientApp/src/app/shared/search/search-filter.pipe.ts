import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'searchFilter', pure: true })
export class SearchFilterPipe implements PipeTransform {
  //reverse: boolean = false;
  source: any;
  filterParam: string;
  sortParam: string;
  order: string;
  transform(items: Array<any>, args: any[]): any[] {
    let filter = args['filter'];
    if (!items || !args || !filter) {
      return items;
    }

    this.source = args['source']
    this.filterParam = filter['filterParam'];
    this.sortParam = filter['sortParam'];
    this.order = filter['order'];

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
    if (this.order === "DESC") {
      //this.reverse = false;
      items = items.slice().reverse();
    //} else if (!this.reverse) {
    //  this.reverse = true;
    } return items;
  }

  sortDate(items, field) {
    let data = items.sort((a, b) => {
      a = new Date(a[field]).getTime();
      b = new Date(b[field]).getTime();
      return  a < b ? -1 : a > b ? 1 : 0;
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
