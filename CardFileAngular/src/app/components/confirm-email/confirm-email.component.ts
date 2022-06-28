import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmEmailDTO } from 'src/app/models/user/ConfirmEmailDTO';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {

  constructor(private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private notifier: NotifierService) { }

  ngOnInit(): void {
    const token = this.route.snapshot.queryParams['token'];
    const email = this.route.snapshot.queryParams['email'];

    const model: ConfirmEmailDTO = {
      email: email,
      token: token
    };

    this.confirmEmail(model);
  }

  confirmEmail(model: ConfirmEmailDTO){
    this.authService.confirmEmail(model).subscribe(res => {
      this.notifier.showNotification("Your email has been confirmed","OK","SUCCESS");
      this.router.navigateByUrl('/login');
    }, err => {
      this.notifier.showNotification(err.error,"OK","ERROR");
    });
  }
}
