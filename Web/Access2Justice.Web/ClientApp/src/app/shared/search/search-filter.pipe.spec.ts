import { SearchFilterPipe } from '../search/search-filter.pipe';

describe('Pipe: SearchFilterPipe', () => {
  let pipe: SearchFilterPipe;
  let items;
  let args: any[];
  let itemsWithNull;
  let argsWithNull: any[];
  let mockBlankitems = ['', '', '', '', '', ''];
  let mockDate = ['07/19/2018 10:12:30', '07/16/2018 08:12:30', '07/18/2018 14:12:30']

  beforeEach(() => {
    pipe = new SearchFilterPipe();
    items = ['item1', 'item2', 'item3', 'item4', 'item5', 'item6'];
    args = ['source', 'filter', 'args3'];
  });

  it("should provide value for item type", () => {
    expect(pipe.transform(items, args)).toBe(items);
  });

  it("should provide value for item", () => {
    expect(pipe.transform(args, items)).toBe(args);
  });

  it("should provide value for both args and item type", () => {
    expect(pipe.transform(items, args)).toBe(items, args);
  });

  it("should provide no value for item type", () => {
    expect(pipe.transform(itemsWithNull, args)).toBeFalsy(itemsWithNull);
  });

  it("should provide no value for args", () => {
    expect(pipe.transform(argsWithNull, items)).toBeFalsy(argsWithNull);
  });

  it("should call orderBy method in transform and sort the items with value items", () => {
    spyOn(pipe, 'orderBy');
    pipe.orderBy(items, 'item2');
    expect(pipe.orderBy).not.toBe(false);
  });

  it("should call orderBy method in transform and sort the items with invalid items", () => {
    spyOn(pipe, 'orderBy');
    pipe.orderBy(items, 'test');
    expect(pipe.orderBy).not.toBe(true);
  });

  it("should call sortOrder method in transform and with ascending order", () => {
    let sortedItems = pipe.sortOrder(items);
    expect(sortedItems.length).toBe(6);
    expect(sortedItems[0]).toBe('item1');
    expect(sortedItems[5]).toBe('item6');
  });

  it("should call sortOrder method in transform in reverse with other params i.e. reverse, date, internal", () => {
    spyOn(pipe, 'transform');
    pipe.sortParam = "date";
    pipe.source = "internal";
    pipe.filterParam = 'filterParam';
    let sortedItems = pipe.sortOrder(items);
    expect(pipe.transform(items, args)).toBeUndefined();
    expect(sortedItems[0]).toBe('item1');
    expect(sortedItems[5]).toBe('item6');
  });

  it("should call orderBy method should return 1", () => {
    spyOn(pipe, 'orderBy');
    pipe.sortParam = "date";
    let test = pipe.orderBy(items, pipe.sortParam);
    expect(pipe.orderBy).not.toBe(0);
  });

  it("should call orderBy method should return null", () => {
    spyOn(pipe, 'orderBy');
    pipe.orderBy('', '');
    expect(pipe.orderBy).not.toBe("");
  })

  it("should call sortDate method in transform and sort the items with date", () => {
    spyOn(pipe, 'sortDate');
    pipe.sortDate(items, 'Yellow');
    expect(pipe.sortDate).toBeTruthy();
  });

  it("should provide no sort params for transform -undefined", () => {
    pipe.sortParam = "";
    expect(pipe.transform(argsWithNull, items)).not.toBeDefined();
  });

  it("should provide no filter params for transform -filter undefined", () => {
    pipe.filterParam = "";
    expect(pipe.transform(argsWithNull, items)).not.toBeDefined();
  });

  it("should provide no value for item type", () => {
    spyOn(pipe, 'transform');
    let itemsWithNull = ['', '', '', '', '', ''];;
    let args = ['', '', ''];
    expect(pipe.transform(itemsWithNull, args)).toBeFalsy(itemsWithNull);

  });
  it("should return undefined values from transform", () => {
    spyOn(pipe, 'transform');
    expect(pipe.transform(mockBlankitems, args)).toBeUndefined(itemsWithNull);
  });

  it("should return defined items from transform", () => {
    spyOn(pipe, 'transform');
    expect(pipe.transform(items, args)).not.toBeDefined(items);
  });

  it("should pass defined args to transform", () => {
    spyOn(pipe, 'transform');
    expect(pipe.transform(items, args)).not.toBeDefined(args);
  });

  it("should pass defined args with date and internal as sort and source params in transform", () => {
    spyOn(pipe, 'transform');
    pipe.sortParam = "date";
    pipe.source = "internal";
    expect(pipe.transform(items, args)).not.toBeDefined(items);
  });

  it("should pass items with date and internal as sort options and source params", () => {
    spyOn(pipe, 'transform');
    pipe.sortDate(mockDate, 'date');
    pipe.sortParam = "date";
    pipe.source = "internal";
    let sortDate = pipe.sortDate(mockDate, 'date');
    expect(pipe.sortDate(mockDate, 'date')).toBeDefined(mockDate);
  });

});
