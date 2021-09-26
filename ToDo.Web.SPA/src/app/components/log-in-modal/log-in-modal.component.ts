import {Component, OnInit} from '@angular/core';
import {ModalService} from "../../services/modal.service";
import {ModalId} from "../../enums/ModalId";
import {GoogleLoginProvider, SocialAuthService} from "angularx-social-login";
import {ExternalAuthProvider} from "../../enums/ExternalAuthProvider";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-log-in-modal',
  templateUrl: './log-in-modal.component.html',
  styleUrls: ['./log-in-modal.component.scss']
})
export class LogInModalComponent implements OnInit {

  errorMessage = '';

  constructor(private modalService: ModalService,
              private socialAuthService: SocialAuthService,
              private authService: AuthService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  hideModal(): void {
    this.modalService.hide(ModalId.LogIn);
  }

  showSignUpModal(): void {
    this.modalService.hideAll();
    this.modalService.show(ModalId.SignUp);
  }

  logInWithGoogle(): void {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID).then(user => {
      this.authService.externalLogIn(user.idToken, ExternalAuthProvider.Google).subscribe(
        () => {
          this.router.navigate(['/app']);
        },
        error => {
          console.log(error);
          this.errorMessage = error.error.message;
        });
    });
  }
}
