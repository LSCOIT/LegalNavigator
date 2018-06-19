import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'searchFilter', pure: true })
export class SearchFilterPipe implements PipeTransform {
    reverse: boolean = false;
    transform(items: Array<any>, args: any[]): any[] {
      let filter = args['filter'];
      if (!items || !args || filter == undefined) {
            return items;
        }

        let source = args['source'];
        let filterParam: string;
        let sortParam: string;

        filterParam = filter['filterParam'];
        sortParam = filter['sortParam'];

      if (filterParam != undefined) {
            if (filterParam != 'All') {
                items = items.filter(item => item.resourceType == filterParam);
            }
            else {
                items = items;
            }
        }

      if (sortParam != undefined) {
            if (sortParam == 'name') {
                return this.sortOrder(this.orderBy(items, sortParam));
            }
            else if (sortParam == 'date' && source == 'internal') {
                return this.sortOrder(this.sortDate(items, '_ts'));
            }
            else if (sortParam == 'date' && source == 'external') {
                return this.sortOrder(this.sortDate(items, 'dateLastCrawled'));
            }
        }

        return items;

    }

    sortOrder(items) {
        if (this.reverse == true) {
            this.reverse = false;
            items = items.slice().reverse();
        }
        else if (this.reverse == false) {
            this.reverse = true;
        }
        return items;
    }
 
    sortDate(items, field) {
        let data = items.sort(function (a, b) {
            return new Date(b[field]).getTime() - new Date(a[field]).getTime();
        });
        
        return data;
    }

    orderBy(items, field) {
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




