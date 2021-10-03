import {Component, OnInit} from '@angular/core';
import {ModalId} from "../../enums/ModalId";
import {GoogleLoginProvider, SocialAuthService} from "angularx-social-login";
import {ExternalAuthProvider} from "../../enums/ExternalAuthProvider";
import {ModalService} from "../../services/modal.service";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-sign-up-modal',
  templateUrl: './sign-up-modal.component.html',
  styleUrls: ['./sign-up-modal.component.scss']
})
export class SignUpModalComponent implements OnInit {

  isLoading = false;
  errorMessage = '';

  constructor(private modalService: ModalService,
              private socialAuthService: SocialAuthService,
              private authService: AuthService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  hide(): void {
    this.modalService.hideCurrent();
  }

  showLogInModal(): void {
    this.modalService.show(ModalId.LogIn)
  }

  signUpWithGoogle(): void {
    this.isLoading = true;

    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID)
      .then(user => {
        this.authService.externalSignUp(user.idToken, ExternalAuthProvider.Google)
          .subscribe(
            () => {
              this.router.navigate(['/app']);
            },
            error => {
              this.isLoading = false;
              this.errorMessage = error.errors.message;
            }
          );
      })
      .catch(() => this.isLoading = false);
  }
}
