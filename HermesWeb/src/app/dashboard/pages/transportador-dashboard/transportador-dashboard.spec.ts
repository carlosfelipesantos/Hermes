import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportadorDashboard } from './transportador-dashboard';

describe('TransportadorDashboard', () => {
  let component: TransportadorDashboard;
  let fixture: ComponentFixture<TransportadorDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransportadorDashboard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransportadorDashboard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
