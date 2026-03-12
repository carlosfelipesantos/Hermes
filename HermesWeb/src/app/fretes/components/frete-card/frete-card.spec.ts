import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreteCard } from './frete-card';

describe('FreteCard', () => {
  let component: FreteCard;
  let fixture: ComponentFixture<FreteCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FreteCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FreteCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
