import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportadorPage } from './transportador-page';

describe('TransportadorPage', () => {
  let component: TransportadorPage;
  let fixture: ComponentFixture<TransportadorPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransportadorPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransportadorPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
