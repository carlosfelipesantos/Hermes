import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeusFretes } from './meus-fretes';

describe('MeusFretes', () => {
  let component: MeusFretes;
  let fixture: ComponentFixture<MeusFretes>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MeusFretes]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MeusFretes);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
