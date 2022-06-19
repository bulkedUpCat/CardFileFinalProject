import { TestBed } from '@angular/core/testing';

import { SharedHomeParamsService } from './shared-home-params.service';

describe('SharedHomeParamsService', () => {
  let service: SharedHomeParamsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SharedHomeParamsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
