import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreteTable } from './frete-table';

describe('FreteTable', () => {
  let component: FreteTable;
  let fixture: ComponentFixture<FreteTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FreteTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FreteTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
