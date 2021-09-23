import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { IconComponent } from './components/icon/icon.component';
import { NavComponent } from './components/nav/nav.component';
import { ToggleMenuIconComponent } from './components/toggle-menu-icon/toggle-menu-icon.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    IconComponent,
    NavComponent,
    ToggleMenuIconComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
