import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerRegister } from './player-register';

describe('PlayerRegister', () => {
  let component: PlayerRegister;
  let fixture: ComponentFixture<PlayerRegister>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlayerRegister],
    }).compileComponents();

    fixture = TestBed.createComponent(PlayerRegister);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
