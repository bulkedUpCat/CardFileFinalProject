import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextMaterialsComponent } from './text-materials.component';

describe('TextMaterialsComponent', () => {
  let component: TextMaterialsComponent;
  let fixture: ComponentFixture<TextMaterialsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TextMaterialsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TextMaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
