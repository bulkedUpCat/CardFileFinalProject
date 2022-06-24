import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TextMaterialService } from 'src/app/services/text-material.service';

@Component({
  selector: 'app-reject-text-material',
  templateUrl: './reject-text-material.component.html',
  styleUrls: ['./reject-text-material.component.css']
})
export class RejectTextMaterialComponent implements OnInit {
  rejectForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
    private dialogRef: MatDialogRef<RejectTextMaterialComponent>,
    @Inject(MAT_DIALOG_DATA) private data,
    private textMaterialService: TextMaterialService) { }

  ngOnInit(): void {
    this.createRejectForm();
  }

  createRejectForm(){
    this.rejectForm = this.fb.group({
      id: [this.data.id],
      rejectMessage: [null,[Validators.required]]
    });
  }

  get rejectMessage(){
    return this.rejectForm.get('rejectMessage');
  }

  onSubmit(){
    this.submitted = true;
    if (!this.rejectForm.valid){
      return;
    }

    const model = this.rejectForm.value;
    console.log(model);

    this.textMaterialService.rejectTextMaterial(model.id, model.rejectMessage).subscribe(x => {
      this.dialogRef.close();
    }, err => {
      console.log(err);
    });
  }
}
