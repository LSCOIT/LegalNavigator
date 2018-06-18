import { SearchFilterPipe } from '../search/search-filter.pipe';
import { SearchComponent } from './search.component';

describe('Pipe: SearchFilterPipe', () => {
  let pipe: SearchFilterPipe;
  let items: Array<any>;
  let args: any[];
  let itemsWithNull: Array<any>;
  let argsWithNull: any[];

  beforeEach(() => {
    pipe = new SearchFilterPipe();
    items = ['item1', 'item2', 'item3'];
    args = ['args1', 'args2', 'args3']
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
});
