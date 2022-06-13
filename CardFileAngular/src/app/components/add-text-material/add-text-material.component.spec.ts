import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTextMaterialComponent } from './add-text-material.component';

describe('AddTextMaterialComponent', () => {
  let component: AddTextMaterialComponent;
  let fixture: ComponentFixture<AddTextMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddTextMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTextMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
