import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailPdfComponent } from './email-pdf.component';

describe('EmailPdfComponent', () => {
  let component: EmailPdfComponent;
  let fixture: ComponentFixture<EmailPdfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailPdfComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailPdfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
