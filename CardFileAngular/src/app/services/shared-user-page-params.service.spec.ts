import { TestBed } from '@angular/core/testing';

import { SharedUserPageParamsService } from './shared-user-page-params.service';

describe('SharedUserPageParamsService', () => {
  let service: SharedUserPageParamsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SharedUserPageParamsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
