import {ComponentFixture, TestBed} from '@angular/core/testing';

import {IconComponent} from './icon.component';

describe('IconComponent', () => {
  let component: IconComponent;
  let fixture: ComponentFixture<IconComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IconComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IconComponent);
    compiled = fixture.debugElement.nativeElement;
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create #iconHref', () => {
    expect(component.iconHref).toBe('assets/svg/-icon.svg#-icon')
  })

  it('#createIconHref() should create href', () => {
    component.name = 'hamburger';
    component.path = 'assets';
    component.extension = 'png';

    expect(component.createIconHref()).toBe('assets/hamburger-icon.png#hamburger-icon');
  })

  it('should add href to svg use element', () => {
    const svgUseHref = compiled.querySelector<SVGUseElement>('.icon use')?.getAttribute("href");

    expect(svgUseHref).toBe('assets/svg/-icon.svg#-icon');
  })
});
