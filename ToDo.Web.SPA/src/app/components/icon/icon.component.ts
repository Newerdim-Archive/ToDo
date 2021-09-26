import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss']
})
export class IconComponent implements OnInit {
  @Input() name: string = '';
  @Input() path = 'assets/svg';
  @Input() extension = 'svg';

  iconHref = '';

  constructor() {
  }

  ngOnInit(): void {
    this.iconHref = this.createIconHref();
  }

  createIconHref(): string {
    const iconFullName = `${this.name}-icon`;
    const iconId = `${this.name}-icon`;
    const fullPath = `${this.path}/${iconFullName}.${this.extension}`;

    return fullPath + '#' + iconId;
  }
}
