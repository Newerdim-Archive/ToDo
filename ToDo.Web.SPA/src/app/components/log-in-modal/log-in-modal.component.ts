import {Component, OnInit} from '@angular/core';
import {ModalService} from "../../services/modal.service";
import {GoogleLoginProvider, SocialAuthService} from "angularx-social-login";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {ModalId} from "../../enums/ModalId";
import {ExternalAuthProvider} from "../../enums/ExternalAuthProvider";

@Component({
  selector: 'app-log-in-modal',
  templateUrl: './log-in-modal.component.html',
  styleUrls: ['./log-in-modal.component.scss']
})
export class LogInModalComponent implements OnInit {

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
    this.modalService.hideCurrent()
  }

  showSignUpModal(): void {
    this.modalService.show(ModalId.SignUp)
  }

  logInWithGoogle(): void {
    this.isLoading = true;

    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID)
      .then(user => {
        this.authService.externalLogIn(user.idToken, ExternalAuthProvider.Google)
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
