import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-toggle-menu-icon',
  templateUrl: './toggle-menu-icon.component.html',
  styleUrls: ['./toggle-menu-icon.component.scss']
})
export class ToggleMenuIconComponent implements OnInit {
  @Input() isMenuOpen = false;

  constructor() {
  }

  ngOnInit(): void {
  }

}
