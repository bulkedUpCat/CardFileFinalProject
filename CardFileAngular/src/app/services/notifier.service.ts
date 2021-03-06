import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotifierComponent } from '../components/dialogs/notifier/notifier.component';

@Injectable({
  providedIn: 'root'
})
export class NotifierService {

  constructor(private snackBar: MatSnackBar) { }

  showNotification(message: string, buttonMessage: string, messageType: 'ERROR' | 'SUCCESS'){
    this.snackBar.openFromComponent(NotifierComponent,{
      data: {
        message: message,
        buttonMessage: buttonMessage,
        type: messageType
      },
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: messageType
    });
  }
}
