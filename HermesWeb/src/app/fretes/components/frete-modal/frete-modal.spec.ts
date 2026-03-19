import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreteModal } from './frete-modal';

describe('FreteModal', () => {
  let component: FreteModal;
  let fixture: ComponentFixture<FreteModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FreteModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FreteModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
