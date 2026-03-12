import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvaliacaoSucesso } from './avaliacao-sucesso';

describe('AvaliacaoSucesso', () => {
  let component: AvaliacaoSucesso;
  let fixture: ComponentFixture<AvaliacaoSucesso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AvaliacaoSucesso]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvaliacaoSucesso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
