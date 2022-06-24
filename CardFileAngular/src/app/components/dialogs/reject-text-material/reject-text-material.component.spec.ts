import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectTextMaterialComponent } from './reject-text-material.component';

describe('RejectTextMaterialComponent', () => {
  let component: RejectTextMaterialComponent;
  let fixture: ComponentFixture<RejectTextMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RejectTextMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RejectTextMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
