import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserListPaginationComponent } from './user-list-pagination.component';

describe('UserListPaginationComponent', () => {
  let component: UserListPaginationComponent;
  let fixture: ComponentFixture<UserListPaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserListPaginationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserListPaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
