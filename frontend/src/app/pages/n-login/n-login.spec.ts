import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NLogin } from './n-login';

describe('NLogin', () => {
  let component: NLogin;
  let fixture: ComponentFixture<NLogin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NLogin],
    }).compileComponents();

    fixture = TestBed.createComponent(NLogin);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
