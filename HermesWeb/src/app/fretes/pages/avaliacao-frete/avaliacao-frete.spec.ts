import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvaliacaoFrete } from './avaliacao-frete';

describe('AvaliacaoFrete', () => {
  let component: AvaliacaoFrete;
  let fixture: ComponentFixture<AvaliacaoFrete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AvaliacaoFrete]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvaliacaoFrete);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
