import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FretesDisponiveis } from './fretes-disponiveis';

describe('FretesDisponiveis', () => {
  let component: FretesDisponiveis;
  let fixture: ComponentFixture<FretesDisponiveis>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FretesDisponiveis]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FretesDisponiveis);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
