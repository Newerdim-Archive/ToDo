import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ToggleMenuIconComponent} from './toggle-menu-icon.component';

describe('ToggleMenuIconComponent', () => {
  let component: ToggleMenuIconComponent;
  let fixture: ComponentFixture<ToggleMenuIconComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ToggleMenuIconComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ToggleMenuIconComponent);
    component = fixture.componentInstance;
    compiled = fixture.debugElement.nativeElement;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('#isMenuOpen toggle open class', () => {
    let svg = compiled.querySelector("svg") as SVGElement;

    expect(svg.classList).not.toContain('open');

    component.isMenuOpen = true;

    fixture.detectChanges();

    expect(svg.classList).toContain('open');
  })
});
