import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientePage } from './cliente-page';

describe('ClientePage', () => {
  let component: ClientePage;
  let fixture: ComponentFixture<ClientePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClientePage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
