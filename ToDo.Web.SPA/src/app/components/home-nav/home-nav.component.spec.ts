import {ComponentFixture, TestBed} from "@angular/core/testing";

import {HomeNavComponent} from "./home-nav.component";
import {ToggleMenuIconComponent} from "../toggle-menu-icon/toggle-menu-icon.component";
import {IconComponent} from "../icon/icon.component";

describe("NavComponent", () => {
  let component: HomeNavComponent;
  let fixture: ComponentFixture<HomeNavComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomeNavComponent, ToggleMenuIconComponent, IconComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeNavComponent);
    component = fixture.componentInstance;
    compiled = fixture.debugElement.nativeElement;
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("#toggleNavbar() should toggle the #isNavbarShown", () => {
    component.isNavbarShown = false;

    component.toggleNavbar();

    expect(component.isNavbarShown).toBeTruthy();

    component.toggleNavbar();

    expect(component.isNavbarShown).toBeFalsy();
  });

  it("#isNavbarShow should toggle an active class on the navbar", () => {
    let navbar = compiled.querySelector(".navbar") as HTMLDivElement;

    expect(navbar.classList).not.toContain("active");

    component.isNavbarShown = true;

    fixture.detectChanges();

    expect(navbar.classList).toContain("active");
  });

  it("The nav toggle button should toggle #isNavbarShown after click", () => {
    const openNavbarButton = compiled.querySelector<HTMLButtonElement>(
      ".nav__button--toggle"
    );

    openNavbarButton!.click();

    fixture.detectChanges();

    expect(component.isNavbarShown).toBeTruthy();

    openNavbarButton!.click();

    fixture.detectChanges();

    expect(component.isNavbarShown).toBeFalsy();
  });
});
