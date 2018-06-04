import { Pipe, PipeTransform } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Pipe({ name: 'searchFilter', pure: false })
export class SearchFilterPipe implements PipeTransform {
  transform(items: Array<any>, args: any[]): any[] {
    console.log(args['source']);
    let source = args['source']
    let filter = args['filter'];
    let filterParam:string;
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
        return items.filter(item => item.resourceType == filterParam)
      }
      else {
        return items;
      }
    }
    if (!isNullOrUndefined(filter) && actionType == 'sort') {
      if (filterParam == 'name') {
        console.log("name");
        return items.sort(function (a, b) {
          if (a[filterParam] < b[filterParam]) {
            return -1;
          }
          else if (a[filterParam] > b[filterParam]) {
            return 1;
          }
          else {
            return 0;
          }
        });
      }
      //ToDo - Implement date sorting
      //else if (filterParam == 'date' && source == 'internal') {
      //}
      //ToDo - Implement date sorting for external source
      else if (filterParam == 'date' && source == 'external') {
        //console.log("date");
        //items.sort(function (a, b) {
        //  console.log(a);
        //  console.log(b);
        //  //return new Date(b.dateLastCrawled) - new Date(a['dateLastCrawled']);
        //  return 0;
        //});
      }
      else {
        return items;
      }
    }
  }
}




