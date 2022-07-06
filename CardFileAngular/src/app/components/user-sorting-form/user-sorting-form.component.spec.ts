import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSortingFormComponent } from './user-sorting-form.component';

describe('UserSortingFormComponent', () => {
  let component: UserSortingFormComponent;
  let fixture: ComponentFixture<UserSortingFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserSortingFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserSortingFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
