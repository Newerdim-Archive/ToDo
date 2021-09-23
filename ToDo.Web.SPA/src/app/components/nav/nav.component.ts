import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isNavbarShown = false;

  constructor() {
  }

  ngOnInit(): void {
  }

  toggleNavbar(): void {
    this.isNavbarShown = !this.isNavbarShown;
  }
}
