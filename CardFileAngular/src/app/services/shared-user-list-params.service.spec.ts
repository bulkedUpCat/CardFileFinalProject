import { TestBed } from '@angular/core/testing';

import { SharedUserListParamsService } from './shared-user-list-params.service';

describe('SharedUserListParamsService', () => {
  let service: SharedUserListParamsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SharedUserListParamsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
