import {Component, OnInit} from '@angular/core';
import {ModalId} from "../../enums/ModalId";
import {GoogleLoginProvider, SocialAuthService} from "angularx-social-login";
import {ExternalAuthProvider} from "../../enums/ExternalAuthProvider";
import {ModalService} from "../../services/modal.service";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-auth-modals',
  templateUrl: './auth-modals.component.html',
  styleUrls: ['./auth-modals.component.scss']
})
export class AuthModalsComponent implements OnInit {

  errorMessage = '';
  modalId = ModalId;

  constructor(private modalService: ModalService,
              private socialAuthService: SocialAuthService,
              private authService: AuthService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  hideLogInModal(): void {
    this.modalService.hide(ModalId.LogIn);
  }

  showSignUpModal(): void {
    this.modalService.hideAll();
    this.modalService.show(ModalId.SignUp);
  }

  hideSignUpModal(): void {
    this.modalService.hide(ModalId.SignUp);
  }

  showLogInModal(): void {
    this.modalService.hideAll();
    this.modalService.show(ModalId.LogIn)
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

  signUpWithGoogle(): void {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID).then(user => {
      this.authService.externalSignUp(user.idToken, ExternalAuthProvider.Google).subscribe(
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
