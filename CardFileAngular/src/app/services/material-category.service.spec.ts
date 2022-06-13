import { TestBed } from '@angular/core/testing';

import { MaterialCategoryService } from './material-category.service';

describe('MaterialCategoryService', () => {
  let service: MaterialCategoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MaterialCategoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
