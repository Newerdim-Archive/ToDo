import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthModalsComponent } from './auth-modals.component';

describe('AuthModalsComponent', () => {
  let component: AuthModalsComponent;
  let fixture: ComponentFixture<AuthModalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthModalsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthModalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
