import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextMaterialDetailComponent } from './text-material-detail.component';

describe('TextMaterialDetailComponent', () => {
  let component: TextMaterialDetailComponent;
  let fixture: ComponentFixture<TextMaterialDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TextMaterialDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TextMaterialDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
