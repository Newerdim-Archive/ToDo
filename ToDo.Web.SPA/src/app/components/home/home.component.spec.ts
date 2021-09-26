import {ComponentFixture, TestBed} from "@angular/core/testing";

import {HomeComponent} from "./home.component";
import {HomeNavComponent} from "../home-nav/home-nav.component";
import {IconComponent} from "../icon/icon.component";
import {ToggleMenuIconComponent} from "../toggle-menu-icon/toggle-menu-icon.component";

describe("HomeComponent", () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        HomeComponent,
        HomeNavComponent,
        IconComponent,
        ToggleMenuIconComponent,
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
