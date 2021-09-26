import {Component, OnInit} from '@angular/core';
import {ModalService} from '../../services/modal.service';
import {ModalId} from "../../enums/ModalId";
import {GoogleLoginProvider, SocialAuthService} from "angularx-social-login";
import {AuthService} from "../../services/auth.service";
import {ExternalAuthProvider} from "../../enums/ExternalAuthProvider";
import {Router} from "@angular/router";

@Component({
  selector: 'app-sign-up-modal',
  templateUrl: './sign-up-modal.component.html',
  styleUrls: ['./sign-up-modal.component.scss']
})
export class SignUpModalComponent implements OnInit {

  errorMessage = '';

  constructor(private modalService: ModalService,
              private socialAuthService: SocialAuthService,
              private authService: AuthService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  hideModal(): void {
    this.modalService.hide(ModalId.SignUp);
  }

  showLogInModal(): void {
    this.modalService.hideAll();
    this.modalService.show(ModalId.LogIn)
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
