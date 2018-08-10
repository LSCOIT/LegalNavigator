import { TestBed, inject } from '@angular/core/testing';

import { ArrayUtilityService } from '../shared/array-utility.service';

fdescribe('UtilityService', () => {
  let service: ArrayUtilityService;

  let mockObjectsArray = [{
    "id": "test id",
    "value": "test value"
  }];
  let mockExistingValue = "test id";
  let mockNonExistingValue = "testid";

  beforeEach(() => {
    service = new ArrayUtilityService();
  });

  it('should be created', () => {
    expect(service).toBeDefined();
  });

  it('should return true if object exists in array on calling checkObjectExistInArray method', () => {
    let isObjectExists = service.checkObjectExistInArray(mockObjectsArray, mockExistingValue);
    expect(isObjectExists).toBeTruthy();
  });

  it('should return false if object does not exists in array on calling checkObjectExistInArray method', () => {
    let isObjectExists = service.checkObjectExistInArray(mockObjectsArray, mockNonExistingValue);
    expect(isObjectExists).toBeFalsy();
  });

  it('should return true if object id value exists in array on calling checkObjectItemIdExistInArray method', () => {
    let isObjectExists = service.checkObjectItemIdExistInArray(mockObjectsArray, mockExistingValue);
    expect(isObjectExists).toBeTruthy();
  });

  it('should return true if object id value does not exists in array on calling checkObjectItemIdExistInArray method', () => {
    let isObjectExists = service.checkObjectItemIdExistInArray(mockObjectsArray, mockExistingValue);
    expect(isObjectExists).toBeTruthy();
  });

});
