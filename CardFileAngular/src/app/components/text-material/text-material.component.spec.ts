import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextMaterialComponent } from './text-material.component';

describe('TextMaterialComponent', () => {
  let component: TextMaterialComponent;
  let fixture: ComponentFixture<TextMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TextMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TextMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
