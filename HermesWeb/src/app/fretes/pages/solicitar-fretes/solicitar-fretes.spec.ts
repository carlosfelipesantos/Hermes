import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolicitarFretes } from './solicitar-fretes';

describe('SolicitarFretes', () => {
  let component: SolicitarFretes;
  let fixture: ComponentFixture<SolicitarFretes>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SolicitarFretes]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SolicitarFretes);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
