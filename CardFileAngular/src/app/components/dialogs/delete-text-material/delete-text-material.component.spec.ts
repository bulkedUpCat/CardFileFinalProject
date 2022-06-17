import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteTextMaterialComponent } from './delete-text-material.component';

describe('DeleteTextMaterialComponent', () => {
  let component: DeleteTextMaterialComponent;
  let fixture: ComponentFixture<DeleteTextMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteTextMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteTextMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
