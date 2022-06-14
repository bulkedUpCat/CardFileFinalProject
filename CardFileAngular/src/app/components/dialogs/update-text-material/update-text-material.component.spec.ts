import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateTextMaterialComponent } from './update-text-material.component';

describe('UpdateTextMaterialComponent', () => {
  let component: UpdateTextMaterialComponent;
  let fixture: ComponentFixture<UpdateTextMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateTextMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateTextMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
