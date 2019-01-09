import { TestBed, inject } from '@angular/core/testing';
import { ArrayUtilityService } from './array-utility.service';

describe('UtilityService', () => {
  let mockDataItems = ['test1', 'test2', 'test3'];
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ArrayUtilityService]
    });
  });

  it('should be created', inject([ArrayUtilityService], (service: ArrayUtilityService) => {
    expect(service).toBeDefined();
  }));

  it('should be created', inject([ArrayUtilityService], (service: ArrayUtilityService) => {
    expect(service).toBeTruthy();
  }));

  it('should check whether object exist in objects', inject([ArrayUtilityService], (service: ArrayUtilityService) => {  
    let mockDataItem = 'test1';
    let isExist = service.checkObjectExistInArray(mockDataItems, mockDataItem);
    expect(isExist).toBe(true);
  }));

  it('should check whether object exist in objects', inject([ArrayUtilityService], (service: ArrayUtilityService) => {
    
    let itemNotExist = 'notexist';
    let isExist = service.checkObjectExistInArray(mockDataItems, itemNotExist);
    expect(isExist).toBe(false);
  }));

});
