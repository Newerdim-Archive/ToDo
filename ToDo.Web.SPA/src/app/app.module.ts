import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";

import {AppRoutingModule} from "./app-routing.module";
import {ServiceWorkerModule} from "@angular/service-worker";
import {environment} from "../environments/environment";
import {ReactiveFormsModule} from "@angular/forms";
import {GoogleLoginProvider, SocialAuthServiceConfig, SocialLoginModule} from "angularx-social-login";
import {HttpClientModule} from "@angular/common/http";
import {JwtModule} from "@auth0/angular-jwt";

import {AppComponent} from "./app.component";
import {HomeComponent} from "./components/home/home.component";
import {IconComponent} from "./components/icon/icon.component";
import {HomeNavComponent} from "./components/home-nav/home-nav.component";
import {ToggleMenuIconComponent} from "./components/toggle-menu-icon/toggle-menu-icon.component";
import {GoogleButtonComponent} from "./components/google-button/google-button.component";
import {SignUpModalComponent} from './components/sign-up-modal/sign-up-modal.component';
import { LogInModalComponent } from './components/log-in-modal/log-in-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    IconComponent,
    HomeNavComponent,
    ToggleMenuIconComponent,
    GoogleButtonComponent,
    SignUpModalComponent,
    LogInModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ServiceWorkerModule.register("ngsw-worker.js", {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the app is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: "registerWhenStable:30000",
    }),
    ReactiveFormsModule,
    SocialLoginModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('access_token');
        },
      },
    }),
  ],
  providers: [
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(environment.googleClientId)
          }
        ]
      } as SocialAuthServiceConfig,
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
