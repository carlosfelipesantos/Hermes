import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Rastreamento } from './rastreamento';

describe('Rastreamento', () => {
  let component: Rastreamento;
  let fixture: ComponentFixture<Rastreamento>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Rastreamento]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Rastreamento);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
