import { TestBed } from '@angular/core/testing';

import { ApprovalStatusGuard } from './approval-status.guard';

describe('ApprovalStatusGuard', () => {
  let guard: ApprovalStatusGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ApprovalStatusGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
