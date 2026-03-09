import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreightsComponent } from './freights.component';

describe('FreightsComponent', () => {
  let component: FreightsComponent;
  let fixture: ComponentFixture<FreightsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FreightsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FreightsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
