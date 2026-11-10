import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerRegisterPopUp } from './player-register-pop-up';

describe('PlayerRegisterPopUp', () => {
  let component: PlayerRegisterPopUp;
  let fixture: ComponentFixture<PlayerRegisterPopUp>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlayerRegisterPopUp],
    }).compileComponents();

    fixture = TestBed.createComponent(PlayerRegisterPopUp);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
